using Food_Backend.Entity;
using Food_Backend.Middleware;
using Food_Backend.Repository;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Food_Backend.Service
{

    public class RecipeVideoService : IRecipeVideoService
    {
        private readonly IRecipeVideoRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly StorageClient _storageClient;
        public RecipeVideoService(IRecipeVideoRepository repository, IConfiguration configuration)
        {
            var privateKeyPath = configuration["Firebase:PrivateKeyFilePath"];

            var credentials = GoogleCredential.FromJson(privateKeyPath);
            _storageClient = StorageClient.Create(credentials);
            _repository = repository;
            _configuration = configuration;
        }
        public List<RecipeVideo> Get()
        {
            var query = _repository.FindAll();

            var result = query.Select(o => new RecipeVideo 
            {
                Id= o.Id,
                Name= o.Name,
                CreatedUserId= o.CreatedUserId,
                CreateTime= o.CreateTime,
                Date = o.Date,
                Description= o.Description,
                EditTime= o.EditTime,
                EditUserId= o.EditUserId,
                File = o.File,
                FileUrl= o.FileUrl,
                IsDelete= o.IsDelete,
                ObjectName= o.ObjectName,
                Time= o.CreateTime!.Value.TimeOfDay,
            }).ToList();

            return result;
        }
        public async Task<RecipeVideo> Get(int id)
        {
            var response = await _repository.FindAsync(id);
            return response;
        }
        public async Task<RecipeVideo> Create(RecipeVideo model)
        {
            if (model == null)
            {
                throw new ServiceException("Required Param Missing");
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ServiceException("Name is required.");
            }
            if (model.File == null || model.File.Length == 0)
                throw new ServiceException("No file provided.");

            // Specify the Firebase storage bucket name
            var bucketName = _configuration["Firebase:StorageBucket"];

            // Specify the content type of the uploaded file
            var contentType = model.File.ContentType;

            // Upload the file to Firebase Storage
            var fileUrl = await this.UploadFileAsync(bucketName, model.File.OpenReadStream(), contentType, model);

            if(fileUrl.Url != null && fileUrl.objectName != null)
            {
                model.FileUrl= fileUrl.Url;
                model.ObjectName = fileUrl.objectName;

            }

            var user = await _repository.CreateAsync(model);

            user.File = null;

            return user;
        }
        public async Task<RecipeVideo> Update(RecipeVideo model)
        {
             var bucketName = _configuration["Firebase:StorageBucket"];

            if (model == null)
            {
                throw new ServiceException("Required Param Missing");
            }
            var dbEntity = await _repository.FindByCondition(o => o.Id == model.Id).FirstOrDefaultAsync();
            if(dbEntity == null)
            {
                throw new ServiceException("Record not found");
            }
            model.CreatedUserId = dbEntity?.CreatedUserId;
            model.CreateTime = dbEntity?.CreateTime;
            if (model.File != null && !string.IsNullOrEmpty(model.File.Name))
            {
                if(dbEntity != null)
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


        private async Task<(string Url, string objectName)> UploadFileAsync(string bucketName, Stream stream, string contentType, RecipeVideo model)
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
        public async Task<List<RecipeVideo>> Search(string filter)
        {
            var query = _repository.FindAll();

            query = query.Where(o =>  o.Name!.StartsWith(filter));

           var lst = await query.ToListAsync();

            return lst;


        }
    

    }
    public interface IRecipeVideoService
    {
        Task<RecipeVideo> Create(RecipeVideo model);
        List<RecipeVideo> Get();
        Task<RecipeVideo> Get(int id);
        Task<RecipeVideo> Update(RecipeVideo model);
        Task<bool> Delete(int id);
        Task<List<RecipeVideo>> Search(string filter);

    }

}
