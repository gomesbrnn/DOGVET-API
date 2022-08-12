using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Models
{
    public class Atendimento
    {
        public int Id { get; set; }
        public Clinica Clinica { get; set; }
        [JsonIgnore]
        public int ClinicaId { get; set; }
        public Veterinario Veterinario { get; set; }
        [JsonIgnore]
        public int VeterinarioId { get; set; }
        public Tutor Tutor { get; set; }
        [JsonIgnore]
        public int TutorId { get; set; }
        public Animal Animal { get; set; }
        [JsonIgnore]
        public int AnimalId { get; set; }
        public DateTime DataHora { get; set; }
        public string DadosDoDia { get; set; }
        public string Diagnostico { get; set; }
        public string Comentarios { get; set; }
        public bool StatusAtendimento { get; set; } = true;
    }
}