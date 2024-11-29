import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { IngredientParams } from "../models/ingredientParams";
import { Ingredient } from "../models/ingredient";
import { environment } from "../../../environments/environment";
import { Listing } from "../models/listing";

@Injectable({
    providedIn: 'root'
})

export class IngredientsService {
    baseUrl = environment.baseUrl + '/api';
    
    constructor(private http: HttpClient) {
    }

    getIngredients(ingredientParams: IngredientParams){
        let params = new HttpParams();
        if(ingredientParams.name){
            params = params.append('name', ingredientParams.name);
        }
        params = params.append('ingredientsDisplayedNo', ingredientParams.ingredientsDisplayedNo);
        return this.http.get<Listing<Ingredient[]>>(this.baseUrl + '/Ingredient', {params});
    }

    getMostCommonIngredients(ingredientsNumber: number) {
        let params = new HttpParams();
        params = params.append('ingredientsNumber', ingredientsNumber);
        return this.http.get<Ingredient[]>(this.baseUrl + '/Ingredient/mostCommon', {params});
    }
}