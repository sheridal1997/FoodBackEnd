using Food_Backend.Entity;

namespace Food_Backend.Repository
{
    public class UserRatingRepository : EFRepository<UserRating, int>, IUserRatingRepository
    {
        public UserRatingRepository(DbContexD requestScopecs) 
            : base(requestScopecs)
        {
            
        }
        
    }
    public interface IUserRatingRepository : IEFRepositorty<UserRating, int>
    {

    }
}
