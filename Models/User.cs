using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Proyect_Solo_Recommendation.Models
{
    [Table("User")]

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Password { get; set; }

        [MaxLength(50)]
        public required string Firstname { get; set; }

        [MaxLength(50)]
        public required string Lastname { get; set; }

        [MaxLength(100)]
        public required string Description { get; set; }
        public string? ProfileImage { get; set; }


        [Required]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role? Role { get; set; }
    }
}
