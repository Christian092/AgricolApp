using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    public class MorNat : IComparable<MorNat>
    {
        public int Natalidad { get; set; }
        public int Mortalidad { get; set; }
        public DateTime Fecha { get; set; }

        public MorNat(DateTime f, int m, int n)
        {
            Natalidad = n;
            Mortalidad = m;
            Fecha = f;
        }

        public int CompareTo(MorNat other)
        {
            if (this.Fecha.Year < other.Fecha.Year)
                return -1;
            if (this.Fecha.Year == other.Fecha.Year)
                return 0;
            return 1;
        }
    }
}