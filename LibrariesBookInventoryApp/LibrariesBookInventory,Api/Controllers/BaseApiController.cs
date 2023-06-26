using LibrariesBookInventory.Application;
using LibrariesBookInventory.Application.Common;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace LibrariesBookInventoryApi.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected IHttpActionResult GetResponse<T>(
            BaseResponse<T> response,
            HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            if (response.IsOk)
                return Content(httpStatusCode, response.Data);

            var notFoundError = response.Errors?.FirstOrDefault(x => x.ErrorCode == ErrorsCodes.ItemNotFound);

            if (notFoundError != null)
                return Content(HttpStatusCode.NotFound, notFoundError.ErrorMessage);

            return Content(HttpStatusCode.BadRequest, response.Errors?.Select(x => x.ErrorMessage).ToList());
        }

        protected IHttpActionResult GetResponseCreated<T>(BaseResponse<T> response)
        {
            return GetResponse(response, HttpStatusCode.Created);
        }

        protected IHttpActionResult GetResponseNoContent<T>(BaseResponse<T> response)
        {
            return GetResponse(response, HttpStatusCode.NoContent);
        }
    }
}