using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoV1.Models
{
    [MetadataType(typeof(partoMeta))]
    public partial class parto
    {

    }

    public class partoMeta
    {
        [Remote("fechaValida", "Parto", ErrorMessage = "Fecha del Parto no puede ser mayor a la fecha actual")]
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime fecha { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Madre")]
        public int madre_id { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Animal Nacido")]
        public Nullable<int> animal_id { get; set; }
    }
}