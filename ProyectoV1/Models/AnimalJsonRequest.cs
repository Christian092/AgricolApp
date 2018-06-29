using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    public class AnimalJsonRequest
    {
        public int id { get; set; }
        public string codigo_sag { get; set; }
        public string sexo { get; set; }
        public int edad { get; set; }
        public string tipo { get; set; }
        public string raza { get; set; }
    }
}