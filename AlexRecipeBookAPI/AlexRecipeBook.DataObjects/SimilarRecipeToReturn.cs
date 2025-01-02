namespace AlexRecipeBook.DataObjects
{
    public class SimilarRecipeToReturn
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Similarity { get; set; }
    }
}
