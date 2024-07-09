using API.Data;
using API.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //[Authorize]
    public class UsersController : BaseApiController
    {
        private readonly DataContext _dataContext;
        public UsersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _dataContext.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUsers(int id)
        {
            var user = await _dataContext.Users.FindAsync(id);
            if (user == null) { return NotFound(); }
            return Ok(user);
        }
    }
}
