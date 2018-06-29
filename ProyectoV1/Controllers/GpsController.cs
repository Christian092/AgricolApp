using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoV1.Models;
using System.IO;
using System.Web.Script.Serialization;
using System.Device.Location;
using System.Drawing.Drawing2D;
//Proyecto final
namespace ProyectoV1.Controllers
{

    public class GpsController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();
        public ActionResult getMakers()
        {
            db.Configuration.ProxyCreationEnabled = false;
            int l, n;

            gpsinsideOutside gpsInOut = new gpsinsideOutside();

            List<vertice> polygon = db.vertice.ToList();
            List<gps> point = db.gps.ToList();
            List<gps> pointOutside = db.gps.ToList();

            List<gpsOutside> gpsOutside = new List<gpsOutside>();
            List<gpsInside> gpsInside = new List<gpsInside>();

            var testPoint = point[2];

            bool result = false;
            int j = polygon.Count() - 1;
            for (int c = 0; c < point.Count(); c++)//recorrido de GPS
            {
                for (int i = 0; i < polygon.Count(); i++)//recorrido por puntos del poligono
                {
                    //se comprueba si el GPS junto a sus coordenadas se encuentra dentro del poligono
                    if (polygon[i].longitud < point[c].longitud && polygon[j].longitud >= point[c].longitud || polygon[j].longitud < point[c].longitud && polygon[i].longitud >= point[c].longitud)
                    {
                        if (polygon[i].latitud + (point[c].longitud - polygon[i].longitud) / (polygon[j].longitud - polygon[i].longitud) * (polygon[j].latitud - polygon[i].latitud) < point[c].latitud)
                        {
                            result = !result;
                            var aqui = point[c];

                            var gpsIn = new gpsInside();
                            var animal_id = point[c].animal_id;
                            var animal1 = db.animal.Include(a => a.tipo).Include(a => a.raza).SingleOrDefault(a => a.id == animal_id);

                            gpsIn.latitud = point[c].latitud;
                            gpsIn.longitud = point[c].longitud;
                            gpsIn.animal_Id = animal1.id;
                            gpsIn.animal_Tipo = animal1.tipo.nombre;
                            gpsIn.animal_Raza = animal1.raza.nombre;
                            gpsIn.animal_Sexo = animal1.sexo;

                            gpsInside.Add(gpsIn);//Agregar GPS a lista de GPS dentro del poligono

                            pointOutside.Remove(point[c]);//eliminando el GPS de la lista de GPS fuera del Poligono
                        }

                    }
                    j = i;
                }
            }
            for (n = 0; n < pointOutside.Count; n++)
            {
                var gpsOut = new gpsOutside();
                var animal_id = pointOutside[n].animal_id;
                var animal2 = db.animal.Include(a => a.tipo).Include(a => a.raza).SingleOrDefault(a => a.id == animal_id);

                gpsOut.latitud = pointOutside[n].latitud;
                gpsOut.longitud = pointOutside[n].longitud;
                gpsOut.animal_Id = pointOutside[n].animal_id;
                gpsOut.animal_Tipo = animal2.tipo.nombre;
                gpsOut.animal_Raza = animal2.raza.nombre;
                gpsOut.animal_Sexo = animal2.sexo;

                gpsOutside.Add(gpsOut);
            }
            gpsInOut.inside = gpsInside;
            gpsInOut.outside = gpsOutside;


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(gpsInOut);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getMakersss()
        {
            db.Configuration.ProxyCreationEnabled = false;
            int l, n;

            gpsinsideOutside gpsInOut = new gpsinsideOutside();

            List<vertice> polygon = db.vertice.ToList();
            List<gps> point = db.gps.ToList();
            List<gps> pointOutside = db.gps.ToList();

            List<gpsOutside> gpsOutside = new List<gpsOutside>();
            List<gpsInside> gpsInside = new List<gpsInside>();

            //var testPoint = point[2];

            bool result = false;
            int j = polygon.Count() - 1;
            for (int c = 0; c < point.Count(); c++)//recorrido de GPS
            {
                for (int i = 0; i < polygon.Count(); i++)//recorrido por puntos del poligono
                {
                    //se comprueba si el GPS junto a sus coordenadas se encuentra dentro del poligono
                    if (polygon[i].latitud < point[c].latitud && polygon[j].latitud >= point[c].latitud || polygon[j].latitud < point[c].latitud && polygon[i].latitud >= point[c].latitud)
                    {
                        if (polygon[i].longitud + (point[c].latitud - polygon[i].latitud) / (polygon[j].latitud - polygon[i].latitud) * (polygon[j].longitud - polygon[i].longitud) < point[c].longitud)
                        {
                            result = !result;
                            var aqui = point[c];

                            var gpsIn = new gpsInside();
                            var animal_id = point[c].animal_id;
                            var animal1 = db.animal.Include(a => a.tipo).Include(a => a.raza).SingleOrDefault(a => a.id == animal_id);

                            gpsIn.latitud = point[c].latitud;
                            gpsIn.longitud = point[c].longitud;
                            gpsIn.animal_Id = animal1.id;
                            gpsIn.animal_Tipo = animal1.tipo.nombre;
                            gpsIn.animal_Raza = animal1.raza.nombre;
                            gpsIn.animal_Sexo = animal1.sexo;

                            gpsInside.Add(gpsIn);//Agregar GPS a lista de GPS dentro del poligono

                            pointOutside.Remove(point[c]);//eliminando el GPS de la lista de GPS fuera del Poligono
                        }

                    }
                    j = i;
                }
            }
            for (n = 0; n < pointOutside.Count; n++)
            {
                var gpsOut = new gpsOutside();
                var animal_id = pointOutside[n].animal_id;
                var animal2 = db.animal.Include(a => a.tipo).Include(a => a.raza).SingleOrDefault(a => a.id == animal_id);

                gpsOut.latitud = pointOutside[n].latitud;
                gpsOut.longitud = pointOutside[n].longitud;
                gpsOut.animal_Id = pointOutside[n].animal_id;
                gpsOut.animal_Tipo = animal2.tipo.nombre;
                gpsOut.animal_Raza = animal2.raza.nombre;
                gpsOut.animal_Sexo = animal2.sexo;

                gpsOutside.Add(gpsOut);
            }
            gpsInOut.inside = gpsInside;
            gpsInOut.outside = gpsOutside;


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(gpsInOut);
            return Json(json, JsonRequestBehavior.AllowGet);

        }

        public ActionResult getMakerss()
        {
            db.Configuration.ProxyCreationEnabled = false;
            int i, j, l, n;

            gpsinsideOutside gpsInOut = new gpsinsideOutside();

            List<vertice> poly = db.vertice.ToList();
            List<gps> point = db.gps.ToList();
            List<gps> pointOutside = db.gps.ToList();

            List<gpsOutside> gpsOutside = new List<gpsOutside>();
            List<gpsInside> gpsInside = new List<gpsInside>();

            for (l = 0; l < point.Count; l++)
            {
                for (i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
                {

                    if ((((poly[i].latitud <= point[l].latitud) && (point[l].latitud < poly[j].latitud))
                        || ((poly[j].latitud <= point[l].latitud) && (point[l].latitud < poly[i].latitud)))
                        && (point[l].longitud < (poly[j].longitud - poly[i].longitud) * (point[l].latitud - poly[i].latitud)
                            / (poly[j].latitud - poly[i].latitud) + poly[i].longitud))
                    {
                        var gpsIn = new gpsInside();
                        var animal_id = point[l].animal_id;
                        var animal1 = db.animal.Include(a => a.tipo).Include(a => a.raza).SingleOrDefault(a => a.id == animal_id);

                        gpsIn.latitud = point[l].latitud;
                        gpsIn.longitud = point[l].longitud;
                        gpsIn.animal_Id = animal1.id;
                        gpsIn.animal_Tipo = animal1.tipo.nombre;
                        gpsIn.animal_Raza = animal1.raza.nombre;
                        gpsIn.animal_Sexo = animal1.sexo;

                        gpsInside.Add(gpsIn);

                        pointOutside.Remove(point[l]);
                    }
                    else
                    { //c = false; 
                    }
                }
            }
            for (n = 0; n < pointOutside.Count; n++)
            {
                var gpsOut = new gpsOutside();
                var animal_id = pointOutside[n].animal_id;
                var animal2 = db.animal.Include(a => a.tipo).Include(a => a.raza).SingleOrDefault(a => a.id == animal_id);

                gpsOut.latitud = pointOutside[n].latitud;
                gpsOut.longitud = pointOutside[n].longitud;
                gpsOut.animal_Id = pointOutside[n].animal_id;
                gpsOut.animal_Tipo = animal2.tipo.nombre;
                gpsOut.animal_Raza = animal2.raza.nombre;
                gpsOut.animal_Sexo = animal2.sexo;

                gpsOutside.Add(gpsOut);
            }
            gpsInOut.inside = gpsInside;
            gpsInOut.outside = gpsOutside;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(gpsInOut);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult actualizarUbicacion(int? id, double lat, double lng)
        {
            if (id != null && lat != null && lng != null)
            {
                Double latitud = lat;
                Double longitud = lng;

                gps gpsmod = db.gps.Find(id);
                if (gpsmod != null)
                {
                    gpsmod.latitud = Math.Round(latitud, 5);
                    gpsmod.longitud = Math.Round(longitud, 6);
                    db.SaveChanges();
                }
            }

            return null;
        }

        public ActionResult IsPointInPolygon()
        {
            db.Configuration.ProxyCreationEnabled = false;
            int i, j, l, n;

            gpsinsideOutside gpsInOut = new gpsinsideOutside();

            List<vertice> poly = db.vertice.ToList();
            List<gps> point = db.gps.ToList();
            List<gps> pointOutside = db.gps.ToList();

            List<gpsOutside> gpsOutside = new List<gpsOutside>();
            List<gpsInside> gpsInside = new List<gpsInside>();

            for (l = 0; l < point.Count; l++)
            {
                for (i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
                {

                    if ((((poly[i].latitud <= point[l].latitud) && (point[l].latitud < poly[j].latitud))
                        || ((poly[j].latitud <= point[l].latitud) && (point[l].latitud < poly[i].latitud)))
                        && (point[l].longitud < (poly[j].longitud - poly[i].longitud) * (point[l].latitud - poly[i].latitud)
                            / (poly[j].latitud - poly[i].latitud) + poly[i].longitud))
                    {
                        var gpsIn = new gpsInside();
                        var animal_id = point[l].animal_id;
                        var animal1 = db.animal.Include(a => a.tipo).Include(a => a.raza).SingleOrDefault(a => a.id == animal_id);

                        gpsIn.latitud = point[l].latitud;
                        gpsIn.longitud = point[l].longitud;
                        gpsIn.animal_Id = animal1.id;
                        gpsIn.animal_Tipo = animal1.tipo.nombre;
                        gpsIn.animal_Raza = animal1.raza.nombre;
                        gpsIn.animal_Sexo = animal1.sexo;

                        gpsInside.Add(gpsIn);

                        pointOutside.Remove(point[l]);
                    }
                    else
                    { //c = false; 
                    }
                }
            }
            for (n = 0; n < pointOutside.Count; n++)
            {
                var gpsOut = new gpsOutside();
                var animal_id = pointOutside[n].animal_id;
                var animal2 = db.animal.Include(a => a.tipo).Include(a => a.raza).SingleOrDefault(a => a.id == animal_id);

                gpsOut.latitud = pointOutside[n].latitud;
                gpsOut.longitud = pointOutside[n].longitud;
                gpsOut.animal_Id = pointOutside[n].animal_id;
                gpsOut.animal_Tipo = animal2.tipo.nombre;
                gpsOut.animal_Raza = animal2.raza.nombre;
                gpsOut.animal_Sexo = animal2.sexo;

                gpsOutside.Add(gpsOut);
            }
            gpsInOut.inside = gpsInside;
            gpsInOut.outside = gpsOutside;


            double latA = -40.553298;
            double longA = -73.143679;
            double latB = -40.553673;
            double longB = -73.138422;

            var locA = new GeoCoordinate(latA, longA);
            var locB = new GeoCoordinate(latB, longB);
            double distance = locA.GetDistanceTo(locB);









            //    JavaScriptSerializer serializer = new JavaScriptSerializer();
            //var json = serializer.Serialize(gpsInOut);
            return Json(gpsInOut, JsonRequestBehavior.AllowGet);
        }
        public ActionResult IsPointInPolygonWeb()
        {
            db.Configuration.ProxyCreationEnabled = false;
            int i, j, l, n;

            gpsinsideOutside gpsInOut = new gpsinsideOutside();

            List<vertice> poly = db.vertice.ToList();
            List<gps> point = db.gps.ToList();
            List<gps> pointOutside = db.gps.ToList();

            List<gpsOutside> gpsOutside = new List<gpsOutside>();
            List<gpsInside> gpsInside = new List<gpsInside>();

            for (l = 0; l < point.Count; l++)
            {
                for (i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
                {

                    if ((((poly[i].latitud <= point[l].latitud) && (point[l].latitud < poly[j].latitud))
                        || ((poly[j].latitud <= point[l].latitud) && (point[l].latitud < poly[i].latitud)))
                        && (point[l].longitud < (poly[j].longitud - poly[i].longitud) * (point[l].latitud - poly[i].latitud)
                            / (poly[j].latitud - poly[i].latitud) + poly[i].longitud))
                    {
                        var gpsIn = new gpsInside();
                        var animal_id = point[l].animal_id;
                        var animal1 = db.animal.Include(a => a.tipo).Include(a => a.raza).SingleOrDefault(a => a.id == animal_id);

                        gpsIn.latitud = point[l].latitud;
                        gpsIn.longitud = point[l].longitud;
                        gpsIn.animal_Id = animal1.id;
                        gpsIn.animal_Tipo = animal1.tipo.nombre;
                        gpsIn.animal_Raza = animal1.raza.nombre;
                        gpsIn.animal_Sexo = animal1.sexo;

                        gpsInside.Add(gpsIn);

                        pointOutside.Remove(point[l]);
                    }
                    else
                    { //c = false; 
                    }
                }
            }
            for (n = 0; n < pointOutside.Count; n++)
            {
                var gpsOut = new gpsOutside();
                var animal_id = pointOutside[n].animal_id;
                var animal2 = db.animal.Include(a => a.tipo).Include(a => a.raza).SingleOrDefault(a => a.id == animal_id);

                gpsOut.latitud = pointOutside[n].latitud;
                gpsOut.longitud = pointOutside[n].longitud;
                gpsOut.animal_Id = pointOutside[n].animal_id;
                gpsOut.animal_Tipo = animal2.tipo.nombre;
                gpsOut.animal_Raza = animal2.raza.nombre;
                gpsOut.animal_Sexo = animal2.sexo;

                gpsOutside.Add(gpsOut);
            }
            gpsInOut.inside = gpsInside;
            gpsInOut.outside = gpsOutside;


            double latA = -40.553298;
            double longA = -73.143679;
            double latB = -40.553673;
            double longB = -73.138422;

            var locA = new GeoCoordinate(latA, longA);
            var locB = new GeoCoordinate(latB, longB);
            double distance = locA.GetDistanceTo(locB);









                JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(gpsInOut);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CrearTerreno()
        {
            ViewBag.poligono_id = new SelectList(db.poligono, "id", "id");
            return View();
        }

        public JsonResult ConsultaQR(string cod_sag)
        {
            animal animalBd = new animal();
            AnimalJsonRequest animalRequest = new AnimalJsonRequest();
            if (cod_sag != null)
            {
                animalBd = db.animal.Include(a => a.tipo).Include(a => a.raza).SingleOrDefault(a => a.codigo_sag == cod_sag);
                if (animalBd != null)
                {
                    animalRequest.id = animalBd.id;
                    animalRequest.codigo_sag = animalBd.codigo_sag;
                    animalRequest.tipo = animalBd.tipo.nombre;
                    animalRequest.raza = animalBd.raza.nombre;
                    animalRequest.sexo = animalBd.sexo;
                }
                else
                {
                    return null;
                }


                var today = DateTime.Today;
                DateTime nac_fec = Convert.ToDateTime(animalBd.fec_nac);
                var age = today.Year - nac_fec.Year;
                animalRequest.edad = age;

            }
            return Json(animalRequest, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearTerrenoJson()
        {
            var poligono = db.poligono.Where(p => p.id == 1).FirstOrDefault();
            var resolveRequest = HttpContext.Request;
            List<Punto> puntos = new List<Punto>();
            resolveRequest.InputStream.Seek(0, System.IO.SeekOrigin.Begin);
            string jsonString = new StreamReader(resolveRequest.InputStream).ReadToEnd();
            if (jsonString != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                puntos = (List<Punto>)serializer.Deserialize(jsonString, typeof(List<Punto>));
                for (var i = 0; i < puntos.Count(); i++)
                {
                    try
                    {
                        var vert = new vertice();
                        vert.latitud = puntos[i].latitud;
                        vert.longitud = puntos[i].longitud;
                        vert.poligono = poligono;
                        db.vertice.Add(vert);
                        db.SaveChanges();
                        
                    }
                    catch (Exception e)
                    {
                    }

                }
                
            }



            //var usuarioId = Convert.ToInt32(Session["userId"]);
            //var poligono = db.poligono.Where(pol => pol.usuario_id == usuarioId).FirstOrDefault();


            return Redirect("/Gps/UbicacionGanado");
        }

        public ActionResult GetTerrenoyGps()
        {
            db.Configuration.ProxyCreationEnabled = false;
            Terreno te = new Terreno();
            List<gps> misGPS = db.gps.ToList();
            List<vertice> vertices = db.vertice.ToList();
            te.Marcadores = misGPS;
            te.vertices = vertices;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(te);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTerreno()
        {
            List<vertice> vert = db.vertice.ToList();
            List<Vertic> vertices = new List<Vertic>();
            for (int i = 0; i < vert.Count(); i++)
            {
                Vertic ver = new Vertic();
                ver.id = vert[i].id;
                ver.latitud = vert[i].latitud;
                ver.longitud = vert[i].longitud;
                vertices.Add(ver);
            }

            return Json(vertices, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UbicacionGanado()
        {
            return View(db.gps.ToList());
        }





        // GET: Gps
        public ActionResult Index()
        {
            var gps = db.gps.Include(g => g.animal);
            return View(gps.ToList());
        }

        // GET: Gps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gps gps = db.gps.Find(id);
            if (gps == null)
            {
                return HttpNotFound();
            }
            return View(gps);
        }

        // GET: Gps/Create
        public ActionResult Create()
        {
            ViewBag.animal_id = new SelectList(db.animal.Where(a => a.gps.Count == 0 && a.estado.nombre != "MUERTO" && a.estado.nombre != "VENDIDO"), "id", "codigo_sag");
            return View();
        }

        // POST: Gps/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,latitud,longitud,imei,animal_id")] gps gps)
        {
            if (ModelState.IsValid)
            {
                db.gps.Add(gps);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.animal_id = new SelectList(db.animal, "id", "codigo_sag", gps.animal_id);
            return View(gps);
        }

        // GET: Gps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gps gps = db.gps.Find(id);
            if (gps == null)
            {
                return HttpNotFound();
            }
            ViewBag.animal_id = new SelectList(db.animal.Where(a => a.gps.Count == 0 && a.estado.nombre != "MUERTO" && a.estado.nombre != "VENDIDO"), "id", "codigo_sag", gps.animal_id);
            return View(gps);
        }

        // POST: Gps/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,latitud,longitud,imei,animal_id")] gps gps)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gps).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.animal_id = new SelectList(db.animal, "id", "codigo_sag", gps.animal_id);
            return View(gps);
        }

        // GET: Gps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gps gps = db.gps.Find(id);
            if (gps == null)
            {
                return HttpNotFound();
            }
            return View(gps);
        }

        // POST: Gps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gps gps = db.gps.Find(id);
            db.gps.Remove(gps);
            db.SaveChanges();
            return RedirectToAction("Index");
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
