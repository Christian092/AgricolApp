using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoV1.Models;
using System.Web.Helpers;
using PagedList;
using PagedList.Mvc;
namespace ProyectoV1.Controllers
{
    public class OrdenaController : Controller
    {
        public JsonResult fechaValida(DateTime fecha)
        {
            DateTime fechaActual = DateTime.Today;
            bool respuesta = false;
            if (fecha > fechaActual)
            {
                respuesta = false;
            }
            else
            {
                respuesta = true;
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        private bdagricolaEntities db = new bdagricolaEntities();
        List<ordena> listado = new List<ordena>();
        // GET: Ordena
        public ActionResult Index(int? page)
        {
            var ordena = db.ordena.Include(o => o.lecheria);
            return View(ordena.ToList().ToPagedList(page ?? 1, 5));
        }

        [HttpPost]
        public ActionResult Index(int? page, DateTime? fecha1, DateTime? fecha2)
        {
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {

                if (fecha1 > fecha2)
                {

                    ViewBag.Error = "Primera Fecha no debe ser superior a la segunda";
                    var ordena = db.ordena.Include(o => o.lecheria);
                    return View(ordena.ToList().ToPagedList(page ?? 1, 5));
                }
               
                if (fecha1 == null || fecha2 == null)
                {
                    ViewBag.Error = "Debe rellenar ambos campos para buscar por fecha";
                    var ordenas1 = db.ordena.Include(o => o.lecheria);
                    return View(ordenas1.ToList().ToPagedList(page ?? 1, 5));
                }
                else
                {

                    var ordenas2 = db.ordena.Include(o => o.lecheria).Where(m => m.fecha >= (fecha1) && m.fecha <= fecha2);
                    int contador = ordenas2.Count();
                    if(contador == 0)
                    {
                        ViewBag.Error = "No existen datos entre esos rangos";
                        var ordena = db.ordena.Include(o => o.lecheria);
                        return View(ordena.ToList().ToPagedList(page ?? 1, 5));
                    }
                    else
                    {
                        return View(ordenas2.ToList().ToPagedList(page ?? 1, 5));
                    }
                    
                }
               
                
            }

            //using (bdagricolaEntities dc = new bdagricolaEntities())
            //{

            //    var ordenas = db.ordena.Where(m => m.fecha >= (fecha1) && m.fecha <= fecha2);
            //    return View(ordenas.ToList().ToPagedList(page ?? 1, 5));
            //}

        }



        // GET: Ordena/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordena ordena = db.ordena.Find(id);
            if (ordena == null)
            {
                return HttpNotFound();
            }
            return View(ordena);
        }

        // GET: Ordena/Create
        public ActionResult Create()
        {
            ViewBag.lecheria_id = new SelectList(db.lecheria, "id", "id");
            return View();
        }

        // POST: Ordena/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fecha,litros,lecheria_id,jornada")] ordena ordena)
        {
            if (ModelState.IsValid)
            {
                db.ordena.Add(ordena);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lecheria_id = new SelectList(db.lecheria, "id", "id", ordena.lecheria_id);
            return View(ordena);
        }

        // GET: Ordena/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordena ordena = db.ordena.Find(id);
            if (ordena == null)
            {
                return HttpNotFound();
            }
            ViewBag.lecheria_id = new SelectList(db.lecheria, "id", "id", ordena.lecheria_id);
            return View(ordena);
        }

        // POST: Ordena/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fecha,litros,lecheria_id,jornada")] ordena ordena)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordena).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lecheria_id = new SelectList(db.lecheria, "id", "id", ordena.lecheria_id);
            return View(ordena);
        }

        // GET: Ordena/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordena ordena = db.ordena.Find(id);
            if (ordena == null)
            {
                return HttpNotFound();
            }
            return View(ordena);
        }

        // POST: Ordena/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ordena ordena = db.ordena.Find(id);
            try
            {
                db.ordena.Remove(ordena);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(ordena);
            
            
        }
        //public ActionResult BuscarFecha(DateTime Fecha_Ordena)
        //{
        //    var fecha = from s in db.ordena select s;
        //    if (!DateTime(Fecha_Ordena))
        //}
        public void GetExcel()
        {
            List<ordena> ordena = new List<ordena>();
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                ordena = dc.ordena.ToList();
            }

            WebGrid grid = new WebGrid(source: ordena, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
                columns: grid.Columns(
                        grid.Column("id", "ID"),
                        grid.Column("fecha", "Fecha"),
                        grid.Column("litros", "Litros"),
                        grid.Column("lecheria_id", "Lecheria"),
                        grid.Column("jornada", "Jornada")
                        )
                    ).ToString();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Ordeñas.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();
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
