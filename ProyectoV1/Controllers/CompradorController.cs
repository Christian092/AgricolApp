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
    public class CompradorController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Comprador
        public ActionResult Index(int? page)
        {
            return View(db.comprador.ToList().ToPagedList(page ?? 1, 5));
        }

        // GET: Comprador/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comprador comprador = db.comprador.Find(id);
            if (comprador == null)
            {
                return HttpNotFound();
            }
            return View(comprador);
        }

        // GET: Comprador/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comprador/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,apellido,telefono")] comprador comprador)
        {
            if (ModelState.IsValid)
            {
                db.comprador.Add(comprador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(comprador);
        }

        // GET: Comprador/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comprador comprador = db.comprador.Find(id);
            if (comprador == null)
            {
                return HttpNotFound();
            }
            return View(comprador);
        }

        // POST: Comprador/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,apellido,telefono")] comprador comprador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comprador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comprador);
        }

        // GET: Comprador/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comprador comprador = db.comprador.Find(id);
            if (comprador == null)
            {
                return HttpNotFound();
            }
            return View(comprador);
        }

        // POST: Comprador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            comprador comprador = db.comprador.Find(id);
            try
            {
                db.comprador.Remove(comprador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(comprador);

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