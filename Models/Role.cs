using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyect_Solo_Recommendation.Models
{
    [Table("Role")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        // Relación de uno a muchos: Un rol puede tener muchos usuarios
        public required ICollection<User> Users { get; set; }
    }
}
