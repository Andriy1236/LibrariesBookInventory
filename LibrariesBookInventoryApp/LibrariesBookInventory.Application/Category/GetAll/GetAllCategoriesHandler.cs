using AutoMapper;
using LibrariesBookInventory.Application.Common;
using LibrariesBookInventory.DomainServices.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LibrariesBookInventory.Application.Category.GetAll
{
    public class GetAllCategoriesQuery : IRequest<BaseResponse<IEnumerable<CategoryDto>>>
    {
    }
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, BaseResponse<IEnumerable<CategoryDto>>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetAllCategoriesHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<IEnumerable<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Domain.Models.Category> categories = await _categoryRepository.GetAll(cancellationToken);

            return new BaseResponse<IEnumerable<CategoryDto>>() { Data =_mapper.Map<IEnumerable<CategoryDto>>(categories) };
        }
    }
}