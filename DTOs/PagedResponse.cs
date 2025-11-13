namespace rise_gs.DTOs
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = null!;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public List<LinkDto> Links { get; set; } = new();
    }
}
