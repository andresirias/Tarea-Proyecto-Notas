using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaNotas.Models
{
    public class Nota
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Text { get; set; }
    }
}
