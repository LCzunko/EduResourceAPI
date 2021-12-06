using System.ComponentModel.DataAnnotations;

namespace EduResourceAPI.Models.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Text { get; set; } = null!;

        [Required]
        [Range(1,10)]
        public int Score { get; set; }

        [Required]
        public Material Material { get; set; } = null!;

        [Required]
        public int MaterialId { get; set; }
    }
}
