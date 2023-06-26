using LibrariesBookInventory.Application.Common;
using LibrariesBookInventory.DomainServices.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LibrariesBookInventory.Application.Book.Delete
{
    public class DeleteBookCommand : IRequest<EmptyResponse>
    {
        public long Id { get; set; }
    }

    internal class DeleteBookHandler : IRequestHandler<DeleteBookCommand, EmptyResponse>
    {
        private readonly IBookRepository _bookRepository;

        public DeleteBookHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<EmptyResponse> Handle(
            DeleteBookCommand request,
            CancellationToken cancellationToken)
        {
            await _bookRepository.Delete(request.Id, cancellationToken);
            await _bookRepository.SaveChanges(cancellationToken);

            return new EmptyResponse();
        }
    }
}