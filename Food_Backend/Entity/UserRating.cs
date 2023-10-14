namespace Food_Backend.Entity
{
    public class UserRating : BaseEnntity<int>
    {
        public int? RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
        public double RecipeRating { get; set; }
    }
    
}
