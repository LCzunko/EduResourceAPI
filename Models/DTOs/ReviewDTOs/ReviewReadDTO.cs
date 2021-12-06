namespace EduResourceAPI.Models.DTOs
{
    public class ReviewReadDTO
    {
        public int Id { get; set; }

        public string Text { get; set; } = null!;

        public int Score { get; set; }

        public int MaterialId { get; set; }
    }
}
