using System.ComponentModel.DataAnnotations.Schema;

namespace Food_Backend.Entity
{
    public class RecipeVideo : BaseEnntity<int>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        public string? FileUrl { get; set; }
        public string? ObjectName { get; set; }
        public DateTime? Date { get; set; }
        [NotMapped]
        public TimeSpan? Time { get; set; }
        private ICollection<Recipe> _recipe;
        public ICollection<Recipe> Recipe
        {
            get => _recipe ?? (_recipe = new List<Recipe>());
        }
    }
}
