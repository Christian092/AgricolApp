using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    public class gpsOutside
    {
        public Nullable<double> latitud { get; set; }
        public Nullable<double> longitud { get; set; }
        public int animal_Id { get; set; }
        public string animal_Sexo { get; set; }
        public string animal_Tipo { get; set; }
        public string animal_Raza { get; set; }
    }
}