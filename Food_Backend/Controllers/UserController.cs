using Food_Backend.Entity;
using Food_Backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Food_Backend.Service.UserService;

namespace Food_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService= userService;
        }
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _userService.Get();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<User> Get(int id)
        {
            return await _userService.Get(id);
        }

        // POST api/<UserController>
        [AllowAnonymous]
        [HttpPost]
        public async Task<User> Post([FromBody] User model)
        {
            return await _userService.Create(model);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public Task<User> Put(int id, [FromBody] User model)
        {
            return _userService.Update(model);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<string> Delete(int id)
        {
          if( await _userService.Delete(id))
            {
                return "Entity delete sucessfully";
            }
            return "Some thing went wrong";
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<LoginInfo> Login([FromBody] Login login)
        {
            var result = await _userService.LogIn(login);
            return result;
        }
    }
}
