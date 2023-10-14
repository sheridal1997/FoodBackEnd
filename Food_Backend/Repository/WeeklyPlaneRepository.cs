using Food_Backend.Entity;

namespace Food_Backend.Repository
{
    public class WeeklyPlaneRepository : EFRepository<WeeklyPlan, int>, IWeeklyPlaneRepository
    {
        public WeeklyPlaneRepository(DbContexD requestScopecs) 
            : base(requestScopecs)
        {
            
        }
        
        
    }
    public interface IWeeklyPlaneRepository : IEFRepositorty<WeeklyPlan, int>
    {

    }
}
