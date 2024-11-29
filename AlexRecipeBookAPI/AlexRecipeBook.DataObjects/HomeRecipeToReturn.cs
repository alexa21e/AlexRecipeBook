namespace AlexRecipeBook.DataObjects
{
    public class HomeRecipeToReturn
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int NumberOfIngredients { get; set; }
        public string SkillLevel { get; set; } = string.Empty;
    }
}
