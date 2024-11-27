using AlexRecipeBook.Domain;

namespace AlexRecipeBook.DataAccess.Abstractions
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetMostProlificAuthors(int authorsNumber);
    }
}
