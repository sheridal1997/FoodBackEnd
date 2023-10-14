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
    public class WeeklyPlanController : ControllerBase
    {
        private readonly IWeeklyPlanService _service;

        public WeeklyPlanController(IWeeklyPlanService service)
        {
            _service = service;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async Task<IEnumerable<WeeklyPlan>> Get()
        {
            return await _service.Get();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<WeeklyPlan> Get(int id)
        {
            return await _service.Get(id);
        }

        // POST api/<UserController>

        [HttpPost]
        public async Task<WeeklyPlan> Post([FromBody] WeeklyPlan model)
        {
            return await _service.Create(model);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public Task<WeeklyPlan> Put(int id, [FromBody] WeeklyPlan model)
        {
            return _service.Update(model);
        }

        [HttpGet("GetByUser")]
        public async Task<List<WeeklyPlan>> GetByUser()
        {
            return await _service.GetByUser();
        }

        [HttpGet("GetByUserAndAdmin")]
        public async Task<List<WeeklyPlan>> GetByUserAndAdmin()
        {
            return await _service.GetAll();
        }
        [HttpGet("GetOtherUsers")]
        public async Task<List<WeeklyPlan>> GetOtherUsers()
        {
            return await _service.GetOtherUsers();
        }
        [HttpGet("GetEndWeeklyPlane")]
        public async Task<List<WeeklyPlan>> GetEndWeeklyPlane()
        {
            return await _service.GetEndWeeklyPlane();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _service.Delete(id))
            {
                return Ok("Entity delete sucessfully");
            }
            return BadRequest("Some thing went wrong");
        }
        
    }
}
