using System.ComponentModel.DataAnnotations;

namespace EduResourceAPI.Models.Entities
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Description { get; set; } = null!;

        public ICollection<Material> Materials { get; set; } = null!;
    }
}
