using Food_Backend.Entity;

namespace Food_Backend.Repository
{
    public class SystemSettingRepository : EFRepository<SystemSetting, int>, ISystemSettingRepository
    {
        public SystemSettingRepository(DbContexD requestScopecs) 
            : base(requestScopecs)
        {
            
        }
        
        
    }
    public interface ISystemSettingRepository : IEFRepositorty<SystemSetting, int>
    {

    }
}
