export class RecipeParams {
    pageNumber = 1;
    pageSize = 20;
    sortOrder: string = '';
    recipeName: string | undefined;
    selectedIngredients: string | undefined;
}