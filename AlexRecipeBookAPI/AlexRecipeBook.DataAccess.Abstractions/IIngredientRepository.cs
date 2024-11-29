using AlexRecipeBook.Domain;

namespace AlexRecipeBook.DataAccess.Abstractions
{
    public interface IIngredientRepository
    {
        Task<List<Ingredient>> GetIngredients(string name, int ingredientsDisplayedNo);
        Task<int> GetNumberOfIngredients(string name);
        Task<List<Ingredient>> GetMostCommonIngredients(int ingredientsNumber);
    }
}
