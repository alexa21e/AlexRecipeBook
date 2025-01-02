using AlexRecipeBook.DataAccess.Abstractions;
using AlexRecipeBook.DataObjects;
using AlexRecipeBook.Domain;
using Neo4j.Driver;

namespace AlexRecipeBook.DataAccess
{
    public class RecipeRepository(INeo4JDataAccess neo4JDataAccess) : IRecipeRepository
    {
        private readonly INeo4JDataAccess _neo4JDataAccess = neo4JDataAccess;

        public async Task<List<HomeRecipeToReturn>> GetRecipes(int skip, int pageSize, string sortOrder,
            string? recipeName, string[]? selectedIngredients)
        {
            var parameters = new Dictionary<string, object>
            {
                { "skip", skip },
                { "pageSize", pageSize }
            };

            var whereClauses = new List<string>();
            var matchClauses = new List<string>();

            if (!string.IsNullOrEmpty(recipeName))
            {
                whereClauses.Add("WHERE toLower(r.name) CONTAINS toLower($recipeName)");
                parameters.Add("recipeName", recipeName);
            }

            if (selectedIngredients is { Length: > 0 })
            {
                for (var i = 0; i < selectedIngredients.Length; i++)
                {
                    matchClauses.Add($"MATCH (r)-[:CONTAINS_INGREDIENT]->(:Ingredient {{name: $ingredient{i}}})");
                    parameters.Add($"ingredient{i}", selectedIngredients[i]);
                }
            }

            var query = $@"MATCH (r:Recipe)-[:CONTAINS_INGREDIENT]->(i:Ingredient), (a:Author)-[:WROTE]->(r) 
                           {string.Join(" AND ", whereClauses)}
                           {string.Join(" ", matchClauses)}
                           RETURN r.id AS Id, r.name AS Name, a.name AS Author, count(i) AS NumberOfIngredients, r.skillLevel AS SkillLevel
                           {ParseSortOrder(sortOrder)}      
                           SKIP $skip 
                           LIMIT $pageSize";

            var records = await _neo4JDataAccess.ExecuteReadPropertiesAsync(query, parameters);

            return records.Select(record => new HomeRecipeToReturn
            {
                Id = record["Id"].As<string>(),
                Name = record["Name"].As<string>(),
                Author = record["Author"].As<string>(),
                NumberOfIngredients = record["NumberOfIngredients"].As<int>(),
                SkillLevel = record["SkillLevel"].As<string>()
            }).ToList();
        }

        public async Task<int> GetRecipesCount(string? recipeName, string[]? selectedIngredients)
        {
            var parameters = new Dictionary<string, object>();

            var whereClauses = new List<string>();
            var matchClauses = new List<string>();

            if (!string.IsNullOrEmpty(recipeName))
            {
                whereClauses.Add("WHERE toLower(r.name) CONTAINS toLower($recipeName)");
                parameters.Add("recipeName", recipeName);
            }

            if (selectedIngredients is { Length: > 0 })
            {
                for (var i = 0; i < selectedIngredients.Length; i++)
                {
                    matchClauses.Add($"MATCH (r)-[:CONTAINS_INGREDIENT]->(:Ingredient {{name: $ingredient{i}}})");
                    parameters.Add($"ingredient{i}", selectedIngredients[i]);
                }
            }

            var query = $@"MATCH (r:Recipe)-[:CONTAINS_INGREDIENT]->(i:Ingredient), (a:Author)-[:WROTE]->(r) 
                           {string.Join(" AND ", whereClauses)}
                           {string.Join(" ", matchClauses)}
                           RETURN count(DISTINCT r)";

            return await _neo4JDataAccess.ExecuteReadScalarAsync<int>(query, parameters);
        }

