using Food_Backend.Commom;
using Food_Backend.Entity;
using Food_Backend.Middleware;
using Food_Backend.Repository;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Text;

namespace Food_Backend.Service
{

    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _repository;
        private readonly IRequestHeader _requestHeader;
        private readonly ISystemSettingRepository _systemSettingRepository;
        private readonly ILogger<RecipeService> _logger;
        private readonly IConfiguration _configuration;
        private readonly StorageClient _storageClient;
        private readonly IUserFavoriteRepository _userFavoriteRepository;
        private readonly IUserRatingRepository _userRatingRepository;

        public RecipeService(IRecipeRepository repository,
            IRequestHeader requestHeader,
            ISystemSettingRepository systemSettingRepository,
            ILogger<RecipeService> logger,
            IConfiguration configuration,
            IUserFavoriteRepository userFavoriteRepository,
            IUserRatingRepository userRatingRepository
            )
        {
            var privateKeyPath = configuration["Firebase:PrivateKeyFilePath"];

            var credentials = GoogleCredential.FromJson(privateKeyPath);
            _storageClient = StorageClient.Create(credentials);
            _repository = repository;
            _requestHeader = requestHeader;
            _systemSettingRepository = systemSettingRepository;
            _logger = logger;
            _configuration = configuration;
            _userFavoriteRepository = userFavoriteRepository;
            _userRatingRepository = userRatingRepository;
        }
        public async Task<List<Recipe>> Get()
        {
            var response = (_repository.FindAll()).Include(o => o.RecipeVideo).ToList();
            var userFav = await _userFavoriteRepository.FindByCondition(o => o.CreatedUserId == _requestHeader.UserId).ToListAsync();
            var rating = await _userRatingRepository.FindByCondition(o => o.CreatedUserId == _requestHeader.UserId).ToListAsync();

            response = response.Select(o => new Recipe
            {
                Id = o.Id,
                CreatedUserId = o.CreatedUserId,
                Title = o.Title,
                Date = o.Date,
                CreateTime = o.CreateTime,
                Discription = o.Discription,
                RecipeVideoId = o.RecipeVideoId,
                EditTime = o.EditTime,
                EditUserId = o.EditUserId,
                Time = o.CreateTime.Value.TimeOfDay,
                FileUrl = o.FileUrl,
                RecipeVideo = o.RecipeVideo != null ? new RecipeVideo
                {
                    Id = o.RecipeVideoId != null ? o.RecipeVideoId.Value : 0,
                    FileUrl = o.RecipeVideo != null ? o.RecipeVideo.FileUrl : "",
                    Name = o.RecipeVideo != null ? o.RecipeVideo.Name : "",


                } : null,
                FavoriteUser = userFav == null ? null : userFav?.Where(x => x.RecipeId == o.Id).FirstOrDefault(),
                RatingUser = rating == null ? null : rating?.Where(x => x.RecipeId == o.Id).FirstOrDefault(),

            }).ToList();

            return response;
        }
        public async Task<Recipe> Get(int id)
        {
            var response = await _repository.FindAsync(id);
            return response;
        }
        public async Task<Recipe> Create(Recipe model)
        {
            if (model == null)
            {
                throw new ServiceException("Required Param Missing");
            }
            var bucketName = _configuration["Firebase:StorageBucket"];

            // Specify the content type of the uploaded file


            // Upload the file to Firebase Storage
            if (model.File != null)
            {
                var contentType = model.File.ContentType;
                var fileUrl = await this.UploadFileAsync(bucketName, model.File.OpenReadStream(), contentType, model);
                if (fileUrl.Url != null && fileUrl.objectName != null)
                {
                    model.FileUrl = fileUrl.Url;
                    model.ObjectName = fileUrl.objectName;

                }
            }
            var user = await _repository.CreateAsync(model);
            return user;
        }
        public async Task<Recipe> Update(Recipe model)
        {
            var bucketName = _configuration["Firebase:StorageBucket"];
            if (model == null)
            {
                throw new ServiceException("Required Param Missing");
            }
            var dbEntity = await _repository.FindByCondition(o => o.Id == model.Id).FirstOrDefaultAsync();
            if (dbEntity == null)
            {
                throw new ServiceException("Record not found");
            }
            model.CreatedUserId = dbEntity?.CreatedUserId;
            model.CreateTime = dbEntity?.CreateTime;
            model.IsShuffle = dbEntity.IsShuffle;
            if (model.File != null && !string.IsNullOrEmpty(model.File.Name))
            {
                if (dbEntity != null)
                {
                    if (!string.IsNullOrEmpty(model.ObjectName))
                    {
                        var existingVideo = await _storageClient.GetObjectAsync(bucketName, dbEntity.ObjectName);
                        await _storageClient.DeleteObjectAsync(existingVideo);
                    }
                }
                var contentType = model.File.ContentType;
                var fileUrl = await this.UploadFileAsync(bucketName, model.File.OpenReadStream(), contentType, model);

                if (fileUrl.Url != null && fileUrl.objectName != null)
                {
                    model.FileUrl = fileUrl.Url;
                    model.ObjectName = fileUrl.objectName;
                }
            }
            else
            {
                model.FileUrl = dbEntity?.FileUrl;
                model.ObjectName = dbEntity?.ObjectName;

            }
            await _repository.UpdateAsync(model);
            model.File = null;

            return model;
        }
        public async Task<bool> Delete(int id)
        {
            var bucketName = _configuration["Firebase:StorageBucket"];
            var dbEntity = await _repository.FindByCondition(o => o.Id == id).FirstOrDefaultAsync();
            if (dbEntity == null)
            {
                throw new ServiceException("entity Not Found");
            }
            if (dbEntity != null)
            {
                if (!string.IsNullOrEmpty(dbEntity.ObjectName))
                {
                    var existingVideo = await _storageClient.GetObjectAsync(bucketName, dbEntity.ObjectName);
                    await _storageClient.DeleteObjectAsync(existingVideo);
                }
            }
            _repository.Delete(dbEntity);
            return true;
        }

