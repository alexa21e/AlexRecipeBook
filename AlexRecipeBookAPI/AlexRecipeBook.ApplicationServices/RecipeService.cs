using AlexRecipeBook.ApplicationServices.Abstractions;
using AlexRecipeBook.DataAccess.Abstractions;
using AlexRecipeBook.DataObjects;

namespace AlexRecipeBook.ApplicationServices
{
    public class RecipeService: IRecipeService
    {
        private IRecipeRepository _recipeRepository;
        public RecipeService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }
        public async Task<int> GetRecipes()
        {
            return await _recipeRepository.GetRecipes();
        }
    }
}
