using AutoMapper;
using LibrariesBookInventory.Application.Category.Common;
using LibrariesBookInventory.Application.Common;
using LibrariesBookInventory.DomainServices.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LibrariesBookInventory.Application.Category.Update
{
    public class UpdateCategoryCommand : BaseCategoryCommand, IRequest<EmptyResponse>
    {
        public int Id { get; set; }
    }

    internal class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, EmptyResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public UpdateCategoryHandler(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<EmptyResponse> Handle(UpdateCategoryCommand updateCategoryCommand, CancellationToken cancellationToken)
        {
            Domain.Models.Category category = _mapper.Map<Domain.Models.Category>(updateCategoryCommand);

            await _categoryRepository.Update(category, cancellationToken);

            await _categoryRepository.SaveChanges(cancellationToken);

            return new EmptyResponse();
        }
    }
}
