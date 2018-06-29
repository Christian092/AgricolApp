using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    [MetadataType(typeof(inseminadorMeta))]
    public partial class inseminador
    {

    }

    public class inseminadorMeta
    {
        [Display(Name = "ID")]

        public int id { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Apellido")]
        public string apellido { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Numero")]
        public string numero { get; set; }
    }
}