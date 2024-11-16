namespace AlexRecipeBook.Domain
{
    public class Keyword
    {
        public string Name { get; set; } = string.Empty;

        private Keyword() { }

        public static Keyword Create(string name)
        {
            return new Keyword
            {
                Name = name
            };
        }
    }
}
