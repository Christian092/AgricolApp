using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    [MetadataType(typeof(tratamientoAnimalMeta))]
    public partial class tratamiento_animal
    {

    }

    public class tratamientoAnimalMeta
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Fecha del Tratamiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime fecha { get; set; }

        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Animal")]
        public int animal_id { get; set; }

        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Tratador")]
        public int tratador_id { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Tipo de Tratamiento")]
        public int tipo_tratamiento_id { get; set; }
    }
}
