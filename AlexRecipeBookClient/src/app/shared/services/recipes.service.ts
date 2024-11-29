import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { RecipeParams } from "../models/recipeParams";
import { Pagination } from "../models/pagination";
import { HomeRecipe } from "../models/homeRecipe";
import { environment } from "../../../environments/environment";
import { RecipeStats } from "../models/recipeStats";

@Injectable({
    providedIn: 'root'
})

export class RecipesService {
    baseUrl = environment.baseUrl + '/api';
    constructor(private http: HttpClient) {
    }

    getRecipes(recipeParams: RecipeParams){
        let params = new HttpParams();
        params = params.append('pageNumber', recipeParams.pageNumber);
        params = params.append('pageSize', recipeParams.pageSize);
        params = params.append('sortOrder', recipeParams.sortOrder);
        if(recipeParams.recipeName){
            params = params.append('recipeName', recipeParams.recipeName);
        }
        if(recipeParams.selectedIngredients){
            params = params.append('selectedIngredients', recipeParams.selectedIngredients);
        }
        return this.http.get<Pagination<HomeRecipe[]>>(this.baseUrl + '/Recipes', {params});
    }

    getMostComplexRecipes(recipesNumber: number){
        let params = new HttpParams();
        params = params.append('recipesNumber', recipesNumber);
        return this.http.get<RecipeStats[]>(this.baseUrl + '/Recipes/mostComplex', {params});
    }
}