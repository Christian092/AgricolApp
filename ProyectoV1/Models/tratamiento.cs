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
    
    public partial class tratamiento
    {
        public int id { get; set; }
        public System.DateTime fecha { get; set; }
        public int tratador_id { get; set; }
        public int tipo_tratamiento_id { get; set; }
    
        public virtual tipo_tratamiento tipo_tratamiento { get; set; }
        public virtual tratador tratador { get; set; }
    }
}
