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
    
    public partial class poligono
    {
        public poligono()
        {
            this.vertice = new HashSet<vertice>();
        }
    
        public int id { get; set; }
        public int usuario_id { get; set; }
    
        public virtual usuario usuario { get; set; }
        public virtual ICollection<vertice> vertice { get; set; }
    }
}
