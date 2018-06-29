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

namespace ProyectoV1.Controllers
{
    public class Nace_muertoController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Nace_muerto
        public ActionResult Index(int? page)
        {
            var nace_muerto = db.nace_muerto.Include(n => n.parto).Include(n => n.raza).Include(n => n.tipo);
            return View(nace_muerto.ToList().ToPagedList(page ?? 1, 5));
        }

        // GET: Nace_muerto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            nace_muerto nace_muerto = db.nace_muerto.Find(id);
            if (nace_muerto == null)
            {
                return HttpNotFound();
            }
            return View(nace_muerto);
        }

        // GET: Nace_muerto/Create
        public ActionResult Create()
        {
            ViewBag.parto_id = new SelectList(db.parto, "id", "id");
            ViewBag.raza_id = new SelectList(db.raza, "id", "nombre");
            ViewBag.tipo_id = new SelectList(db.tipo, "id", "nombre");
            return View();
        }

        // POST: Nace_muerto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,parto_id,raza_id,tipo_id")] nace_muerto nace_muerto)
        {
            if (ModelState.IsValid)
            {
                db.nace_muerto.Add(nace_muerto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.parto_id = new SelectList(db.parto, "id", "id", nace_muerto.parto_id);
            ViewBag.raza_id = new SelectList(db.raza, "id", "nombre", nace_muerto.raza_id);
            ViewBag.tipo_id = new SelectList(db.tipo, "id", "nombre", nace_muerto.tipo_id);
            return View(nace_muerto);
        }

        // GET: Nace_muerto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            nace_muerto nace_muerto = db.nace_muerto.Find(id);
            if (nace_muerto == null)
            {
                return HttpNotFound();
            }
            ViewBag.parto_id = new SelectList(db.parto, "id", "id", nace_muerto.parto_id);
            ViewBag.raza_id = new SelectList(db.raza, "id", "nombre", nace_muerto.raza_id);
            ViewBag.tipo_id = new SelectList(db.tipo, "id", "nombre", nace_muerto.tipo_id);
            return View(nace_muerto);
        }

        // POST: Nace_muerto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,parto_id,raza_id,tipo_id")] nace_muerto nace_muerto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nace_muerto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.parto_id = new SelectList(db.parto, "id", "id", nace_muerto.parto_id);
            ViewBag.raza_id = new SelectList(db.raza, "id", "nombre", nace_muerto.raza_id);
            ViewBag.tipo_id = new SelectList(db.tipo, "id", "nombre", nace_muerto.tipo_id);
            return View(nace_muerto);
        }

        // GET: Nace_muerto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            nace_muerto nace_muerto = db.nace_muerto.Find(id);
            if (nace_muerto == null)
            {
                return HttpNotFound();
            }
            return View(nace_muerto);
        }

        // POST: Nace_muerto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            nace_muerto nace_muerto = db.nace_muerto.Find(id);
            db.nace_muerto.Remove(nace_muerto);
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
