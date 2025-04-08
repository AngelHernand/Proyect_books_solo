using System.ComponentModel.DataAnnotations;

namespace Proyect_Solo_Recommendation.Models
{
    public class EditProfileViewModel
    {
        public int Id { get; set; } // ID del usuario
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? CurrentPassword { get; set; }
        
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string? NewPassword { get; set; } // Nueva contraseña
        
        public IFormFile? ProfileImage { get; set; }
        public int RoleId { get; set; }
    }
}
