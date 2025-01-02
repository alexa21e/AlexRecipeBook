using AlexRecipeBook.Domain;

namespace AlexRecipeBook.DataObjects
{
    public class DetailedRecipeToReturn
    {
        public Recipe Recipe { get; set; }
        public List<string>? Ingredients { get; set; }
        public List<string>? Collections { get; set; }
        public List<string>? Keywords { get; set; }
        public List<string>? DietTypes { get; set; }
    }
}
