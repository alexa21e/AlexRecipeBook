namespace AlexRecipeBook.Domain
{
    public class DietType
    {
        public string Name { get; set; } = string.Empty;

        private DietType() { }

        public static DietType Create(string name)
        {
            return new DietType
            {
                Name = name
            };
        }
    }
}
