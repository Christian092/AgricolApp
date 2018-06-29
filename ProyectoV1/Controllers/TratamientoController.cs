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
    public class TratamientoController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Tratamiento
        public ActionResult Index(int? page)
        {
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {

                DateTime mañana = DateTime.Today.AddDays(1);
                int alertaHoy = db.tratamiento.Where(m => m.fecha.Equals(mañana)).Count();
                if (alertaHoy == 1)
                {
                    TempData["notice"] = "Recuerda Que este Proximo Dia Tienes 1 Tratamiento";
                }
                else if (alertaHoy == 2)
                {
                    TempData["notice"] = "Recuerda Que este Proximo Dia Tienes 2 Tratamientos";
                }
                else if (alertaHoy >= 3)
                {
                    TempData["notice"] = "Recuerda Que este Proximo Dia Tienes Varios Tratamientos";
                }

            }
            var tratamiento = db.tratamiento.Include(t => t.tratador).Include(t => t.tipo_tratamiento);
            return View(tratamiento.ToList().ToPagedList(page ?? 1, 5));
        }
        [HttpPost]
        public ActionResult Index(int? page, DateTime fecha1)
        {
            if (fecha1 == null)
            {
                ViewBag.Error = "Ingrese una fecha";
                var trat = db.tratamiento;
                return View(trat.ToList().ToPagedList(page ?? 1, 5));

            }
            else
            {
                using (bdagricolaEntities dc = new bdagricolaEntities())
                {

                    var trat = db.tratamiento.Where(m => m.fecha == (fecha1));
                    return View(trat.ToList().ToPagedList(page ?? 1, 5));
                }
            }



        }
        public void GetExcel()
        {
            List<tratamiento> tratamiento = new List<tratamiento>();
            List<tratador> tratador = new List<tratador>();
            List<tipo_tratamiento> tipo_tratamiento = new List<tipo_tratamiento>();
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                tratamiento = dc.tratamiento.ToList();
                tratador = dc.tratador.ToList();
                tipo_tratamiento = dc.tipo_tratamiento.ToList();

            }

            WebGrid grid = new WebGrid(source: tratamiento, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
                columns: grid.Columns(
                        grid.Column("id", "ID"),
                        grid.Column("fecha", "Fecha"),
                        grid.Column("tratador.nombre", "tratador"),
                        grid.Column("tipo_tratamiento.nombre", "Tipo Tratamiento")
                        )
                    ).ToString();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Tratamientos.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();
        }
        // GET: Tratamiento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tratamiento tratamiento = db.tratamiento.Find(id);
            if (tratamiento == null)
            {
                return HttpNotFound();
            }
            return View(tratamiento);
        }

        // GET: Tratamiento/Create
        public ActionResult Create()
        {
           
            ViewBag.tratador_id = new SelectList(db.tratador, "id", "nombre");
            ViewBag.tipo_tratamiento_id = new SelectList(db.tipo_tratamiento, "id", "nombre");
            return View();
        }

        // POST: Tratamiento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fecha,tratador_id,tipo_tratamiento_id")] tratamiento tratamiento)
        {
            if (ModelState.IsValid)
            {
                db.tratamiento.Add(tratamiento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            
            ViewBag.tratador_id = new SelectList(db.tratador, "id", "nombre", tratamiento.tratador_id);
            ViewBag.tipo_tratamiento_id = new SelectList(db.tipo_tratamiento, "id", "nombre", tratamiento.tipo_tratamiento_id);
            return View(tratamiento);
        }

        // GET: Tratamiento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tratamiento tratamiento = db.tratamiento.Find(id);
            if (tratamiento == null)
            {
                return HttpNotFound();
            }
            ViewBag.tratador_id = new SelectList(db.tratador, "id", "nombre", tratamiento.tratador_id);
            ViewBag.tipo_tratamiento_id = new SelectList(db.tipo_tratamiento, "id", "nombre", tratamiento.tipo_tratamiento_id);
            return View(tratamiento);
        }

        // POST: Tratamiento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fecha,alerta_id,tratador_id")] tratamiento tratamiento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tratamiento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tratador_id = new SelectList(db.tratador, "id", "nombre", tratamiento.tratador_id);
            ViewBag.tipo_tratamiento_id = new SelectList(db.tipo_tratamiento, "id", "nombre", tratamiento.tipo_tratamiento_id);
            return View(tratamiento);
        }

        // GET: Tratamiento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tratamiento tratamiento = db.tratamiento.Find(id);
            if (tratamiento == null)
            {
                return HttpNotFound();
            }
            return View(tratamiento);
        }

        // POST: Tratamiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tratamiento tratamiento = db.tratamiento.Find(id);
            try
            {
                db.tratamiento.Remove(tratamiento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(tratamiento);
           
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
