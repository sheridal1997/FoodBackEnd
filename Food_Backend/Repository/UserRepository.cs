using Food_Backend.Entity;

namespace Food_Backend.Repository
{
    public class UserRepository : EFRepository<User, int>, IUserRepository
    {
        public UserRepository(DbContexD requestScopecs) 
            : base(requestScopecs)
        {
            
        }
        
        
    }
    public interface IUserRepository : IEFRepositorty<User, int>
    {

    }
}
