using System.ComponentModel.DataAnnotations;

namespace EduResourceAPI.Models.DTOs
{
    public class AuthorCreateDTO
    {
        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Description { get; set; } = null!;
    }
}
