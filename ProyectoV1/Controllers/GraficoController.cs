using ProyectoV1.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using System.Data;
using System.Data.Entity;
using System;
using System.Web.UI.WebControls;
using PagedList;

namespace ProyectoV1.Controllers
{
    public class GraficoController : Controller
    {
        public string Nombre;
        private bdagricolaEntities db = new bdagricolaEntities();
        // GET: Grafico

        public ActionResult CantidadMuertes()
        {
            return View();
        }

        public ActionResult GraficoProduccion()
        {
            return View();
        }

        public ActionResult TipoPrueba()
        {
            return View();
        }
        public ActionResult GraficoTipos()
        {
            return View();
        }
        public ActionResult GraficoInseminaciones()
        {
            return View();
        }
        public ActionResult GraficoGanancias()
        {
            return View();
        }
        public ActionResult GraficoRazas()
        {
            return View();
        }
        public ActionResult Produccion(int? id)
        {

            var _context = new bdagricolaEntities();

            ArrayList xValor = new ArrayList();
            ArrayList yValor = new ArrayList();



            var results = (from c in _context.ordena select c);

            results.ToList().ForEach(rs => xValor.Add(rs.fecha));
            results.ToList().ForEach(rs => yValor.Add(rs.litros));
            new Chart(width: 800, height: 500, theme: ChartTheme.Blue)
                .AddTitle("PRODUCCION DE LECHE")
                .AddLegend(title: "Produccion")
            .AddSeries(name: "Litros", chartType: "Column", xValue: xValor, yValues: yValor)


            .Write("bmp");
            return null;



        }
        public ActionResult SexoAnimales()
        {

            var bd = new bdagricolaEntities();
            int machos = bd.animal.Where(m => m.sexo == "M").Count();
            int hembras = bd.animal.Where(m => m.sexo == "H").Count();
            List<int> yValues = new List<int>();
            yValues.Add(machos);
            yValues.Add(hembras);
            List<string> xValues = new List<string>();
            xValues.Add("MACHOS");
            xValues.Add("HEMBRAS");

            new Chart(width: 500, height: 400, theme: ChartTheme.Blue)

                .AddTitle("SEXO DE ANIMALES")

                .AddLegend(title: "SEXO")
            .AddSeries(chartType: "Pie", xValue: xValues, yValues: yValues, name: "Animales")


            .Write("bmp")
            ;

            return null;
        }
        public ActionResult TipoAnimales()
        {

            var bd = new bdagricolaEntities();
            int terneros = bd.animal.Where(m => m.tipo.nombre == "Ternero").Count();
            int vacas = bd.animal.Where(m => m.tipo.nombre == "Vaca").Count();
            int novillos = bd.animal.Where(m => m.tipo.nombre == "Novillo").Count();
            int toros = bd.animal.Where(m => m.tipo.nombre == "Toro").Count();
            int terneras = bd.animal.Where(m => m.tipo.nombre == "Ternera").Count();



            List<int> yValues = new List<int>();
            yValues.Add(terneros);
            yValues.Add(vacas);
            yValues.Add(novillos);
            yValues.Add(toros);
            yValues.Add(terneras);
            List<string> xValues = new List<string>();
            xValues.Add("TERNEROS");
            xValues.Add("VACAS");
            xValues.Add("NOVILLOS");
            xValues.Add("TOROS");
            xValues.Add("TERNERAS");

            new Chart(width: 450, height: 450, theme: ChartTheme.Blue)
                .AddTitle("TIPO DE ANIMALES")
                .AddLegend(title: "TIPOS")
            .AddSeries(chartType: "Pie", xValue: xValues, yValues: yValues, name: "Animales")

            .Write("bmp")

            ;
            return null;

        }


        public ActionResult GraficoSexo(int? page)
        {
            var animal = db.animal;
            return View(animal.ToList().ToPagedList(page ?? 1, 5));
        }

        public JsonResult GetSalesData()
        {
            List<ordena> sd = new List<ordena>();

            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                sd = dc.ordena.OrderBy(a => a.fecha).ToList();

            }
            List<ordenaJornada> chartData = new List<ordenaJornada>();

            DateTime fechaAnterior = DateTime.Today;
            var fecha = DateTime.Today;

