using Food_Backend.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Food_Backend.Repository
{
    public class EFRepository<TEntity, Tkey> : EFRepository<DbContexD, TEntity, Tkey>
       where TEntity : class, IBaseEntity<Tkey>, new()
    {
        public EFRepository(DbContexD requestScopecs)
          : base(requestScopecs)
        {
        }
    }

    public class EFRepository<Context, TEntity, Tkey> : IEFRepositorty<TEntity, Tkey>
         where Context : DbContexD
        where TEntity : class, IBaseEntity<Tkey>, new()
    {
        public readonly DbContexD RepositoryContext;
        public EFRepository(DbContexD contexts)
        {
            RepositoryContext = contexts;
        }
        public IQueryable<TEntity> FindAll()
        {
            return this.RepositoryContext.Set<TEntity>().AsNoTracking();
        }
        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool suppressEntityTracking = true)
        {
            return suppressEntityTracking
                ? this.RepositoryContext.Set<TEntity>().Where(expression).AsNoTracking()
                : this.RepositoryContext.Set<TEntity>().Where(expression);
        }
        public void Update(TEntity entity)
        {
            entity.EditTime = DateTime.UtcNow;
            entity.EditUserId = RepositoryContext._requestInfo.UserId;
            this.RepositoryContext.Set<TEntity>().Update(entity);
            this.RepositoryContext.SaveChanges();
        }
        public void Delete(TEntity entity)
        {
            this.RepositoryContext.Set<TEntity>().Remove(entity);
            this.RepositoryContext.SaveChanges();
        }

        public async Task<bool> Exist(Expression<Func<TEntity, bool>> expression)
        {
            return await this.RepositoryContext.Set<TEntity>().AnyAsync(expression);
        }
        public async Task<TEntity> FindAsync(Tkey id)
        {
            return await RepositoryContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            entity.CreateTime = DateTime.UtcNow;
            entity.CreatedUserId = RepositoryContext._requestInfo.UserId;
            var entry = this.RepositoryContext.Set<TEntity>().Add(entity);
            await this.RepositoryContext.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(TEntity entity)
        {
            entity.EditTime = DateTime.UtcNow;
            entity.EditUserId = RepositoryContext._requestInfo.UserId;
            this.RepositoryContext.Entry(entity).State = EntityState.Modified;
            await this.RepositoryContext.SaveChangesAsync();
        }
        public void FlagRange(IEnumerable<TEntity> entities, EntityState newState)
        {
            switch (newState)
            {
                case EntityState.Added:
                    this.RepositoryContext.AddRange(entities);
                    break;
                case EntityState.Modified:
                    this.RepositoryContext.UpdateRange(entities);
                    break;
                case EntityState.Deleted:
                    this.RepositoryContext.RemoveRange(entities);
                    break;
            }
        }
        public async Task<int> SaveChangesAsync()
        {
            return await this.RepositoryContext.SaveChangesAsync();
        }
    }
    public interface IEFRepositorty<TEntity, Tkey>
        where TEntity : class, IBaseEntity<Tkey>, new()
    {
        Task<int> SaveChangesAsync();
        void FlagRange(IEnumerable<TEntity> entities, EntityState newState);
        IQueryable<TEntity> FindAll();
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool suppressEntityTracking = true);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<bool> Exist(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> FindAsync(Tkey id);
        Task<TEntity> CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
    }
}
