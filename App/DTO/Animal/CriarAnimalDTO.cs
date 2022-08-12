using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.DTO.Animal
{
    public class CriarAnimalDTO
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Raca { get; set; }
        [Required]
        public string Peso { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        [Required]
        public int TutorId { get; set; }
    }
}