using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.DTO.Atendimento
{
    public class AtualizarAtendimentoDTO
    {
        [Required]
        public DateTime DataHora { get; set; }
        public string DadosDoDia { get; set; }
        public string Diagnostico { get; set; }
        public string Comentarios { get; set; }
    }
}