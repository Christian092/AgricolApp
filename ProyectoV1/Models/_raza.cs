using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    [MetadataType(typeof(razaMeta))]
    public partial class raza
    {

    }

    public class razaMeta
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Procedencia")]
        public int procedencia_id { get; set; }
    }
}