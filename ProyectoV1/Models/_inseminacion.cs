using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoV1.Models
{
    [MetadataType(typeof(_inseminacionMeta))]
    public partial class inseminacion
    {

    }

    public class _inseminacionMeta
    {
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "ID")]
        public int id { get; set; }
        [Remote("fechaValida", "Inseminacion", ErrorMessage = "Fecha no puede ser mayor a la fecha actual")]
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime fecha { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Inseminador")]
        public int inseminador_id { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Animal")]
        public int animal_id { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Procedencia del Semen")]
        public int procedencia_semen_id { get; set; }
    }
}