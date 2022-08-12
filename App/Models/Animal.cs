using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Raca { get; set; }
        public string Peso { get; set; }
        public DateTime DataNascimento { get; set; }
        [JsonIgnore]
        public Tutor Tutor { get; set; }
        public int TutorId { get; set; }
        public bool Status { get; set; } = true;
    }
}