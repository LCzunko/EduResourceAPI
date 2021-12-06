using System.ComponentModel.DataAnnotations;

namespace EduResourceAPI.Models.Entities
{
    public class Material
    {
        [Key]
        public int Id { get; set; }

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
        public Author Author { get; set; } = null!;

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public Category Category { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }

        public ICollection<Review> Reviews { get; set; } = null!;
    }
}
