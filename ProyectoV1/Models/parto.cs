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
    
    public partial class parto
    {
        public parto()
        {
            this.nace_muerto = new HashSet<nace_muerto>();
        }
    
        public int id { get; set; }
        public System.DateTime fecha { get; set; }
        public int madre_id { get; set; }
        public Nullable<int> animal_id { get; set; }
    
        public virtual animal animal { get; set; }
        public virtual animal animal1 { get; set; }
        public virtual ICollection<nace_muerto> nace_muerto { get; set; }
    }
}
