using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    [MetadataType(typeof(alertaMantencionMeta))]
    public partial class alerta_mantencion
    {

    }

    public class alertaMantencionMeta
    {
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Fecha de Mantención")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime fecha { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        public int lecheria_id { get; set; }
    }
  
}