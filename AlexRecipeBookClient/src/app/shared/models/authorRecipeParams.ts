export class AuthorRecipeParams {
    pageNumber: number = 1;
    pageSize: number = 20;
    sortOrder: string = '_asc';
    authorName: string = '';
    clickedRecipe: boolean = false;
    clickedRecipeId: string | undefined;
    recipeName: string | undefined;
    selectedIngredients: string | undefined;
}