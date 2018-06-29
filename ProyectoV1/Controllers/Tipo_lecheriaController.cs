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
    public class Tipo_lecheriaController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Tipo_lecheria
        public ActionResult Index()
        {
            return View(db.tipo_lecheria.ToList());
        }

        // GET: Tipo_lecheria/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_lecheria tipo_lecheria = db.tipo_lecheria.Find(id);
            if (tipo_lecheria == null)
            {
                return HttpNotFound();
            }
            return View(tipo_lecheria);
        }

        // GET: Tipo_lecheria/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tipo_lecheria/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre")] tipo_lecheria tipo_lecheria)
        {
            if (ModelState.IsValid)
            {
                bool f = false;
                tipo_lecheria.nombre = tipo_lecheria.nombre.ToUpperInvariant();
                var tipo_lecherias = db.tipo_lecheria.Select(a => a.nombre);
                foreach (var a in tipo_lecherias)
                {

                    if (a == tipo_lecheria.nombre)
                    {
                        ViewBag.Error = "Tipo de lecheria ya existe";
                        f = true;
                    }


                }
                if (f == true)
                {
                    return View(tipo_lecheria);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        db.tipo_lecheria.Add(tipo_lecheria);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }


            return View(tipo_lecheria);
        }


        // GET: Tipo_lecheria/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_lecheria tipo_lecheria = db.tipo_lecheria.Find(id);
            if (tipo_lecheria == null)
            {
                return HttpNotFound();
            }
            return View(tipo_lecheria);
        }

        // POST: Tipo_lecheria/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre")] tipo_lecheria tipo_lecheria)
        {
            bool f = false;
            tipo_lecheria.nombre = tipo_lecheria.nombre.ToUpperInvariant();
            var tipo_lecherias = db.tipo_lecheria.Select(a => a.nombre);
            foreach (var a in tipo_lecherias)
            {

                if (a == tipo_lecheria.nombre)
                {
                    ViewBag.Error = "Tipo de lecheria ya existe";
                    f = true;
                }


            }
            if (f == true)
            {
                return View(tipo_lecheria);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tipo_lecheria).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(tipo_lecheria);
        }

        // GET: Tipo_lecheria/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_lecheria tipo_lecheria = db.tipo_lecheria.Find(id);
            if (tipo_lecheria == null)
            {
                return HttpNotFound();
            }
            return View(tipo_lecheria);
        }

        // POST: Tipo_lecheria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tipo_lecheria tipo_lecheria = db.tipo_lecheria.Find(id);
            try
            {
                db.tipo_lecheria.Remove(tipo_lecheria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(tipo_lecheria);


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