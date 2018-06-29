using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProyectoV1.Models;

namespace ProyectoV1.Utils
{
    public class Shared
    {
        public static int AlertaMantencion()
        {
            DateTime mañana = DateTime.Today.AddDays(1);
            var db = new bdagricolaEntities();
            int alertasMantencion = db.alerta_mantencion.Where(m => m.fecha.Equals(mañana)).Count();
            return alertasMantencion;
        }
        public static int AlertaTratamiento()
        {
            DateTime mañana = DateTime.Today.AddDays(1);
            var db = new bdagricolaEntities();
            int alertaTratamiento = db.tratamiento.Where(m => m.fecha.Equals(mañana)).Count();
            return alertaTratamiento;
        }
        public static int AlertaTratamientoAnimal()
        {
            DateTime mañana = DateTime.Today.AddDays(1);
            var db = new bdagricolaEntities();
            int alertaTratamientoAnimal = db.tratamiento_animal.Where(m => m.fecha.Equals(mañana)).Count();
            return alertaTratamientoAnimal;
        }
        public static int TotalGanancias()
        {
            List<venta> ventas = new List<venta>();

            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                ventas = dc.venta.ToList();

            }
            int contador = 0;
            foreach (var a in ventas)
            {
               contador = a.precio + contador;
            }
            return contador;
        }
    }
}