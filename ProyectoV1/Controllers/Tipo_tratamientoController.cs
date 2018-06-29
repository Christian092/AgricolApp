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
    public class Tipo_tratamientoController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Tipo_tratamiento
        public ActionResult Index(int? page)
        {
            return View(db.tipo_tratamiento.ToList().ToPagedList(page ?? 1, 5));
        }

        // GET: Tipo_tratamiento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_tratamiento tipo_tratamiento = db.tipo_tratamiento.Find(id);
            if (tipo_tratamiento == null)
            {
                return HttpNotFound();
            }
            return View(tipo_tratamiento);
        }

        // GET: Tipo_tratamiento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tipo_tratamiento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre")] tipo_tratamiento tipo_tratamiento)
        {
            bool f = false;
            tipo_tratamiento.nombre = tipo_tratamiento.nombre.ToUpperInvariant();
            var tipo_tratamientos = db.tipo_tratamiento.Select(a => a.nombre);
            foreach (var a in tipo_tratamientos)
            {

                if (a == tipo_tratamiento.nombre)
                {
                    ViewBag.Error = "Tipo de tratamiento ya existe";
                    f = true;
                }


            }
            if (f == true)
            {
                return View(tipo_tratamiento);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.tipo_tratamiento.Add(tipo_tratamiento);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(tipo_tratamiento);
        }

        // GET: Tipo_tratamiento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_tratamiento tipo_tratamiento = db.tipo_tratamiento.Find(id);
            if (tipo_tratamiento == null)
            {
                return HttpNotFound();
            }
            return View(tipo_tratamiento);
        }

        // POST: Tipo_tratamiento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre")] tipo_tratamiento tipo_tratamiento)
        {
            bool f = false;
            tipo_tratamiento.nombre = tipo_tratamiento.nombre.ToUpperInvariant();
            var tipo_tratamientos = db.tipo_tratamiento.Select(a => a.nombre);
            foreach (var a in tipo_tratamientos)
            {

                if (a == tipo_tratamiento.nombre)
                {
                    ViewBag.Error = "Tipo de tratamiento ya existe";
                    f = true;
                }


            }
            if (f == true)
            {
                return View(tipo_tratamiento);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tipo_tratamiento).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(tipo_tratamiento);
        }

        // GET: Tipo_tratamiento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_tratamiento tipo_tratamiento = db.tipo_tratamiento.Find(id);
            if (tipo_tratamiento == null)
            {
                return HttpNotFound();
            }
            return View(tipo_tratamiento);
        }

        // POST: Tipo_tratamiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tipo_tratamiento tipo_tratamiento = db.tipo_tratamiento.Find(id);
            try
            {
                db.tipo_tratamiento.Remove(tipo_tratamiento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(tipo_tratamiento);
            
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
