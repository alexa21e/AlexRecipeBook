using AlexRecipeBook.ApplicationServices;
using AlexRecipeBook.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AlexRecipeBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController(IAuthorService authorService) : ControllerBase
    {
        private readonly IAuthorService _authorService = authorService;

        [HttpGet("mostprolific")]
        public async Task<ActionResult<List<Author>>> GetMostProlificAuthors([FromQuery] int authorsNumber)
        {
            return await _authorService.GetMostProlificAuthors(authorsNumber);
        }
    }
}
