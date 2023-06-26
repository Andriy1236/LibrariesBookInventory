using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LibrariesBookInventory.Application.Book.Common;
using LibrariesBookInventory.Application.Common;
using LibrariesBookInventory.DomainServices.Interfaces;
using MediatR;

namespace LibrariesBookInventory.Application.Book.Update
{
    public class UpdateBookCommand : BookCommand, IRequest<EmptyResponse>
    {
        public long Id { get; set; }
    }

    internal class UpdateBookHandler : IRequestHandler<UpdateBookCommand, EmptyResponse>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public UpdateBookHandler(
            IBookRepository bookRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<EmptyResponse> Handle(UpdateBookCommand updateBookCommand, CancellationToken cancellationToken)
        {
            var book = _mapper.Map<Domain.Models.Book>(updateBookCommand);

            await _bookRepository.Update(book, cancellationToken);
            await _bookRepository.SaveChanges(cancellationToken);

            return new EmptyResponse();
        }
    }
}