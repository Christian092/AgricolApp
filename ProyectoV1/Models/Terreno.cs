using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    public class Terreno
    {
        public List<gps> Marcadores { get; set; }
        public List<vertice> vertices { get; set; }
    }
}