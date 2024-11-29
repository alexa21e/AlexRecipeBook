using AlexRecipeBook.ApplicationServices.Abstractions;
using AlexRecipeBook.Domain;
using AlexRecipeBook.DataAccess.Abstractions;
using AlexRecipeBook.Domain.Specifications;

namespace AlexRecipeBook.ApplicationServices
{
    public class IngredientService(IIngredientRepository ingredientRepository) : IIngredientService
    {
        private readonly IIngredientRepository _ingredientRepository = ingredientRepository;

        public async Task<List<Ingredient>> GetIngredients(IngredientParam param)
        {
            return await _ingredientRepository.GetIngredients(param.Name, param.IngredientsDisplayedNo);
        }

        public async Task<int> GetNumberOfIngredients(string name)
        {
            return await _ingredientRepository.GetNumberOfIngredients(name);
        }

        public async Task<List<Ingredient>> GetMostCommonIngredients(int ingredientsNumber)
        {
            return await _ingredientRepository.GetMostCommonIngredients(ingredientsNumber);
        }
    }
}