        public async Task<List<Recipe>> GetByUser()
        {
            var query = _repository.FindByCondition(o => o.CreatedUserId == _requestHeader.UserId);
            var response = await query.Select(o => new Recipe
            {
                Id = o.Id,
                CreatedUserId = o.CreatedUserId,
                Title = o.Title,
                Date = o.Date,
                CreateTime = o.CreateTime,
                Discription = o.Discription,
                RecipeVideoId = o.RecipeVideoId,
                EditTime = o.EditTime,
                EditUserId = o.EditUserId,
                Time = o.CreateTime.Value.TimeOfDay,
                FileUrl = o.FileUrl,
                RecipeVideo = o.RecipeVideo != null ? new RecipeVideo
                {
                    Id = o.RecipeVideoId != null ? o.RecipeVideoId.Value : 0,
                    FileUrl = o.RecipeVideo != null ? o.RecipeVideo.FileUrl : "",
                    Name = o.RecipeVideo != null ? o.RecipeVideo.Name : "",


                } : null
            }).ToListAsync();
            return response;
        }
        public async Task<List<Recipe>> GetOtherUsers()
        {
            var query = _repository.FindAll();
            var response = await query.Select(o => new Recipe
            {
                Id = o.Id,
                CreatedUserId = o.CreatedUserId,
                Title = o.Title,
                Date = o.Date,
                CreateTime = o.CreateTime,
                Discription = o.Discription,
                RecipeVideoId = o.RecipeVideoId,
                EditTime = o.EditTime,
                EditUserId = o.EditUserId,
                Time = o.CreateTime.Value.TimeOfDay,
                FileUrl = o.FileUrl,
                RecipeVideo = o.RecipeVideo != null ? new RecipeVideo
                {
                    Id = o.RecipeVideoId != null ? o.RecipeVideoId.Value : 0,
                    FileUrl = o.RecipeVideo != null ? o.RecipeVideo.FileUrl : "",
                    Name = o.RecipeVideo != null ? o.RecipeVideo.Name : "",


                } : null
            }).ToListAsync();
            return response;
        }

