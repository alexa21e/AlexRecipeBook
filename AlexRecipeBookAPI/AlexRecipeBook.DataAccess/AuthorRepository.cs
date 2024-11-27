using AlexRecipeBook.DataAccess.Abstractions;
using AlexRecipeBook.Domain;
using Neo4j.Driver;

namespace AlexRecipeBook.DataAccess
{
    public class AuthorRepository(INeo4JDataAccess neo4JDataAccess) : IAuthorRepository
    {
        private readonly INeo4JDataAccess _neo4JDataAccess = neo4JDataAccess;

        public async Task<List<Author>> GetMostProlificAuthors(int authorsNumber)
        {
            var query = $@"MATCH (a:Author)-[:WROTE]->(r:Recipe)
                           RETURN a.name AS Author, COUNT(r) AS RecipeCount
                           ORDER BY RecipeCount DESC
                           LIMIT {authorsNumber}";

            var records = await _neo4JDataAccess.ExecuteReadPropertiesAsync(query, null);

            var authors = records.Select(record => Author.Create(record["Author"].As<string>())).ToList();

            return authors;
        }
    }
}
