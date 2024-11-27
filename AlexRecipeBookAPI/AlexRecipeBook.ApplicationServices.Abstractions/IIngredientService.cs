using AlexRecipeBook.Domain;
using AlexRecipeBook.Domain.Specifications;

namespace AlexRecipeBook.ApplicationServices.Abstractions
{
    public interface IIngredientService
    {
        Task<List<Ingredient>> GetIngredients(IngredientParam param);
        Task<int> GetNumberOfIngredients(string name);
    }
}
