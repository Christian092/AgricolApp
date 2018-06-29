using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    [MetadataType(typeof(medicamentoMeta))]
    public partial class medicamento
    {

    }

    public class medicamentoMeta
    {
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "ID")]
        public int id { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        [StringLength(100, ErrorMessage ="Maximo 100 caracteres")]
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }
    }
}