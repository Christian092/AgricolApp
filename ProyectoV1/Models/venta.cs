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
    
    public partial class venta
    {
        public int id { get; set; }
        public System.DateTime fecha { get; set; }
        public int pesaje { get; set; }
        public int precio { get; set; }
        public int animal_id { get; set; }
        public int comprador_id { get; set; }
    
        public virtual animal animal { get; set; }
        public virtual comprador comprador { get; set; }
    }
}
