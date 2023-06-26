using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using LibrariesBookInventory.Application.Category.Common;
using LibrariesBookInventory.Application.Category.Create;
using LibrariesBookInventory.Application.Category.Delete;
using LibrariesBookInventory.Application.Category.Get;
using LibrariesBookInventory.Application.Category.GetAll;
using LibrariesBookInventory.Application.Category.Update;
using MediatR;

namespace LibrariesBookInventoryApi.Controllers
{
    [RoutePrefix("api/Categories")]
    public class CategoriesController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CategoriesController(
            IMediator mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<IHttpActionResult> Get(
            [FromUri] int id,
            CancellationToken cancellationToken)
        {
            return GetResponse(await _mediator.Send(new GetCategoryQuery() { Id = id }, cancellationToken));
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll(CancellationToken cancellationToken)
        {
            return GetResponse(await _mediator.Send(new GetAllCategoriesQuery(), cancellationToken));
        }

        [HttpPost]
        public async Task<IHttpActionResult> Create(
            [FromBody] CreateCategoryCommand createCategoryDto,
            CancellationToken cancellationToken)
        {
            return GetResponseCreated(await _mediator.Send(createCategoryDto, cancellationToken));
        }

        [HttpPut]
        [Route("{Id}")]
        public async Task<IHttpActionResult> Put([FromUri] int id,
            [FromBody] BaseCategoryCommand baseCategoryCommand,
            CancellationToken cancellationToken)
        {
            UpdateCategoryCommand updateCategoryCommand = new UpdateCategoryCommand
            {
                Id = id,
            };

            if (baseCategoryCommand != null)
                _mapper.Map(baseCategoryCommand, updateCategoryCommand);

            return GetResponseNoContent(await _mediator.Send(updateCategoryCommand, cancellationToken));
        }

        [HttpDelete]
        [Route("{Id}")]
        public async Task<IHttpActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            return GetResponse(await _mediator.Send(new DeleteCategoryCommand() { Id = id }, cancellationToken));
        }
    }
}