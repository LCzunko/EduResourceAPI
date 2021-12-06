using System.ComponentModel.DataAnnotations;

namespace EduResourceAPI.Models.DTOs
{
    public class MaterialCreateDTO
    {
        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Description { get; set; } = null!;

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Location { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Published { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
