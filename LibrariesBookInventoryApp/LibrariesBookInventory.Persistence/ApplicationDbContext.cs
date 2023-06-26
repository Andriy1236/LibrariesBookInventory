using LibrariesBookInventory.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using LibrariesBookInventory.Domain.Models;

namespace LibrariesBookInventory.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }


        public ApplicationDbContext() { }

        public ApplicationDbContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasKey(x => x.Id);
            modelBuilder.Entity<Book>().Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Book>().Property(x => x.ISBN).HasMaxLength(17).IsRequired();
            modelBuilder.Entity<Book>().Property(x => x.Title).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Book>().Property(x => x.Author).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Book>().Property(x => x.PublicationYear).IsRequired();
            modelBuilder.Entity<Book>().Property(x => x.CategoryId).IsRequired();

            modelBuilder.Entity<Category>().HasKey(p => p.Id);
            modelBuilder.Entity<Category>().Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Category>().Property(p => p.Name).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Category>().Property(p => p.Description).HasMaxLength(100).IsOptional();

            modelBuilder.Entity<Book>()
                 .HasRequired(b => b.Category)
                 .WithMany(c => c.Books)
                 .HasForeignKey(b => b.CategoryId)
                 .WillCascadeOnDelete(false);
        }
    }
}