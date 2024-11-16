using AlexRecipeBook.DataAccess.Abstractions;
using AlexRecipeBook.Domain;
using AlexRecipeBook.DataObjects;

namespace AlexRecipeBook.DataAccess
{
    public class RecipeRepository: IRecipeRepository
    {
        private readonly INeo4JDataAccess _neo4JDataAccess;

        public RecipeRepository(INeo4JDataAccess neo4JDataAccess)
        {
            _neo4JDataAccess = neo4JDataAccess;
        }

        public async Task<int> GetRecipes()
        {
            var parameters = new Dictionary<string, object>();

            var query = $@"MATCH (r:Recipe)-[:CONTAINS_INGREDIENT]->(i:Ingredient), (a:Author)-[:WROTE]->(r) 
                           RETURN count(DISTINCT r)";

            return await _neo4JDataAccess.ExecuteReadScalarAsync<int>(query, parameters);
        }
    }
}
