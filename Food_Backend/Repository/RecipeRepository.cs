using Food_Backend.Entity;

namespace Food_Backend.Repository
{
    public class RecipeRepository : EFRepository<Recipe, int>, IRecipeRepository
    {
        public RecipeRepository(DbContexD requestScopecs) 
            : base(requestScopecs)
        {
            
        }
        
        
    }
    public interface IRecipeRepository : IEFRepositorty<Recipe, int>
    {

    }
}
