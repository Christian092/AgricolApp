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
    public class VerticeController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Vertice
        public ActionResult Index()
        {
            var vertice = db.vertice.Include(v => v.poligono);
            return View(vertice.ToList());
        }

        // GET: Vertice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vertice vertice = db.vertice.Find(id);
            if (vertice == null)
            {
                return HttpNotFound();
            }
            return View(vertice);
        }

        // GET: Vertice/Create
        public ActionResult Create()
        {
            ViewBag.poligono_id = new SelectList(db.poligono, "id", "id");
            return View();
        }

        // POST: Vertice/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,latitud,longitud,poligono_id")] vertice vertice)
        {
            if (ModelState.IsValid)
            {
                db.vertice.Add(vertice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.poligono_id = new SelectList(db.poligono, "id", "id", vertice.poligono_id);
            return View(vertice);
        }

        // GET: Vertice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vertice vertice = db.vertice.Find(id);
            if (vertice == null)
            {
                return HttpNotFound();
            }
            ViewBag.poligono_id = new SelectList(db.poligono, "id", "id", vertice.poligono_id);
            return View(vertice);
        }

        // POST: Vertice/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,latitud,longitud,poligono_id")] vertice vertice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vertice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.poligono_id = new SelectList(db.poligono, "id", "id", vertice.poligono_id);
            return View(vertice);
        }

        // GET: Vertice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vertice vertice = db.vertice.Find(id);
            if (vertice == null)
            {
                return HttpNotFound();
            }
            return View(vertice);
        }

        // POST: Vertice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            vertice vertice = db.vertice.Find(id);
            db.vertice.Remove(vertice);
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
