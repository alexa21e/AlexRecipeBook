using AlexRecipeBook.DataObjects;

namespace AlexRecipeBook.ApplicationServices.Abstractions
{
    public interface IRecipeService
    {
        public Task<int> GetRecipes();
    }
}
