using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    [MetadataType(typeof(UsuarioMetadatos))]
    public partial class usuario
    {

    }
    class UsuarioMetadatos
    {
        [Required(ErrorMessage = "El Rut es Obligatorio")]
        [RutValidator]
        public string rut { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La Contraseña es Obligatoria")]
        public string clave { get; set; }
        public string rol { get; set; }
    }
}