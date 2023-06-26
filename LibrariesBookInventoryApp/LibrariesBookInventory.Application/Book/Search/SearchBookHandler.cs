using AutoMapper;
using LibrariesBookInventory.Application.Book.Common;
using LibrariesBookInventory.DomainServices.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using LibrariesBookInventory.DomainServices.Models;
using LibrariesBookInventory.Application.Common;

namespace LibrariesBookInventory.Application.Book
{
    public class SearchBooksQuery : IRequest<BaseResponse<IEnumerable<BookDto>>>
    {
        public SearchBooksParameters Args { get; set; }
    }

    internal class SearchBooksHandler : IRequestHandler<SearchBooksQuery, BaseResponse<IEnumerable<BookDto>>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public SearchBooksHandler(
            IBookRepository bookRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<IEnumerable<BookDto>>> Handle(
            SearchBooksQuery query,
            CancellationToken cancellationToken)
        {
            var books = await _bookRepository.Search(query.Args, cancellationToken);

            return new BaseResponse<IEnumerable<BookDto>>() { Data = _mapper.Map<IEnumerable<BookDto>>(books) };
        }
    }
}
