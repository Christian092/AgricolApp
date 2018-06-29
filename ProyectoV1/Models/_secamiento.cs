using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoV1.Models
{
    [MetadataType(typeof(secamientoMeta))]
    public partial class secamiento
    {

    }

    public class secamientoMeta
    {

        [Display(Name = "ID")]
        public int id { get; set; }
        [Remote("fechaValida", "Secamiento", ErrorMessage = "Fecha no puede ser mayor a la fecha actual")]
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime fecha { get; set; }
        [Range(1, 4, ErrorMessage = "Las Cantidad es entre 1 y 4")]
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Cantidad de Tetas Secadas")]
        public int cantidad_ubres { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Animal")]
        public int animal_id { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Medicamento")]
        public int medicamento_id { get; set; }
    }
}