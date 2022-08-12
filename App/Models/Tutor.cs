using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Models
{
    public class Tutor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Animal> Animal { get; set; }
        [JsonIgnore]
        public int AnimalId { get; set; }
        public bool IsCliente { get; set; } = true;
    }
}