        public async Task<List<Recipe>> GetAll()
        {
            
            
            
                var response = await _repository.FindByCondition(o => o.CreatedUserId == _requestHeader.UserId).ToListAsync();
                return response;
            
            
        }

        public async Task<List<Recipe>> Search(string filter)
        {

            var query = _repository.FindAll();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(o => o.Title.ToLower().Contains(filter.ToLower()));
            }

            var response = await query.Select(o => new Recipe
            {
                Id = o.Id,
                CreatedUserId = o.CreatedUserId,
                Title = o.Title,
                Date = o.Date,
                CreateTime = o.CreateTime,
                Discription = o.Discription,
                RecipeVideoId = o.RecipeVideoId,
                EditTime = o.EditTime,
                EditUserId = o.EditUserId,
                Time = o.CreateTime.Value.TimeOfDay,
                RecipeVideo = o.RecipeVideo != null ? new RecipeVideo
                {
                    Id = o.RecipeVideoId != null ? o.RecipeVideoId.Value : 0,
                    FileUrl = o.RecipeVideo != null ? o.RecipeVideo.FileUrl : "",
                    Name = o.RecipeVideo != null ? o.RecipeVideo.Name : "",


                } : null
            }).ToListAsync();

            return response;

        }

        public async Task<List<Recipe>?> Shufflte()
        {
            var query = _repository.FindByCondition(o => o.IsShuffle == false).Include(o => o.RecipeVideo).ToList();

            var getSetting = await _systemSettingRepository.FindByCondition(o => o.Key == SystemSettingEnums.Shuffle.ToString()).FirstOrDefaultAsync();
            if (getSetting == null)
            {
                return null;
            }
            var nextShuffle = getSetting.ShuffleDate;
            var date = DateTime.UtcNow.AddHours(4);
            Recipe? recipe = null;
            List<Recipe> lstRecipe = new List<Recipe>();
            if (nextShuffle <= date)
            {

                recipe = GetRandom<Recipe>(query);
                recipe.IsShuffle = true;
                _repository.Update(recipe);
                var oldRecipe = await _repository.FindByCondition(o => o.IsShuffle && o.Id != recipe.Id).FirstOrDefaultAsync();
                if (oldRecipe != null)
                {
                    oldRecipe.IsShuffle = false;
                    _repository.Update(oldRecipe);
                }
                var now = DateTime.UtcNow.AddHours(4).AddDays(1);
                var Nextdate = new DateTime(now.Year, now.Month, now.Day);
                getSetting.ShuffleDate = Nextdate;
                await _systemSettingRepository.UpdateAsync(getSetting);
            }
            else
            {
                recipe = await _repository.FindByCondition(o => o.IsShuffle).Include(o => o.RecipeVideo).FirstOrDefaultAsync();
            }
            if (recipe != null)
            {
                recipe.RecipeVideo = recipe.RecipeVideo != null ? new RecipeVideo
                {
                    Id = recipe.RecipeVideoId != null ? recipe.RecipeVideoId.Value : 0,
                    FileUrl = recipe.RecipeVideo != null ? recipe.RecipeVideo.FileUrl : "",
                    Name = recipe.RecipeVideo != null ? recipe.RecipeVideo.Name : "",
                    ObjectName = recipe.RecipeVideo != null ? recipe.RecipeVideo.ObjectName : "",
                    Time = recipe.RecipeVideo != null && recipe.RecipeVideo.CreateTime != null ? recipe.RecipeVideo.CreateTime.Value.TimeOfDay : null,


                } : null;
                recipe.Time = recipe.CreateTime != null ? recipe.CreateTime.Value.TimeOfDay : null;
            }
            if (recipe != null)
                lstRecipe.Add(recipe);
            return lstRecipe;
        }
        public T GetRandom<T>(IEnumerable<T> list)
        {
            return list.ElementAt(new Random(DateTime.Now.Millisecond).Next(list.Count()));
        }
        private async Task<(string Url, string objectName)> UploadFileAsync(string bucketName, Stream stream, string contentType, Recipe model)
        {

            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                var fileContent = Encoding.UTF8.GetString(memoryStream.ToArray());

                // You can now use the file content or perform any necessary operations

                // Upload the file to Firebase Storage
                var storageObject = await _storageClient.UploadObjectAsync(
                    bucket: bucketName,
                    objectName: model?.File?.FileName,
                    contentType: contentType,
                    source: memoryStream

                );
                return (storageObject.MediaLink, storageObject.Name);
            }
        }

        public async Task<bool> UserFavorite(UserFavorite userFavorite)
        {
            try
            {
                var userFav = await _userFavoriteRepository
                .FindByCondition(o => o.CreatedUserId == _requestHeader.UserId && o.RecipeId == userFavorite.RecipeId)
                .FirstOrDefaultAsync();

                if (userFav == null)
                {
                    userFav = await _userFavoriteRepository.CreateAsync(userFavorite);
                }
                else
                {
                    userFav.IsFavorite = userFavorite.IsFavorite;
                    await _userFavoriteRepository.UpdateAsync(userFav);
                }

                return userFavorite.IsFavorite;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public async Task<double> UserRating(UserRating rating)
        {
            try
            {
                var userRating = await _userRatingRepository
                .FindByCondition(o => o.CreatedUserId == _requestHeader.UserId && o.RecipeId == rating.RecipeId)
                .FirstOrDefaultAsync();

                if (userRating == null)
                {
                    userRating = await _userRatingRepository.CreateAsync(rating);
                }
                else
                {
                    userRating.RecipeRating = rating.RecipeRating;
                    await _userRatingRepository.UpdateAsync(userRating);
                }

                return rating.RecipeRating;
            }
            catch (Exception)
            {

                throw;
            }

        }


        public async Task<List<UserFavorite>> GetFavorite()
        {
          
            var query = _userFavoriteRepository.FindByCondition(o => o.IsFavorite == true)
                .Include(o => o.Recipe)
                .ThenInclude(o => o != null ? o.RecipeVideo : null); ;
          
            var respone = await query.Select(o => new UserFavorite
            {
                Id = o.Id,
                IsFavorite = o.IsFavorite,
                CreateTime = o.CreateTime,
                RecipeId = o.RecipeId,
                CreatedUserId = o.CreatedUserId,
                Recipe = o.Recipe != null ? new Recipe
                {
                    Id = o.Recipe != null ? o.Recipe.Id : 0,
                    Title = o.Recipe != null ? o.Recipe.Title : "",
                    FileUrl = o.Recipe != null ? o.Recipe.FileUrl : "",
                    Discription = o.Recipe != null ? o.Recipe.Discription : "",
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

            return respone;
        }
    }
    public interface IRecipeService
    {
        Task<Recipe> Create(Recipe model);
        Task<List<Recipe>> Get();
        Task<Recipe> Get(int id);
        Task<Recipe> Update(Recipe model);
        Task<bool> Delete(int id);
        Task<List<Recipe>> GetByUser();
        Task<List<Recipe>> GetAll();
        Task<List<Recipe>> GetOtherUsers();
        Task<List<Recipe>> Search(string filter);
        Task<List<Recipe>?> Shufflte();
        Task<bool> UserFavorite(UserFavorite userFavorite);
        Task<double> UserRating(UserRating userFavorite);
        Task<List<UserFavorite>> GetFavorite();

    }

}
