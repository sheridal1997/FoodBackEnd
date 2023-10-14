using Food_Backend.Entity;

namespace Food_Backend.Repository
{
    public class RecipeVideoRepository : EFRepository<RecipeVideo, int>, IRecipeVideoRepository
    {
        public RecipeVideoRepository(DbContexD requestScopecs) 
            : base(requestScopecs)
        {
            
        }
        
        
    }
    public interface IRecipeVideoRepository : IEFRepositorty<RecipeVideo, int>
    {

    }
}
