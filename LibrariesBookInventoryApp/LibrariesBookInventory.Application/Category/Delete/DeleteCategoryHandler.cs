using LibrariesBookInventory.Application.Common;
using LibrariesBookInventory.DomainServices.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LibrariesBookInventory.Application.Category.Delete
{
    public class DeleteCategoryCommand : IRequest<EmptyResponse>
    {
        public int Id { get; set; }
    }
    internal class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, EmptyResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        public DeleteCategoryHandler(ICategoryRepository categoryRepository) 
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<EmptyResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            await _categoryRepository.Delete(request.Id, cancellationToken);

            await _categoryRepository.SaveChanges(cancellationToken);

            return new EmptyResponse();
        }
    }
}