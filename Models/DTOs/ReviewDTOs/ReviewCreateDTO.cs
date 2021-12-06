using System.ComponentModel.DataAnnotations;

namespace EduResourceAPI.Models.DTOs
{
    public class ReviewCreateDTO
    {
        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Text { get; set; } = null!;

        [Required]
        [Range(1,10)]
        public int Score { get; set; }
    }
}
