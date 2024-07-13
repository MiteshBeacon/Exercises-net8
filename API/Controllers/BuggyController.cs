using API.Data;
using API.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController(DataContext dataContext) : BaseApiController
    {
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetAuth() {

            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var data = dataContext.Users.Find(-1);

            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return data;
            }
        }
        [HttpGet("server-error")]
        public ActionResult<AppUser> GetGetServerError()
        {
                var data = dataContext.Users.Find(-1) ?? throw new Exception("Bad thing happened!");
                return data;            
        }
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {

            return BadRequest("this is bad request");
        }

    }
}
