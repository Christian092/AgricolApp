using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    public class ordenaJornadaStr
    {
        public DateTime Fecha { get; set; }
        public int LitrosManana { get; set; }
        public int LitrosTarde { get; set; }

        public ordenaJornadaStr(DateTime f, int lm, int lt)
        {
            Fecha = f.Date;
            Fecha = Fecha.Date;
            
           
            LitrosManana = lm;
            LitrosTarde = lt;
        }
    }
}