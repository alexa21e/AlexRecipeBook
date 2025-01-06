using AlexRecipeBook.DataObjects;
using AlexRecipeBook.Domain.Specifications;

namespace AlexRecipeBook.ApplicationServices.Abstractions
{
    public interface IRecipeService
    {
        Task<List<HomeRecipeToReturn>> GetRecipes(RecipeParam param);
        Task<int> GetRecipesCount(RecipeParam param);
        Task<List<RecipeStatsToReturn>> GetMostComplexRecipes(int recipesNumber);
        Task<DetailedRecipeToReturn> GetRecipeById(string id);
        Task<RecipeNameToReturn> GetRecipeNameById(string id);
        Task<List<SimilarRecipeToReturn>> GetFiveMostSimilarRecipes(string id);
        Task<List<HomeRecipeToReturn>> GetRecipesByAuthor(AuthorRecipeParameters param);
        Task<int> GetRecipesByAuthorCount(AuthorRecipeParameters param);

    }
}
