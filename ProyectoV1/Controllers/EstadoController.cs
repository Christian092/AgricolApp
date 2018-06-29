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
    public class EstadoController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Estado
        public ActionResult Index()
        {
            return View(db.estado.ToList());
        }

        // GET: Estado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            estado estado = db.estado.Find(id);
            if (estado == null)
            {
                return HttpNotFound();
            }
            return View(estado);
        }

        // GET: Estado/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Estado/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre")] estado estado)
        {
            if (ModelState.IsValid)
            {
                bool f = false;
                estado.nombre = estado.nombre.ToUpper();
                var estados = db.estado.Select(a => a.nombre);
                foreach (var a in estados)
                {

                    if (a == estado.nombre)
                    {
                        ViewBag.Error = "Estado ya Existe";
                        f = true;
                    }


                }
                if (f == true)
                {
                    return View(estado);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        db.estado.Add(estado);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

            }


            return View(estado);
        }

        // GET: Estado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            estado estado = db.estado.Find(id);
            if (estado == null)
            {
                return HttpNotFound();
            }
            return View(estado);
        }

        // POST: Estado/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre")] estado estado)
        {
            bool f = false;
            estado.nombre = estado.nombre.ToUpperInvariant();
            var estados = db.estado.Select(a => a.nombre);
            foreach (var a in estados)
            {

                if (a == estado.nombre)
                {
                    ViewBag.Error = "Estado ya Existe";
                    f = true;
                }


            }
            if (f == true)
            {
                return View(estado);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(estado).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(estado);
        }

        // GET: Estado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            estado estado = db.estado.Find(id);
            if (estado == null)
            {
                return HttpNotFound();
            }
            return View(estado);
        }

        // POST: Estado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            estado estado = db.estado.Find(id);
            try
            {

                db.estado.Remove(estado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(estado);
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