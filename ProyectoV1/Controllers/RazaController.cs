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
    public class RazaController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Raza
        public ActionResult Index()
        {
            var raza = db.raza.Include(r => r.procedencia);
            return View(raza.ToList());
        }

        // GET: Raza/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            raza raza = db.raza.Find(id);
            if (raza == null)
            {
                return HttpNotFound();
            }
            return View(raza);
        }

        // GET: Raza/Create
        public ActionResult Create()
        {
            ViewBag.procedencia_id = new SelectList(db.procedencia, "id", "nombre");
            return View();
        }

        // POST: Raza/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,procedencia_id")] raza raza)
        {
            if (ModelState.IsValid)
            {
                bool f = false;
                raza.nombre = raza.nombre.ToUpperInvariant();
                var razas = db.raza.Select(a => a.nombre);
                foreach (var a in razas)
                {

                    if (a == raza.nombre)
                    {
                        ViewBag.Error = "Raza ya existe";
                        f = true;
                    }


                }
                if (f == true)
                {
                    return View(raza);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        db.raza.Add(raza);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }


            ViewBag.procedencia_id = new SelectList(db.procedencia, "id", "nombre", raza.procedencia_id);
            return View(raza);

        }

        // GET: Raza/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            raza raza = db.raza.Find(id);
            if (raza == null)
            {
                return HttpNotFound();
            }
            ViewBag.procedencia_id = new SelectList(db.procedencia, "id", "nombre", raza.procedencia_id);
            return View(raza);
        }

        // POST: Raza/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,procedencia_id")] raza raza)
        {
            bool f = false;
            raza.nombre = raza.nombre.ToUpperInvariant();
            var razas = db.raza.Select(a => a.nombre);
            foreach (var a in razas)
            {

                if (a == raza.nombre)
                {
                    ViewBag.Error = "Raza ya existe";
                    f = true;
                }


            }
            if (f == true)
            {
                return View(raza);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(raza).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.procedencia_id = new SelectList(db.procedencia, "id", "nombre", raza.procedencia_id);
            return View(raza);
        }

        // GET: Raza/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            raza raza = db.raza.Find(id);
            if (raza == null)
            {
                return HttpNotFound();
            }
            return View(raza);
        }

        // POST: Raza/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            raza raza = db.raza.Find(id);
            try
            {
                db.raza.Remove(raza);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(raza);

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