﻿namespace LibrariesBookInventory.Domain.Models
{
    public class Book
    {
        public Book() { }
        public Book(
            string title,
            string author,
            string isbn,
            int publicationYear,
            int quantity,
            int categoryId)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            PublicationYear = publicationYear;
            Quantity = quantity;
            CategoryId = categoryId;
        }
        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int PublicationYear { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}