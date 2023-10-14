using Food_Backend.Commom;
using Food_Backend.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;
using System.Linq.Expressions;
namespace Food_Backend
{
    public class DbContexD : DbContext
    {
      
        public readonly IRequestHeader _requestInfo;

        public DbContexD(DbContextOptions options, IRequestHeader requestHeader)
            : base(options)
        {
            _requestInfo = requestHeader;
        }

        protected ModelBuilder _modelBuilder;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _modelBuilder = modelBuilder;
            Entities();
        }

        public void Entities()
        {
            this.InitializeEntity<User>();
            this.InitializeEntity<RecipeVideo>();
            this.InitializeEntity<Recipe>();
            this.InitializeEntity<WeeklyPlan>();
            this.CreateRelation<Recipe, WeeklyPlan>(o => o.WeeklyPlans, o => o.Recipe, o => o.RecipeId);
            this.InitializeEntity<SystemSetting>();
            this.InitializeEntity<UserFavorite>();
            this.InitializeEntity<UserRating>();
            this.CreateRelation<Recipe, UserFavorite>(o => o.UserFavorite, o => o.Recipe, o => o.RecipeId);
            this.CreateRelation<Recipe, UserRating>(o => o.UserRating, o => o.Recipe, o => o.RecipeId);


        }
        protected virtual EntityTypeBuilder<TEntity> InitializeEntity<TEntity>()
            where TEntity : class, IBaseEntity
        {
            TEntity entity;
            var entityTypeBuilder = _modelBuilder.Entity<TEntity>();

            entityTypeBuilder
                .HasKey(nameof(entity.Id));

            entityTypeBuilder
                .HasQueryFilter(O => !O.IsDelete);

            return entityTypeBuilder;
        }
        protected void CreateRelation<TEntity, TRelated>(Expression<Func<TEntity, IEnumerable<TRelated>>> navigationExpressionMany,
           Expression<Func<TRelated, TEntity>> navigationExpressionOne, Expression<Func<TRelated, dynamic>> foreignKeyExpression)
           where TEntity : class, IBaseEntity
           where TRelated : class, IBaseEntity
        {
            _modelBuilder.Entity<TEntity>()
                .HasMany(navigationExpressionMany)
                .WithOne(navigationExpressionOne)
                .HasForeignKey(foreignKeyExpression)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
