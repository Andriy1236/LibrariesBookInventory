namespace LibrariesBookInventory.Application.Book.Common
{
    public class BookCommand
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int? PublicationYear { get; set; }
        public int? Quantity { get; set; }
        public int? CategoryId { get; set; }
    }
}