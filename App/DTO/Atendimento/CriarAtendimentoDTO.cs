using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.DTO.Atendimento
{
    public class CriarAtendimentoDTO
    {
        [Required]
        public int ClinicaId { get; set; }
        [Required]
        public int VeterinarioId { get; set; }
        [Required]
        public int TutorId { get; set; }
        [Required]
        public int AnimalId { get; set; }
        [Required]
        public DateTime DataHora { get; set; }
        [Required]
        public string DadosDoDia { get; set; }
        [Required]
        public string Diagnostico { get; set; }
        [Required]
        public string Comentarios { get; set; }
    }
}