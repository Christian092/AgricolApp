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
    
    public partial class tipo_tratamiento
    {
        public tipo_tratamiento()
        {
            this.tratamiento_animal = new HashSet<tratamiento_animal>();
            this.tratamiento = new HashSet<tratamiento>();
        }
    
        public int id { get; set; }
        public string nombre { get; set; }
    
        public virtual ICollection<tratamiento_animal> tratamiento_animal { get; set; }
        public virtual ICollection<tratamiento> tratamiento { get; set; }
    }
}
