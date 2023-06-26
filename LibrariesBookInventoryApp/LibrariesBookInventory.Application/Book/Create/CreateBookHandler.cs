using AutoMapper;
using LibrariesBookInventory.Application.Book.Common;
using LibrariesBookInventory.Application.Common;
using LibrariesBookInventory.DomainServices.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LibrariesBookInventory.Application.Book.Create
{
    public class CreateBookCommand : BookCommand, IRequest<BaseResponse<long>>
    {
    }

    internal class CreateBookHandler : IRequestHandler<CreateBookCommand, BaseResponse<long>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public CreateBookHandler(
            IBookRepository bookRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<long>> Handle(
            CreateBookCommand createBookCommand,
            CancellationToken cancellationToken)
        {
            var book = _mapper.Map<Domain.Models.Book>(createBookCommand);

            _bookRepository.Create(book);

            await _bookRepository.SaveChanges(cancellationToken);

            return new BaseResponse<long>() { Data = book.Id };
        }
    }
}