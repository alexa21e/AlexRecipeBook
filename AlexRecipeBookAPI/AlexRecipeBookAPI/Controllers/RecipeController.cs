using AlexRecipeBook.ApplicationServices.Abstractions;
using AlexRecipeBook.DataObjects;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryRecipesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipesController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        public async Task<ActionResult<int>> GetRecipes()
        {
            var recipes = await _recipeService.GetRecipes();
            return Ok(recipes);
        }
    }
}