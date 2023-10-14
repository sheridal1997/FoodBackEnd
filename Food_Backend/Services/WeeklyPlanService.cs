using Food_Backend.Commom;
using Food_Backend.Entity;
using Food_Backend.Middleware;
using Food_Backend.Repository;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Food_Backend.Service
{

    public class WeeklyPlanService : IWeeklyPlanService
    {
        private readonly IWeeklyPlaneRepository _repository;
        private readonly IRequestHeader _requestHeader;
        public WeeklyPlanService(IWeeklyPlaneRepository repository, IRequestHeader requestHeader)
        {
            _repository = repository;
            _requestHeader = requestHeader;
        }
        public async Task<List<WeeklyPlan>> Get()
        {
            var query = _repository.FindByCondition(o => o.IsActive == true)
                .Include(o => o.Recipe)
                .ThenInclude(o => o != null ? o.RecipeVideo : null);
           
            
                query = query.Where(o => o.CreatedUserId == _requestHeader.UserId)
                    .Include(o => o.Recipe)
                    .ThenInclude(o => o != null ? o.RecipeVideo : null); ;
            
           
            var result = await query.Select(o => new WeeklyPlan
            {
                Id = o.Id,
                RecipeId = o.RecipeId,
                CreatedUserId = o.CreatedUserId,
                StartDate = o.StartDate,
                CreateTime = o.CreateTime,
                EditTime = o.EditTime,
                EditUserId = o.EditUserId,
                EndDate = o.EndDate,
                IsActive = o.IsActive,
                Name = o.Name,
                Recipe = o.Recipe != null ? new Recipe
                {
                    Id = o.Recipe != null ? o.Recipe.Id : 0,
                    Title = o.Recipe != null ? o.Recipe.Title : null,
                    FileUrl = o.Recipe != null ? o.Recipe.FileUrl : null,
                    RecipeVideoId = o.Recipe != null ? o.Recipe.RecipeVideoId : null,
                    RecipeVideo = o.Recipe != null && o.Recipe.RecipeVideo != null ? new RecipeVideo
                    {
                        Id = o.Recipe != null && o.Recipe.RecipeVideo != null ? o.Recipe.RecipeVideo.Id : 0,
                        Name = o.Recipe != null && o.Recipe.RecipeVideo != null ? o.Recipe.RecipeVideo.Name : "",
                        FileUrl = o.Recipe != null && o.Recipe.RecipeVideo != null ? o.Recipe.RecipeVideo.FileUrl : "",

                    }
                    : null

                } : null
            }).ToListAsync();


            return result;
        }
        public async Task<WeeklyPlan> Get(int id)
        {

            var response = await _repository.FindAsync(id);
            return response;
        }
        public async Task<WeeklyPlan> Create(WeeklyPlan model)
        {

            if (model == null)
            {
                throw new ServiceException("Required Param Missing");
            }
            if (model.StartDate != null)
            {
                model.StartDate = new DateTime(model.StartDate!.Value.Year, model.StartDate!.Value.Month, model.StartDate!.Value.Day);
                model.StartDate = model.StartDate.Value.ToUniversalTime();

                model.EndDate = model.StartDate.Value.AddDays(7);
            }
            

            model.IsActive = true;
            var user = await _repository.CreateAsync(model);
            return user;
        }
        public async Task<WeeklyPlan> Update(WeeklyPlan model)
        {
            if (model == null)
            {
                throw new ServiceException("Required Param Missing");
            }
            var dbEntity = await _repository.FindByCondition(o => o.Id == model.Id).FirstOrDefaultAsync();
            if (dbEntity == null)
            {
                throw new ServiceException("Record not found");
            }
            model.IsActive = dbEntity.IsActive;
            model.CreatedUserId = dbEntity?.CreatedUserId;
            model.CreateTime = dbEntity?.CreateTime;
            await _repository.UpdateAsync(model);

            return model;
        }
     
        public async Task<bool> Delete(int id)
        {
            var user = await _repository.FindAsync(id);
            if (user == null)
            {
                throw new ServiceException("User Not Found");
            }
            _repository.Delete(user);
            return true;
        }

        public async Task<List<WeeklyPlan>> GetByUser()
        {
            var response = await _repository.FindByCondition(o => o.CreatedUserId == _requestHeader.UserId).ToListAsync();
            return response;
        }
        public async Task<List<WeeklyPlan>> GetOtherUsers()
        {
            var response = await _repository.FindByCondition(o => o.CreatedUserId != _requestHeader.UserId).ToListAsync();
            return response;
        }

        public async Task<List<WeeklyPlan>> GetAll()
        {
            
                var response = await _repository.FindAll().ToListAsync();
                return response;
            
        }
        public async Task<List<WeeklyPlan>> GetEndWeeklyPlane()
        {
            var result = await _repository.FindAll().Where(o => DateTime.UtcNow > o.EndDate && o.CreatedUserId == _requestHeader.UserId).ToListAsync();
            return result;
        }

    }
    public interface IWeeklyPlanService
    {
        Task<WeeklyPlan> Create(WeeklyPlan model);
        Task<List<WeeklyPlan>> Get();
        Task<WeeklyPlan> Get(int id);
        Task<WeeklyPlan> Update(WeeklyPlan model);
        Task<bool> Delete(int id);
        Task<List<WeeklyPlan>> GetByUser();
        Task<List<WeeklyPlan>> GetAll();
        Task<List<WeeklyPlan>> GetOtherUsers();
        Task<List<WeeklyPlan>> GetEndWeeklyPlane();

    }

}
