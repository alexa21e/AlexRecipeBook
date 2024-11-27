using AlexRecipeBook.ApplicationServices.Abstractions;
using AlexRecipeBook.Domain;
using AlexRecipeBook.Domain.Specifications;
using AlexRecipeBookAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AlexRecipeBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<Ingredient>>> GetIngredients([FromQuery] IngredientParam param)
        {
            var ingredients = await _ingredientService.GetIngredients(param);
            var noIngredients = await _ingredientService.GetNumberOfIngredients(param.Name);
            return Ok(new Listing<Ingredient>(param.IngredientsDisplayedNo, noIngredients, ingredients));
        }
    }
}
