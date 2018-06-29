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
    public class FabricanteController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Fabricante
        public ActionResult Index()
        {
            return View(db.fabricante.ToList());
        }

        // GET: Fabricante/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            fabricante fabricante = db.fabricante.Find(id);
            if (fabricante == null)
            {
                return HttpNotFound();
            }
            return View(fabricante);
        }

        // GET: Fabricante/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fabricante/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre")] fabricante fabricante)
        {
            if (ModelState.IsValid)
            {
                bool f = false;
                fabricante.nombre = fabricante.nombre.ToUpperInvariant();
                var fabricantes = db.fabricante.Select(a => a.nombre);
                foreach (var a in fabricantes)
                {

                    if (a == fabricante.nombre)
                    {
                        ViewBag.Error = "Fabricante ya Existe";
                        f = true;
                    }


                }
                if (f == true)
                {
                    return View(fabricante);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        db.fabricante.Add(fabricante);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

            }

            return View(fabricante);
        }

        // GET: Fabricante/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            fabricante fabricante = db.fabricante.Find(id);
            if (fabricante == null)
            {
                return HttpNotFound();
            }
            return View(fabricante);
        }

        // POST: Fabricante/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre")] fabricante fabricante)
        {
            bool f = false;
            fabricante.nombre = fabricante.nombre.ToUpperInvariant();
            var fabricantes = db.fabricante.Select(a => a.nombre);
            foreach (var a in fabricantes)
            {

                if (a == fabricante.nombre)
                {
                    ViewBag.Error = "Fabricante ya Existe";
                    f = true;
                }


            }
            if (f == true)
            {
                return View(fabricante);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(fabricante).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(fabricante);
        }

        // GET: Fabricante/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            fabricante fabricante = db.fabricante.Find(id);
            if (fabricante == null)
            {
                return HttpNotFound();
            }
            return View(fabricante);
        }

        // POST: Fabricante/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            fabricante fabricante = db.fabricante.Find(id);
            try
            {
                db.fabricante.Remove(fabricante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(fabricante);
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