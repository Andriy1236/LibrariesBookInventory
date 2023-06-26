using AutoMapper;

namespace LibrariesBookInventory.Application.Category.Create
{
    public class CreateCategoryMapper : Profile
    {
        public CreateCategoryMapper()
        {
            CreateMap<CreateCategoryCommand, Domain.Models.Category>();
        }
    }
}