using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.DTO.Clinica
{
    public class AtualizarClinicaDTO
    {

        public string Nome { get; set; }

        public string Cnpj { get; set; }

        public string Endereco { get; set; }
    }
}