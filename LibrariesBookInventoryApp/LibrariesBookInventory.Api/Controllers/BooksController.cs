using AutoMapper;
using LibrariesBookInventory.Application.Book.Common;
using LibrariesBookInventory.Application.Book.Delete;
using LibrariesBookInventory.Application.Book;
using LibrariesBookInventory.Application.Book.Get;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using LibrariesBookInventory.Application.Book.Create;
using LibrariesBookInventory.Application.Book.Update;
using LibrariesBookInventory.DomainServices.Models;

namespace LibrariesBookInventoryApi.Controllers
{
    [RoutePrefix("api/Books")]
    public class BooksController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public BooksController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<IHttpActionResult> Get(
            [FromUri] long id,
            CancellationToken cancellationToken)
        {
            return GetResponse(await _mediator.Send(new GetBookQuery() { Id = id }, cancellationToken));
        }

        [HttpGet]
        public async Task<IHttpActionResult> Search(
            [FromUri] SearchBooksParameters args,
            CancellationToken cancellationToken)
        {
            return GetResponse(await _mediator.Send(new SearchBooksQuery() { Args = args }, cancellationToken));
        }

        [HttpPost]
        public async Task<IHttpActionResult> Create(
            [FromBody] CreateBookCommand createBookDto,
            CancellationToken cancellationToken)
        {
            return GetResponseCreated(await _mediator.Send(createBookDto, cancellationToken));
        }

        [HttpPut]
        [Route("{Id}")]
        public async Task<IHttpActionResult> Put(
            [FromUri] long id,
            [FromBody] BookCommand bookCommand,
            CancellationToken cancellationToken)
        {
            UpdateBookCommand updateBookCommand = new UpdateBookCommand
            {
                Id = id,
            };

            if (bookCommand != null)
                _mapper.Map(bookCommand, updateBookCommand);

            return GetResponseNoContent(await _mediator.Send(updateBookCommand, cancellationToken));
        }

        [HttpDelete]
        [Route("{Id}")]
        public async Task<IHttpActionResult> Delete(
            long id,
            CancellationToken cancellationToken)
        {
            return GetResponse(await _mediator.Send(new DeleteBookCommand() { Id = id }, cancellationToken));
        }
    }
}