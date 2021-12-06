using System.ComponentModel.DataAnnotations;

namespace EduResourceAPI.Models.DTOs.AuthDTOs
{
    public class AuthLoginDTO
    {
        [Required]
        [StringLength(256, MinimumLength = 5)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Password { get; set; } = null!;
    }
}
