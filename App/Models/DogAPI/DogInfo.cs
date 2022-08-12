using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class DogInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string life_span { get; set; }
        public string temperament { get; set; }
        public Weight weight { get; set; }
        public Height height { get; set; }
    }
}