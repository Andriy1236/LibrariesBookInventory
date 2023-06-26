using AutoMapper;


namespace LibrariesBookInventory.Application.Category.Get
{
    public class GetCategoryMapper :Profile
    {
        public GetCategoryMapper()
        {
            CreateMap<Domain.Models.Category, CategoryDto>();
        }
    }
}
