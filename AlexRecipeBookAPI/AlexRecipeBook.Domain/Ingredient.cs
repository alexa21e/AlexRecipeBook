namespace AlexRecipeBook.Domain
{
    public class Ingredient
    {
        public string Name { get; set; }

        private Ingredient() { }

        public static Ingredient Create(string name)
        {
            return new Ingredient
            {
                Name = name
            };
        }
    }
}