            foreach (var ordena in sd)
            {
                var litrosM = 0;
                var litrosT = 0;
                var tieneOrdena = false;
                if (ordena.jornada == "M")
                {
                    litrosM = ordena.litros;
                }
                if (ordena.jornada == "T")
                {
                    litrosT = ordena.litros;
                }

                while (fecha.Subtract(fechaAnterior).Days > 1)
                {
                    chartData.Add(new ordenaJornada(fechaAnterior.AddDays(1), 0, 0));
                    fechaAnterior = fechaAnterior.AddDays(1);
                }

                fechaAnterior = fecha.Date;
                fecha = ordena.fecha.Date;
                chartData.Add(new ordenaJornada(fecha.Date, litrosM, litrosT));

            }

            chartData.Sort();
            var datos = new List<ordenaJornadaStr>();
            var datosCh = new List<object[]>();

            var fAnterior = new DateTime(1, 1, 1);
            var i = 0;
            foreach (var ch in chartData)
            {
                if (fAnterior == new DateTime(1, 1, 1))
                {
                    datos.Add(new ordenaJornadaStr(ch.Fecha.Date, ch.LitrosManana, ch.LitrosTarde));
                }
                else
                {
                    var fActual = ch.Fecha;
                    if (fAnterior != fActual)
                    {
                        datos.Add(new ordenaJornadaStr(ch.Fecha.Date, ch.LitrosManana, ch.LitrosTarde));
                    }
                    else
                    {
                        var datoActual = ch;
                        if (datoActual.LitrosManana > 0)
                        {
                            datos[i - 1].LitrosManana += datoActual.LitrosManana;
                        }
                        if (datoActual.LitrosTarde > 0)
                        {
                            datos[i - 1].LitrosTarde += datoActual.LitrosTarde;
                        }
                        i--;
                    }
                }
                fAnterior = ch.Fecha.Date;
                i++;
            }

