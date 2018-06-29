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
    public class ProcedenciaController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Procedencia
        public ActionResult Index()
        {
            return View(db.procedencia.ToList());
        }

        // GET: Procedencia/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            procedencia procedencia = db.procedencia.Find(id);
            if (procedencia == null)
            {
                return HttpNotFound();
            }
            return View(procedencia);
        }

        // GET: Procedencia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Procedencia/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre")] procedencia procedencia)
        {
            if (ModelState.IsValid)
            {
                bool f = false;
                procedencia.nombre = procedencia.nombre.ToUpper();
                var procedencias = db.procedencia.Select(a => a.nombre);
                foreach (var a in procedencias)
                {

                    if (a == procedencia.nombre)
                    {
                        ViewBag.Error = "Procedencia ya Existe";
                        f = true;
                    }


                }
                if (f == true)
                {
                    return View(procedencia);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        db.procedencia.Add(procedencia);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }


            }

            return View(procedencia);
        }

        // GET: Procedencia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            procedencia procedencia = db.procedencia.Find(id);
            if (procedencia == null)
            {
                return HttpNotFound();
            }
            return View(procedencia);
        }

        // POST: Procedencia/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre")] procedencia procedencia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(procedencia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(procedencia);
        }

        // GET: Procedencia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            procedencia procedencia = db.procedencia.Find(id);
            if (procedencia == null)
            {
                return HttpNotFound();
            }
            return View(procedencia);
        }

        // POST: Procedencia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            procedencia procedencia = db.procedencia.Find(id);
            try
            {
                db.procedencia.Remove(procedencia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(procedencia);


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