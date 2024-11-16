using AlexRecipeBook.DataObjects;

namespace AlexRecipeBook.DataAccess.Abstractions;

public interface IRecipeRepository
{
    public Task<int> GetRecipes();
}