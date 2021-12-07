namespace EduResourceAPI.Models.DTOs
{
    public class MaterialReadDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Location { get; set; } = null!;

        public DateTime Published { get; set; }

        public int AuthorId { get; set; }

        public string AuthorName { get; set; } = null!;

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;
    }
}
