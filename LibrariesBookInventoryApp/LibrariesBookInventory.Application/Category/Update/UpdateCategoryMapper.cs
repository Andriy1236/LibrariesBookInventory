using AutoMapper;
using LibrariesBookInventory.Application.Category.Update;

namespace LibrariesBookInventory.Application.Category.Common
{
    public class UpdateCategoryMapper : Profile
    {
        public UpdateCategoryMapper()
        {
            CreateMap<BaseCategoryCommand, UpdateCategoryCommand>();
            CreateMap<UpdateCategoryCommand, Domain.Models.Category>();
        }
    }
}
