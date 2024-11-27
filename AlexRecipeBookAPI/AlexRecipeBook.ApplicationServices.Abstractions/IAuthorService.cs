using AlexRecipeBook.Domain;

namespace AlexRecipeBook.ApplicationServices
{
    public interface IAuthorService
    {
        Task<List<Author>> GetMostProlificAuthors(int authorsNumber);
    }
}
