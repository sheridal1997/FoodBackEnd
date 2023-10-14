namespace Food_Backend.Entity
{
    public class WeeklyPlan : BaseEnntity<int>
    {
        public string? Name { get; set; }
        public int? RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }

    }
}
