using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    [MetadataType(typeof(tratamientoMeta))]
    public partial class tratamiento
    {

    }

    public class tratamientoMeta
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Fecha del Tratamiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime fecha { get; set; }
       
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Tratador")]
        public int tratador_id { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Tipo de Tratamiento")]
        public int tipo_tratamiento_id { get; set; }
    }
}