namespace AlexRecipeBookAPI.Helpers
{
    public class Pagination<T>(int pageNumber, int pageSize, int count, string sortOrder, IReadOnlyList<T> data) where T : class
    {
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
        public int Count { get; set; } = count;
        public string SortOrder { get; set; } = sortOrder;
        public IReadOnlyList<T> Data { get; set; } = data;
    }
}
