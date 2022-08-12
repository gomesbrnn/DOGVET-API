using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class Veterinario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string numeroRegistro { get; set; }
        public bool IsFuncionario { get; set; } = true;
    }
}