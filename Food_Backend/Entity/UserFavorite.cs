namespace Food_Backend.Entity
{
    public class UserFavorite : BaseEnntity<int>
    {
        public int? RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
        public bool IsFavorite { get; set; }
        

    }
}
