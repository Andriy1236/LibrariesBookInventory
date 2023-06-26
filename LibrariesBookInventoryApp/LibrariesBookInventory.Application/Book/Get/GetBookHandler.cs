using AutoMapper;
using LibrariesBookInventory.Application.Book.Common;
using LibrariesBookInventory.Application.Common;
using LibrariesBookInventory.DomainServices.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LibrariesBookInventory.Application.Book.Get
{
    public class GetBookQuery : IRequest<BaseResponse<BookDto>>
    {
        public long Id { get; set; }
    }

    internal class GetBookHandler : IRequestHandler<GetBookQuery, BaseResponse<BookDto>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public GetBookHandler(
            IBookRepository bookRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<BookDto>> Handle(GetBookQuery query, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.Get(query.Id, cancellationToken);

            return new BaseResponse<BookDto>() { Data = _mapper.Map<BookDto>(book)};
        }
    }
}
