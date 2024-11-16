namespace AlexRecipeBook.Domain
{
    public class Collection
    {
        string Name { get; set; } = string.Empty;

        private Collection() { }

        public static Collection Create(string name)
        {
            return new Collection()
            {
                Name = name
            };
        }
    }
}
