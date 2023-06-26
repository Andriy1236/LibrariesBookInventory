using AutoMapper;
using LibrariesBookInventory.Application.Book.Common;

namespace LibrariesBookInventory.Application.Book.Update
{
    public class UpdateBookMapper : Profile
    {
        public UpdateBookMapper()
        {
            CreateMap<BookCommand, UpdateBookCommand>();
            CreateMap<UpdateBookCommand, Domain.Models.Book> ();
        }
    }
}
