using System.ComponentModel.DataAnnotations;

namespace EduResourceAPI.Models.DTOs
{
    public class CategoryCreateDTO
    {
        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Definition { get; set; } = null!;
    }
}
