using AlexRecipeBook.DataObjects;

namespace AlexRecipeBook.DataAccess.Abstractions
{
    public interface IRecipeRepository
    {
        Task<List<HomeRecipeToReturn>> GetRecipes(int skip, int pageSize, string sortOrder,
            string? name, string[]? selectedIngredients);
        Task<int> GetRecipesCount(string? name, string[]? selectedIngredients);
        Task<List<RecipeStatsToReturn>> GetMostComplexRecipes(int recipesNumber);
        Task<DetailedRecipeToReturn> GetRecipeById(string id);
        Task<RecipeNameToReturn> GetRecipeNameById(string id);
        Task<List<SimilarRecipeToReturn>> GetFiveMostSimilarRecipes(string id);
        Task<List<HomeRecipeToReturn>> GetRecipesByAuthor(int skip, int pageSize, string sortOrder,
            string authorName,
            string? recipeName, string[]? selectedIngredients);
        Task<int> GetRecipesByAuthorCount(string authorName, string? recipeName,
            string[]? selectedIngredients);
    }
}
