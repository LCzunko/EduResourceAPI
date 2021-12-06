namespace EduResourceAPI.Models.DTOs
{
    public class CategoryReadDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Definition { get; set; } = null!;
    }
}
