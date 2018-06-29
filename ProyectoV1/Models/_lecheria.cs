using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    [MetadataType(typeof(lecheriaMeta))]
    public partial class lecheria
    {

    }

    public class lecheriaMeta
    {
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Capacidad")]
        public int capacidad { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Ordeñas Diarias")]
        public int ordenas_diarias { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Cantidad de Trabajadores")]
        public int cantidad_trabajadores { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Tipo ")]
        public int tipo_lecheria_id { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Fabricante")]
        public int fabricante_id { get; set; }
    }
}