//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoV1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vertice
    {
        public int id { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public int poligono_id { get; set; }
    
        public virtual poligono poligono { get; set; }
    }
}
