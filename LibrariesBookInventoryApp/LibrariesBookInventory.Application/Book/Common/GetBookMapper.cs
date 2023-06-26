using AutoMapper;

namespace LibrariesBookInventory.Application.Book.Common
{
    public class GetBookMapper : Profile
    {
        public GetBookMapper()
        {
            CreateMap<Domain.Models.Book, BookDto>();
        }
    }
}