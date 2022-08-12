using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.DTO.Tutor
{
    public class CriarTutorDTO
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Cpf { get; set; }
    }
}