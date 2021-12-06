namespace EduResourceAPI.Models.DTOs
{
    public class AuthorReadDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int MaterialsCount { get; set; }
    }
}
