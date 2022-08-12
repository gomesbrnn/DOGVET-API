using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Models
{
    public class Autenticacao
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool IsFuncionario { get; set; }
        public bool Status { get; set; } = true;
    }
}