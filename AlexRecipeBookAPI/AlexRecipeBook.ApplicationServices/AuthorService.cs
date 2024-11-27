using AlexRecipeBook.DataAccess.Abstractions;
using AlexRecipeBook.Domain;

namespace AlexRecipeBook.ApplicationServices
{
    public class AuthorService(IAuthorRepository authorRepository) : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository = authorRepository;

        public async Task<List<Author>> GetMostProlificAuthors(int authorsNumber)
        {
            return await _authorRepository.GetMostProlificAuthors(authorsNumber);
        }
    }
}
