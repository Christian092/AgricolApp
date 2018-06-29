using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    public class MorNatStr
    {
        public DateTime Fecha { get; set; }
        public int Natalidad { get; set; }
        public int Mortalidad { get; set; }

        public MorNatStr(DateTime f, int m, int n)
        {
            Fecha = f;
            Natalidad = n;
            Mortalidad = m;
        }
    }
}