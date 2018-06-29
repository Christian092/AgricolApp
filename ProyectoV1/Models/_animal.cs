using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoV1.Models
{
    [MetadataType(typeof(animalMeta))]
    public partial class animal    {
       
    }
    public class animalMeta
    {
        [Remote("codigoValido", "Animal", ErrorMessage = "Código del Animal ya Existe !")]
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Codigo SAG"),]
        public string codigo_sag { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Sexo")]
        public string sexo { get; set; }
        [Display(Name = "Nacimiento")]
        [Remote("fechaValida", "Animal", ErrorMessage = "Fecha de nacimiento no puede ser mayor a la fecha actual")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> fec_nac { get; set; }
        [Display(Name = "Ingreso")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime fecha_ing { get; set; }
        
        
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Tipo")]
        public int tipo_id { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Raza")]
        public int raza_id { get; set; }
        [Required(ErrorMessage = "Necesitamos este dato, es fundamental !")]
        [Display(Name = "Estado")]
        public int estado_id { get; set; }
    }

}