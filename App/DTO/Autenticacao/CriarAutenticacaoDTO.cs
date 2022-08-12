using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.DTO.Autenticacao
{
    public class CriarAutenticacaoDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Senha { get; set; }
        [JsonIgnore]
        public bool IsFuncionario { get; set; }

    }
}