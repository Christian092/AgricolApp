using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    public class gpsinsideOutside
    {
        public List<gpsInside> inside { get; set; }
        public List<gpsOutside> outside { get; set; }
    }
}