using Food_Backend.Entity;
using Food_Backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace Food_Backend.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IWeeklyPlaneRepository _weeklyPlaneRepository;
        private readonly IRecipeVideoRepository _recipeVideoRepository;

        public DashboardService(IRecipeRepository recipeRepository, IWeeklyPlaneRepository weeklyPlaneRepository, IRecipeVideoRepository recipeVideoRepository)
        {
            _recipeRepository = recipeRepository;
            _recipeVideoRepository = recipeVideoRepository;
            _weeklyPlaneRepository = weeklyPlaneRepository;
        }

        public async Task<List<Dashboard>> AllCount()
        {
            List<Dashboard> resposne = new List<Dashboard>();
            Dashboard dashboard = new Dashboard();
            var query =  _recipeRepository.FindAll();
            var recipe = await query.CountAsync();
            var recommend = await query.Where(o => o.IsShuffle).CountAsync();
            var video = await _recipeVideoRepository.FindAll().CountAsync();
            var weekly = await _weeklyPlaneRepository.FindByCondition(o => o.IsActive == true).CountAsync();
            dashboard.Name = "Videos";
            dashboard.Count = video;
            resposne.Add(dashboard);
            dashboard = new Dashboard();
            dashboard.Name = "Recipe";
            dashboard.Count = recipe;
            resposne.Add(dashboard);
            dashboard = new Dashboard();
            dashboard.Name = "Recommend";
            dashboard.Count = recommend;
            resposne.Add(dashboard);
            dashboard = new Dashboard();
            dashboard.Name = "Weekly";
            dashboard.Count = weekly;
            resposne.Add(dashboard);
            return resposne;
        }
    }
    public interface IDashboardService
    {
        Task<List<Dashboard>> AllCount();
    }
    public class Dashboard
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
