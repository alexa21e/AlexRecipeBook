using AlexRecipeBook.ApplicationServices.Abstractions;
using AlexRecipeBook.DataAccess.Abstractions;
using AlexRecipeBook.DataObjects;
using AlexRecipeBook.Domain.Specifications;

namespace AlexRecipeBook.ApplicationServices
{
    public class RecipeService(IRecipeRepository recipeRepository) : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository = recipeRepository;

        public async Task<List<HomeRecipeToReturn>> GetRecipes(RecipeParam param)
        {
            var skip = (param.PageNumber - 1) * param.PageSize;

            string[] ingredients = [];
            if (!string.IsNullOrEmpty(param.SelectedIngredients))
            {
                ingredients = param.SelectedIngredients.Split(',');
            }

            return await _recipeRepository.GetRecipes(skip, param.PageSize, param.SortOrder, param.RecipeName, ingredients);
        }

        public async Task<int> GetRecipesCount(RecipeParam param)
        {
            string[] ingredients = [];
            if (!string.IsNullOrEmpty(param.SelectedIngredients))
            {
                ingredients = param.SelectedIngredients.Split(',');
            }

            return await _recipeRepository.GetRecipesCount(param.RecipeName, ingredients);
        }

        public async Task<List<RecipeStatsToReturn>> GetMostComplexRecipes(int recipesNumber)
        {
            return await _recipeRepository.GetMostComplexRecipes(recipesNumber);
        }


    }
}
