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
    public class PoligonosController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Poligonos
        public ActionResult Index()
        {
            var poligono = db.poligono.Include(p => p.usuario);
            return View(poligono.ToList());
        }

        // GET: Poligonos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            poligono poligono = db.poligono.Find(id);
            if (poligono == null)
            {
                return HttpNotFound();
            }
            return View(poligono);
        }

        // GET: Poligonos/Create
        public ActionResult Create()
        {
            ViewBag.usuario_id = new SelectList(db.usuario, "id", "rut");
            return View();
        }

        // POST: Poligonos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,usuario_id")] poligono poligono)
        {
            if (ModelState.IsValid)
            {
                db.poligono.Add(poligono);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.usuario_id = new SelectList(db.usuario, "id", "rut", poligono.usuario_id);
            return View(poligono);
        }

        // GET: Poligonos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            poligono poligono = db.poligono.Find(id);
            if (poligono == null)
            {
                return HttpNotFound();
            }
            ViewBag.usuario_id = new SelectList(db.usuario, "id", "rut", poligono.usuario_id);
            return View(poligono);
        }

        // POST: Poligonos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,usuario_id")] poligono poligono)
        {
            if (ModelState.IsValid)
            {
                db.Entry(poligono).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.usuario_id = new SelectList(db.usuario, "id", "rut", poligono.usuario_id);
            return View(poligono);
        }

        // GET: Poligonos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            poligono poligono = db.poligono.Find(id);
            if (poligono == null)
            {
                return HttpNotFound();
            }
            return View(poligono);
        }

        // POST: Poligonos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            poligono poligono = db.poligono.Find(id);
            db.poligono.Remove(poligono);
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