        public async Task<List<RecipeStatsToReturn>> GetMostComplexRecipes(int recipesNumber)
        {
            var query = $@"MATCH (r:Recipe)-[:CONTAINS_INGREDIENT]->(i:Ingredient)
                           WITH r, COUNT(i) AS IngredientCount
                           RETURN r.id AS Id, r.name AS Name, IngredientCount, r.skillLevel, r.preparationTime, r.cookingTime, 
                             (IngredientCount + CASE r.skillLevel WHEN 'Easy' THEN 1 WHEN 'More effort' THEN 2 WHEN 'A challenge' THEN 3 END 
                             + r.preparationTime/60 + r.cookingTime/60) AS ComplexityScore
                           ORDER BY ComplexityScore DESC
                           LIMIT {recipesNumber}";

            var records = await _neo4JDataAccess.ExecuteReadPropertiesAsync(query, null);

            var recipes = records.Select(record => new RecipeStatsToReturn()
            {
                Id = record["Id"].As<string>(),
                Name = record["Name"].As<string>()
            }).ToList();

            return recipes;
        }

        public async Task<DetailedRecipeToReturn> GetRecipeById(string id)
        {
            var query = @"MATCH (r:Recipe {id: $id}) - [:CONTAINS_INGREDIENT]->(i: Ingredient)  
                          OPTIONAL MATCH (r) - [:COLLECTION]->(c: Collection)
                          OPTIONAL MATCH (r) - [:KEYWORD]->(k: Keyword)
                          OPTIONAL MATCH (r) - [:DIET_TYPE]->(d: DietType)
                          RETURN r.id AS Id, r.name AS Name, r.description AS Description, r.preparationTime AS PreparationTime, 
                                        r.cookingTime AS CookingTime, r.skillLevel AS SkillLevel, collect(DISTINCT i) AS Ingredients,
                                        collect(DISTINCT c) AS Collections, collect(DISTINCT k) AS Keywords, collect(DISTINCT d) AS DietTypes";

            var parameters = new Dictionary<string, object>
            {
                { "id", id }
            };

            var record = await _neo4JDataAccess.ExecuteReadSingleRecordAsync(query, parameters);

            if (record.Count == 0)
            {
                return null;
            }

            var ingredientsAsNodes = record["Ingredients"].As<ICollection<INode>>();
            var collectionsAsNodes = record["Collections"].As<ICollection<INode>>();
            var keywordsAsNodes = record["Keywords"].As<ICollection<INode>>();
            var dietAsNode = record["DietTypes"].As<ICollection<INode>>();

            var ingredients = ingredientsAsNodes.Select(node => node.Properties["name"].ToString()).ToList();
            var collections = collectionsAsNodes.Select(node => node.Properties["name"].ToString()).ToList();
            var keywords = keywordsAsNodes.Select(node => node.Properties["name"].ToString()).ToList();
            var diets = dietAsNode.Select(node => node.Properties["name"].ToString()).ToList(); ;

            var recipe = new DetailedRecipeToReturn()
            {
                Recipe = Recipe.Create(
                    (string)record["Id"],
                    (string)record["Name"],
                    (string)record["Description"],
                    (long)record["PreparationTime"],
                    (long)record["CookingTime"],
                    (string)record["SkillLevel"]),
                Ingredients = ingredients,
                Collections = collections,
                Keywords = keywords,
                DietTypes = diets
            };

            return recipe;
        }

        public async Task<RecipeNameToReturn> GetRecipeNameById(string id)
        {
            var query = @"MATCH (r:Recipe {id: $id})
                          RETURN r.name AS Name";

            var parameters = new Dictionary<string, object>
            {
                { "id", id }
            };

            var record = await _neo4JDataAccess.ExecuteReadSingleRecordAsync(query, parameters);

            if (record.Count == 0)
            {
                return null;
            }

            var recipe = new RecipeNameToReturn()
            {
                Name = ((string)record["Name"])
            };

            return recipe;
        }

        private static string ParseSortOrder(string sortOrder)
        {
            var parts = sortOrder.Split('_');
            var column = parts[0];
            var direction = parts[1].ToUpper();

            return column switch
            {
                "numberOfIngredients" => $"ORDER BY count(i) {direction}, r.name ASC",
                "skillLevel" => $"ORDER BY CASE r.skillLevel WHEN 'Easy' THEN 1 WHEN 'More effort' THEN 2 WHEN 'A challenge' THEN 3 END {direction}, r.name ASC",
                _ => $"ORDER BY r.name {direction}",
            };
        }
    }
}
