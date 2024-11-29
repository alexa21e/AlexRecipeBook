namespace AlexRecipeBookAPI.Helpers
{
    public class Listing<T>(int ingredientsDisplayedNo, int count, IReadOnlyList<T> data) where T : class
    {
        public int IngredientsDisplayedNo { get; set; } = ingredientsDisplayedNo;
        public int Count { get; set; } = count;
        public IReadOnlyList<T> Data { get; set; } = data;
    }
}
