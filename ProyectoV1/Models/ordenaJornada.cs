using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    public class ordenaJornada : IComparable<ordenaJornada>
    {
        public int LitrosManana { get; set; }
        public int LitrosTarde { get; set; }
        public DateTime Fecha { get; set; }

        public ordenaJornada(DateTime f,int lm,int lt)
        {
            LitrosManana = lm;
            LitrosTarde = lt;
            Fecha = f;
            Fecha = Fecha.Date;
        }

        public int CompareTo(ordenaJornada other)
        {
            if(this.Fecha < other.Fecha)
                return -1;
            if (this.Fecha == other.Fecha)
                return 0;
            return 1;
        }
    }
    

}