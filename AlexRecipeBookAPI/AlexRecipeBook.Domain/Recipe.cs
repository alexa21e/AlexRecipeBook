namespace AlexRecipeBook.Domain
{
    public class Recipe
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long PreparationTime { get; set; }
        public long CookingTime { get; set; }
        public string SkillLevel { get; set; } = string.Empty;

        private Recipe() { }

        public static Recipe Create(string id, string name, string description, long preparationTime, long cookingTime,
            string skillLevel)
        {
            return new Recipe
            {
                Id = id,
                Name = name,
                Description = description,
                PreparationTime = preparationTime,
                CookingTime = cookingTime,
                SkillLevel = skillLevel
            };
        }
    }
}