            foreach (var d in datos)
            {
                datosCh.Add(new object[] { d.Fecha.ToString("dd/MM/yyyy"), d.LitrosManana, d.LitrosTarde });

            }
            return new JsonResult { Data = datosCh, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult GetSalesData(DateTime fecha1, DateTime fecha2)
        {
            List<ordena> sd = new List<ordena>();

            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                sd = dc.ordena.Where(a => a.fecha >= fecha1 && a.fecha <= fecha2).OrderBy(a => a.fecha).ToList();

            }
            List<ordenaJornada> chartData = new List<ordenaJornada>();

            DateTime fechaAnterior = DateTime.Today;
            var fecha = DateTime.Today;

            foreach (var ordena in sd)
            {
                var litrosM = 0;
                var litrosT = 0;
                var tieneOrdena = false;
                if (ordena.jornada == "M")
                {
                    litrosM = ordena.litros;
                }
                if (ordena.jornada == "T")
                {
                    litrosT = ordena.litros;
                }

                while (fecha.Subtract(fechaAnterior).Days > 1)
                {
                    chartData.Add(new ordenaJornada(fechaAnterior.AddDays(1), 0, 0));
                    fechaAnterior = fechaAnterior.AddDays(1);
                }

                fechaAnterior = fecha.Date;
                fecha = ordena.fecha.Date;
                chartData.Add(new ordenaJornada(fecha.Date, litrosM, litrosT));

            }

            chartData.Sort();
            var datos = new List<ordenaJornadaStr>();
            var datosCh = new List<object[]>();

            var fAnterior = new DateTime(1, 1, 1);
            var i = 0;
            foreach (var ch in chartData)
            {
                if (fAnterior == new DateTime(1, 1, 1))
                {
                    datos.Add(new ordenaJornadaStr(ch.Fecha.Date, ch.LitrosManana, ch.LitrosTarde));
                }
                else
                {
                    var fActual = ch.Fecha;
                    if (fAnterior != fActual)
                    {
                        datos.Add(new ordenaJornadaStr(ch.Fecha.Date, ch.LitrosManana, ch.LitrosTarde));
                    }
                    else
                    {
                        var datoActual = ch;
                        if (datoActual.LitrosManana > 0)
                        {
                            datos[i - 1].LitrosManana += datoActual.LitrosManana;
                        }
                        if (datoActual.LitrosTarde > 0)
                        {
                            datos[i - 1].LitrosTarde += datoActual.LitrosTarde;
                        }
                        i--;
                    }
                }
                fAnterior = ch.Fecha.Date;
                i++;
            }

            foreach (var d in datos)
            {
                datosCh.Add(new object[] { d.Fecha.ToString("dd/MM/yyyy"), d.LitrosManana, d.LitrosTarde });

            }
            return new JsonResult { Data = datosCh, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ObtenerTiposAnimal()
        {
            var bd = new bdagricolaEntities();
            List<animal> animales = new List<animal>();
            List<tipo> tipos = new List<tipo>();
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                animales = dc.animal.Where(a => a.estado.nombre != "Vendido" && a.estado.nombre != "Muerto").ToList();

                tipos = dc.tipo.ToList();
            }
            var chartData = new List<object[]>();

            var query = from x in animales
                        group x.tipo.nombre by x.tipo.nombre into g
                        let count = g.Count()
                        orderby g.Key ascending
                        select new { tipo = g.Key, cantidad = count };
            chartData.Add(new object[] { "Tipo", "Cantidad" });
            foreach (var d in query)
            {
                chartData.Add(new object[] { d.tipo, d.cantidad });
            }
            return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ObtenerSexoAnimales()
        {
            var bd = new bdagricolaEntities();
            List<animal> sd = new List<animal>();
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                sd = dc.animal.Where(a => a.estado.nombre != "Vendido" && a.estado.nombre != "Muerto").ToList();

            }
            int machos = sd.Where(m => m.sexo == "M").Count();
            int hembras = sd.Where(m => m.sexo == "H").Count();

            var chartData = new object[3];
            chartData[0] = new object[]{
                "Sexo",
                "Cantidad"
            };

            chartData[1] = new object[] { "Machos", machos };
            chartData[2] = new object[] { "Hembras", hembras };

            return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ObtenerCantidadMuertes()
        {
            List<muerte> muertes = new List<muerte>();

            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                muertes = dc.muerte.OrderBy(a => a.fecha).ToList();

            }
            var chartData = new List<object[]>();

            var query = from x in muertes
                        group x.fecha.Year by x.fecha.Year into g
                        let count = g.Count()
                        orderby g.Key ascending
                        select new { año = g.Key, cantidad = count };
            chartData.Add(new object[] { "Año", "Cantidad de Muertes" });
            foreach (var d in query)
            {
                chartData.Add(new object[] { d.año.ToString(), d.cantidad });
            }
            return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        public JsonResult ObtenerInseminacionesAnuales()
        {
            List<inseminacion> inseminaciones = new List<inseminacion>();

            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                inseminaciones = dc.inseminacion.OrderBy(a => a.fecha).ToList();

            }
            var chartData = new List<object[]>();

            var query = from x in inseminaciones
                        group x.fecha.Year by x.fecha.Year into g
                        let count = g.Count()
                        orderby g.Key ascending
                        select new { año = g.Key, cantidad = count };
            chartData.Add(new object[] { "Año", "Cantidad de Inseminaciones" });
            foreach (var d in query)
            {
                chartData.Add(new object[] { d.año.ToString(""), d.cantidad });
            }
            return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ObtenerGanancias()
        {
            List<venta> ventas = new List<venta>();

            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                ventas = dc.venta.OrderBy(a => a.fecha.Year).ToList();

            }


            var query = from x in ventas
                        group x.precio by x.fecha.Year into g
                        let count = g.Sum()
                        orderby g.Key ascending
                        select new { año = g.Key, cantidad = count };
            var chartData = new List<object[]>();
            chartData.Add(new object[] { "Año", "Recaudacion de Ventas de Animales" });
            foreach (var d in query)
            {
                chartData.Add(new object[] { d.año.ToString(""), d.cantidad });
            }
            return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ObtenerRaza()
        {
            var bd = new bdagricolaEntities();
            List<animal> animales = new List<animal>();
            List<raza> razas = new List<raza>();
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                animales = dc.animal.Where(a => a.estado.nombre != "Vendido" && a.estado.nombre != "Muerto").ToList();

                razas = dc.raza.ToList();
            }
            var chartData = new List<object[]>();

            var query = from x in animales
                        group x.raza.nombre by x.raza.nombre into g
                        let count = g.Count()
                        orderby g.Key ascending
                        select new { raza = g.Key, cantidad = count };
            chartData.Add(new object[] { "Raza", "Cantidad" });
            foreach (var d in query)
            {
                chartData.Add(new object[] { d.raza, d.cantidad });
            }
            return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}