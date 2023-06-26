using AutoMapper;
using LibrariesBookInventory.Application.Common;
using LibrariesBookInventory.DomainServices.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LibrariesBookInventory.Application.Category.Get
{
    public class GetCategoryQuery : IRequest<BaseResponse<CategoryDto>>
    {
        public int Id { get; set; }

        internal class GetCategoryHandler : IRequestHandler<GetCategoryQuery, BaseResponse<CategoryDto>>
        {
            private readonly ICategoryRepository _categoryRepository;
            private readonly IMapper _mapper;
            public GetCategoryHandler(
                ICategoryRepository categoryRepository,
                IMapper mapper)
            {
                _categoryRepository = categoryRepository;
                _mapper = mapper;
            }
            public async Task<BaseResponse<CategoryDto>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
            {
                var category = await _categoryRepository.Get(request.Id, cancellationToken);

                return new BaseResponse<CategoryDto>() { Data = _mapper.Map<CategoryDto>(category) };
            }
        }
    }
}