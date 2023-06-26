using AutoMapper;

namespace LibrariesBookInventory.Application.Book.Create
{
    public class CreateBookMapper : Profile
    {
        public CreateBookMapper()
        {
            CreateMap<CreateBookCommand, Domain.Models.Book>();
        }
    }
}
