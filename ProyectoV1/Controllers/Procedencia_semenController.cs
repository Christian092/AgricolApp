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
    public class Procedencia_semenController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Procedencia_semen
        public ActionResult Index()
        {
            return View(db.procedencia_semen.ToList());
        }

        // GET: Procedencia_semen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            procedencia_semen procedencia_semen = db.procedencia_semen.Find(id);
            if (procedencia_semen == null)
            {
                return HttpNotFound();
            }
            return View(procedencia_semen);
        }

        // GET: Procedencia_semen/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Procedencia_semen/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre")] procedencia_semen procedencia_semen)
        {
            if (ModelState.IsValid)
            {
                bool f = false;
                procedencia_semen.nombre = procedencia_semen.nombre.ToUpperInvariant();
                var procedencias_semen = db.procedencia_semen.Select(a => a.nombre);
                foreach (var a in procedencias_semen)
                {

                    if (a == procedencia_semen.nombre)
                    {
                        ViewBag.Error = "Procedencia de semen ya existe";
                        f = true;
                    }


                }
                if (f == true)
                {
                    return View(procedencia_semen);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        db.procedencia_semen.Add(procedencia_semen);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

            }

            return View(procedencia_semen);
        }

        // GET: Procedencia_semen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            procedencia_semen procedencia_semen = db.procedencia_semen.Find(id);
            if (procedencia_semen == null)
            {
                return HttpNotFound();
            }
            return View(procedencia_semen);
        }

        // POST: Procedencia_semen/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre")] procedencia_semen procedencia_semen)
        {
            bool f = false;
            procedencia_semen.nombre = procedencia_semen.nombre.ToUpperInvariant();
            var procedencias_semen = db.procedencia_semen.Select(a => a.nombre);
            foreach (var a in procedencias_semen)
            {

                if (a == procedencia_semen.nombre)
                {
                    ViewBag.Error = "Procedencia de semen ya existe";
                    f = true;
                }


            }
            if (f == true)
            {
                return View(procedencia_semen);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(procedencia_semen).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(procedencia_semen);

        }

        // GET: Procedencia_semen/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            procedencia_semen procedencia_semen = db.procedencia_semen.Find(id);
            if (procedencia_semen == null)
            {
                return HttpNotFound();
            }
            return View(procedencia_semen);
        }

        // POST: Procedencia_semen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            procedencia_semen procedencia_semen = db.procedencia_semen.Find(id);
            try
            {
                db.procedencia_semen.Remove(procedencia_semen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(procedencia_semen);

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