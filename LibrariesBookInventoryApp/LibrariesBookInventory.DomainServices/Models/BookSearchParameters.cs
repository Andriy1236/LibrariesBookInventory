namespace LibrariesBookInventory.DomainServices.Models
{
    public class SearchBooksParameters
    {
        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        public string SearchText { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
    }
}