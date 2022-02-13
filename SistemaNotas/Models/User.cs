using System.ComponentModel.DataAnnotations;

namespace SistemaNotas.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
