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
    public class RecipeVideoController : ControllerBase
    {
        private readonly IRecipeVideoService _service;

        public RecipeVideoController(IRecipeVideoService service)
        {
            _service = service;
        }
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<RecipeVideo> Get()
        {
            return _service.Get();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<RecipeVideo> Get(int id)
        {
            return await _service.Get(id);
        }

        // POST api/<UserController>
        
        [HttpPost]
        public async Task<RecipeVideo> Post([FromForm] RecipeVideo model)
        {
            return await _service.Create(model);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public Task<RecipeVideo> Put(int id, [FromForm] RecipeVideo model)
        {
            return _service.Update(model);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
          if( await _service.Delete(id))
            {
                return Ok( "Entity delete sucessfully");
            }
            return BadRequest();
        }
        [HttpGet("Search")]
        public async Task<List<RecipeVideo>> Search(string filter)
        {
            return await _service.Search(filter);
        }
    }
}
