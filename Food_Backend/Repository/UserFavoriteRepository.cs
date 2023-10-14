using Food_Backend.Entity;

namespace Food_Backend.Repository
{
    public class UserFavoriteRepository : EFRepository<UserFavorite, int>, IUserFavoriteRepository
    {
        public UserFavoriteRepository(DbContexD requestScopecs) 
            : base(requestScopecs)
        {
            
        }
        
    }
    public interface IUserFavoriteRepository : IEFRepositorty<UserFavorite, int>
    {

    }
}
