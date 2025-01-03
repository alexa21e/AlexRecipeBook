import { Recipe } from "./recipe";

export interface DetailedRecipe{
    recipe: Recipe;
    ingredients: string[];
    collections: string[];
    keywords: string[];
    dietTypes: string[];
}