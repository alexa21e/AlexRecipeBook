﻿using AlexRecipeBook.ApplicationServices.Abstractions;
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

        [HttpGet("{id}/name")]
        public async Task<ActionResult<RecipeNameToReturn>> GetRecipeNameById([FromRoute] string id)
        {
            var recipe = await _recipeService.GetRecipeNameById(id);
            return Ok(recipe);
        }

        [HttpGet("{id}/similar")]
        public async Task<ActionResult<List<SimilarRecipeToReturn>>> GetFiveMostSimilarRecipes([FromRoute] string id)
        {
            var recipes = await _recipeService.GetFiveMostSimilarRecipes(id);
            return Ok(recipes);
        }

        [HttpGet("author")]
        public async Task<ActionResult<Pagination<HomeRecipeToReturn>>> GetRecipesByAuthor([FromQuery] AuthorRecipeParameters param)
        {
            var recipes = await _recipeService.GetRecipesByAuthor(param);
            var noRecipes = await _recipeService.GetRecipesByAuthorCount(param);
            return Ok(new Pagination<HomeRecipeToReturn>(param.PageNumber, param.PageSize, noRecipes, param.SortOrder, recipes));
        }
    }
}