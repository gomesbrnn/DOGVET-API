using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.DTO.Veterinario
{
    public class CriarVeterinarioDTO
    {
        [Required]
        [MaxLength(30)]
        public string Nome { get; set; }
        [Required]
        [Range(10000000000, 99999999999)]
        public string numeroRegistro { get; set; }
    }
}