using AutoMapper;
using LibrariesBookInventory.Application.Book.Common;
using LibrariesBookInventory.Application.Common;
using LibrariesBookInventory.DomainServices.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LibrariesBookInventory.Application.Book.GetAll
{
    public class GetAllBooksQuery : IRequest<BaseResponse<IEnumerable<BookDto>>>
    {
    }
    internal class GetAllBooksHandler : IRequestHandler<GetAllBooksQuery, BaseResponse<IEnumerable<BookDto>>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public GetAllBooksHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<IEnumerable<BookDto>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _bookRepository.GetAll(cancellationToken);

            return new BaseResponse<IEnumerable<BookDto>>() { Data = _mapper.Map<IEnumerable<BookDto>>(books) };
        }
    }
}