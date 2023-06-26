using AutoMapper;
using LibrariesBookInventory.Application.Category.Common;
using LibrariesBookInventory.Application.Common;
using LibrariesBookInventory.DomainServices.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LibrariesBookInventory.Application.Category.Create
{

    public class CreateCategoryCommand : BaseCategoryCommand, IRequest<BaseResponse<int>>
    {
    }

    internal class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, BaseResponse<int>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CreateCategoryHandler(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<int>> Handle(CreateCategoryCommand createCategoryCommand, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Domain.Models.Category>(createCategoryCommand);

            _categoryRepository.Create(category);
            await _categoryRepository.SaveChanges(cancellationToken);

            return new BaseResponse<int>() { Data = category.Id };
        }
    }
}