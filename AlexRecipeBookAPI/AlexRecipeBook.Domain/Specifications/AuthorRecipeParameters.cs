namespace AlexRecipeBook.Domain.Specifications
{
    public class AuthorRecipeParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortOrder { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public bool ClickedRecipe { get; set; } = false;
        public string? ClickedRecipeId { get; set; } = string.Empty;
        public string? RecipeName { get; set; }
        public string? SelectedIngredients { get; set; }
    }
}
