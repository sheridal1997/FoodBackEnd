using Food_Backend.Entity;
using Food_Backend.Service;
using Food_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Food_Backend.Service.UserService;

namespace Food_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _service;
        private readonly IDashboardService _dashboardService;

        public RecipeController(IRecipeService service, IDashboardService dashboardService)
        {
            _service = service;
            _dashboardService = dashboardService;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async Task<IEnumerable<Recipe>> Get()
        {
            return await _service.Get();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<Recipe> Get(int id)
        {
            return await _service.Get(id);
        }

        // POST api/<UserController>
        
        [HttpPost]
        public async Task<Recipe> Post([FromForm] Recipe model)
        {
            return await _service.Create(model);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public Task<Recipe> Put(int id, [FromForm] Recipe model)
        {
            return _service.Update(model);
        }

        [HttpGet("GetByUser")]
        public async Task<List<Recipe>> GetByUser()
        {
            return await _service.GetByUser();
        }

        [HttpGet("GetByUserAndAdmin")]
        public async Task<List<Recipe>> GetByUserAndAdmin()
        {
            return await _service.GetAll();
        }
        [HttpGet("GetOtherUsers")]
        public async Task<List<Recipe>> GetOtherUsers()
        {
            return await _service.GetOtherUsers();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<string> Delete(int id)
        {
          if( await _service.Delete(id))
            {
                return "Entity delete sucessfully";
            }
            return "Some thing went wrong";
        }
        [HttpGet("Search")]
        public async Task<List<Recipe>> Search(string filter)
        {
            return await _service.Search(filter);
        }
        [HttpGet("Shuffle")]
        public async Task<List<Recipe>?> Shufflte()
        {
            return await _service.Shufflte();
        }
        [HttpGet("DashboardCount")]
        public async Task<List<Dashboard>> Dashboard()
        {
            return await _dashboardService.AllCount();
        }
        [HttpPost("UserFavorite")]
        public async Task<bool> UserFavorite(UserFavorite userFavorite)
        {
            var res = await _service.UserFavorite(userFavorite);
            return res;
            
        }
        [HttpPost("UserRating")]
        public async Task<double> UserRating(UserRating rating)
        {
            var res = await _service.UserRating(rating);
            return res;
        }
        [HttpGet("GetFavorite")]
        public async Task<List<UserFavorite>> GetFavorite()
        {
            var res = await _service.GetFavorite();
            return res;
        }
        [AllowAnonymous]
        [HttpGet("CurrentDateTime")]
        public DateTime ServerDate()
        {
            var date = DateTime.UtcNow.AddHours(4);
            return date;
        }
    }
}
