using System.ComponentModel.DataAnnotations;

namespace EduResourceAPI.Models.DTOs.AuthDTOs
{
    public class AuthRegistrationDTO
    {
        [Required]
        [StringLength(256, MinimumLength = 1)]
        [RegularExpression("^[a-zA-Z0-9]+$")]
        public string UserName { get; set; } = null!;

        [Required]
        [StringLength(256, MinimumLength = 5)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Password { get; set; } = null!;
    }
}
