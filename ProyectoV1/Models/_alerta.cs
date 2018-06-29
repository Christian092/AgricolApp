using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    [MetadataType(typeof(alertaMeta))]
    public partial class alerta
    {

    }
    public class alertaMeta
    {
        public int id { get; set; }
        [Display(Name = "Fecha de la Mantencion")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime fecha { get; set; }
        [Display(Name = "Tipo de Tratamiento")]
        public int tipo_tratamiento_id { get; set; }
    }
}