using AlexRecipeBook.ApplicationServices.Abstractions;
using AlexRecipeBook.DataObjects;
using AlexRecipeBook.Domain.Specifications;
using AlexRecipeBookAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AlexRecipeBookAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController(IRecipeService recipeService) : ControllerBase
    {
        private readonly IRecipeService _recipeService = recipeService;

        [HttpGet]
        public async Task<ActionResult<Pagination<HomeRecipeToReturn>>> GetRecipes([FromQuery] RecipeParam param)
        {
            var recipes = await _recipeService.GetRecipes(param);
            var noRecipes = await _recipeService.GetRecipesCount(param);
            return Ok(new Pagination<HomeRecipeToReturn>(param.PageNumber, param.PageSize, noRecipes, param.SortOrder, recipes));
        }

        [HttpGet("mostcomplex")]
        public async Task<ActionResult<List<RecipeStatsToReturn>>> GetMostComplexRecipes([FromQuery] int recipesNumber)
        {
            var recipes = await _recipeService.GetMostComplexRecipes(recipesNumber);
            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetailedRecipeToReturn>> GetRecipeById([FromRoute] string id)
        {
            var recipe = await _recipeService.GetRecipeById(id);
            return Ok(recipe);
        }
    }
}