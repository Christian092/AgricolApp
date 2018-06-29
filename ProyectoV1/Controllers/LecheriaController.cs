using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoV1.Models;

namespace ProyectoV1.Controllers
{
    public class LecheriaController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Lecheria
        public ActionResult Index()
        {
            var lecheria = db.lecheria.Include(l => l.fabricante).Include(l => l.tipo_lecheria);
            return View(lecheria.ToList());
        }

        // GET: Lecheria/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lecheria lecheria = db.lecheria.Find(id);
            if (lecheria == null)
            {
                return HttpNotFound();
            }
            return View(lecheria);
        }

        // GET: Lecheria/Create
        public ActionResult Create()
        {
            ViewBag.fabricante_id = new SelectList(db.fabricante, "id", "nombre");
            ViewBag.tipo_lecheria_id = new SelectList(db.tipo_lecheria, "id", "nombre");
            return View();
        }

        // POST: Lecheria/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,capacidad,ordenas_diarias,cantidad_trabajadores,tipo_lecheria_id,fabricante_id")] lecheria lecheria)
        {
            if (ModelState.IsValid)
            {
                db.lecheria.Add(lecheria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.fabricante_id = new SelectList(db.fabricante, "id", "nombre", lecheria.fabricante_id);
            ViewBag.tipo_lecheria_id = new SelectList(db.tipo_lecheria, "id", "nombre", lecheria.tipo_lecheria_id);
            return View(lecheria);
        }

        // GET: Lecheria/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lecheria lecheria = db.lecheria.Find(id);
            if (lecheria == null)
            {
                return HttpNotFound();
            }
            ViewBag.fabricante_id = new SelectList(db.fabricante, "id", "nombre", lecheria.fabricante_id);
            ViewBag.tipo_lecheria_id = new SelectList(db.tipo_lecheria, "id", "nombre", lecheria.tipo_lecheria_id);
            return View(lecheria);
        }

        // POST: Lecheria/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,capacidad,ordenas_diarias,cantidad_trabajadores,tipo_lecheria_id,fabricante_id")] lecheria lecheria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lecheria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.fabricante_id = new SelectList(db.fabricante, "id", "nombre", lecheria.fabricante_id);
            ViewBag.tipo_lecheria_id = new SelectList(db.tipo_lecheria, "id", "nombre", lecheria.tipo_lecheria_id);
            return View(lecheria);
        }

        // GET: Lecheria/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lecheria lecheria = db.lecheria.Find(id);
            if (lecheria == null)
            {
                return HttpNotFound();
            }
            return View(lecheria);
        }

        // POST: Lecheria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            lecheria lecheria = db.lecheria.Find(id);
            try
            {
                db.lecheria.Remove(lecheria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(lecheria);
            
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
