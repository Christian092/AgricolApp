using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoV1.Models;
using PagedList;
using System.Web.Helpers;

namespace ProyectoV1.Controllers
{
    public class Alerta_mantencionController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Alerta_mantencion
        public ActionResult Index(int? page)
        {
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {

                DateTime mañana = DateTime.Today.AddDays(1);
                int alertaHoy = db.alerta_mantencion.Where(m => m.fecha.Equals(mañana)).Count();
                if (alertaHoy == 1)
                {
                    TempData["notice"] = "Recuerda Que este Proximo Dia Tienes 1 Mantencion";
                }
                else if (alertaHoy == 2)
                {
                    TempData["notice"] = "Recuerda Que este Proximo Dia Tienes 2 Mantenciones";
                }
                else if (alertaHoy >= 3)
                {
                    TempData["notice"] = "Recuerda Que este Proximo Dia Tienes Varias Mantenciones";
                }

            }
            var alerta_mantencion = db.alerta_mantencion.Include(a => a.lecheria);
            return View(alerta_mantencion.ToList().ToPagedList(page ?? 1, 10));
        }
        [HttpPost]
        public ActionResult Index(int? page, DateTime? fecha1)
        {
            if (fecha1 == null)
            {
                ViewBag.Error = "Ingrese una fecha";
                var alertas = db.alerta_mantencion;
                return View(alertas.ToList().ToPagedList(page ?? 1, 5));

            }
            else
            {
                using (bdagricolaEntities dc = new bdagricolaEntities())
                {

                    var alertas = db.alerta_mantencion.Where(m => m.fecha == (fecha1));
                    return View(alertas.ToList().ToPagedList(page ?? 1, 5));
                }
            }
               

      
        }
        // GET: Alerta_mantencion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            alerta_mantencion alerta_mantencion = db.alerta_mantencion.Find(id);
            if (alerta_mantencion == null)
            {
                return HttpNotFound();
            }
            return View(alerta_mantencion);
        }

        // GET: Alerta_mantencion/Create
        public ActionResult Create()
        {
            ViewBag.lecheria_id = new SelectList(db.lecheria, "id", "id");
            return View();
        }

        // POST: Alerta_mantencion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fecha,lecheria_id")] alerta_mantencion alerta_mantencion)
        {
            if (ModelState.IsValid)
            {
                db.alerta_mantencion.Add(alerta_mantencion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lecheria_id = new SelectList(db.lecheria, "id", "id", alerta_mantencion.lecheria_id);
            return View(alerta_mantencion);
        }

        // GET: Alerta_mantencion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            alerta_mantencion alerta_mantencion = db.alerta_mantencion.Find(id);
            if (alerta_mantencion == null)
            {
                return HttpNotFound();
            }
            ViewBag.lecheria_id = new SelectList(db.lecheria, "id", "id", alerta_mantencion.lecheria_id);
            return View(alerta_mantencion);
        }

        // POST: Alerta_mantencion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fecha,lecheria_id")] alerta_mantencion alerta_mantencion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alerta_mantencion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lecheria_id = new SelectList(db.lecheria, "id", "id", alerta_mantencion.lecheria_id);
            return View(alerta_mantencion);
        }

        // GET: Alerta_mantencion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            alerta_mantencion alerta_mantencion = db.alerta_mantencion.Find(id);
            if (alerta_mantencion == null)
            {
                return HttpNotFound();
            }
            return View(alerta_mantencion);
        }
        public void GetExcel()
        {
            List<alerta_mantencion> alerta_mantencion = new List<alerta_mantencion>();
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                alerta_mantencion = dc.alerta_mantencion.ToList();
            }

            WebGrid grid = new WebGrid(source: alerta_mantencion, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
                columns: grid.Columns(
                        grid.Column("id", "ID"),
                        grid.Column("fecha", "Fecha"),
                        grid.Column("lecheria_id", "Lecheria")
                        )
                    ).ToString();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=MantencionesLecheria.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();
        }

        // POST: Alerta_mantencion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            alerta_mantencion alerta_mantencion = db.alerta_mantencion.Find(id);
            db.alerta_mantencion.Remove(alerta_mantencion);
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
