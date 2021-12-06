using System.ComponentModel.DataAnnotations;

namespace EduResourceAPI.Models.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Definition { get; set; } = null!;

        public ICollection<Material> Materials { get; set; } = null!;
    }
}
