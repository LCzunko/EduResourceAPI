namespace EduResourceAPI.Models.DTOs
{
    public class MaterialReadDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Location { get; set; } = null!;

        public DateTime Published { get; set; }

        public AuthorReadDTO Author { get; set; } = null!;

        public CategoryReadDTO Category { get; set; } = null!;
    }
}
