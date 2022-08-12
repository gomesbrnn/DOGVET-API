using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.DTO.Clinica
{
    public class CriarClinicaDTO
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Cnpj { get; set; }
        [Required]
        public string Endereco { get; set; }
    }
}