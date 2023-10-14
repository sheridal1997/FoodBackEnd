using System.ComponentModel.DataAnnotations.Schema;

namespace Food_Backend.Entity
{
    public class Recipe : BaseEnntity<int>
    {
        public string? Title { get; set; }
        public string? Discription { get; set; }
        public DateTime? Date { get; set; }
        public int? RecipeVideoId { get; set; }
        [NotMapped]
        public TimeSpan? Time { get; set; }
        public bool IsShuffle { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        public string? FileUrl { get; set; }
        public string? ObjectName { get; set; }
        //public bool IsFavorite { get; set; }
        //public RecipeRating? RecipeRating { get; set; }

        public RecipeVideo? RecipeVideo { get; set; }
        private ICollection<WeeklyPlan> _weeklyPlan;
        public ICollection<WeeklyPlan> WeeklyPlans
        {
            get => _weeklyPlan ?? (_weeklyPlan = new List<WeeklyPlan>());
        }
        private ICollection<UserFavorite> _userFavorite;
        public ICollection<UserFavorite> UserFavorite
        {
            get => _userFavorite ?? (_userFavorite = new List<UserFavorite>());
        }

        private ICollection<UserRating> _userRating;
        public ICollection<UserRating> UserRating
        {
            get => _userRating ?? (_userRating = new List<UserRating>());
        }
        [NotMapped]
        public UserFavorite? FavoriteUser { get; set; }
        [NotMapped]
        public UserRating? RatingUser { get; set; }
    }
    
}
