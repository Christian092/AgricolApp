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
    public class TipoController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Tipo
        public ActionResult Index()
        {
            return View(db.tipo.ToList());
        }

        // GET: Tipo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo tipo = db.tipo.Find(id);
            if (tipo == null)
            {
                return HttpNotFound();
            }
            return View(tipo);
        }

        // GET: Tipo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tipo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre")] tipo tipo)
        {

            bool f = false;
            tipo.nombre = tipo.nombre.ToUpper();
            var tipos = db.tipo.Select(a => a.nombre);
            foreach (var a in tipos)
            {

                if (a == tipo.nombre)
                {
                    ViewBag.Error = "Tipo ya Existe";
                    f = true;
                }


            }
            if (f == true)
            {
                return View(tipo);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.tipo.Add(tipo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            

            return View(tipo);
        }

        // GET: Tipo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo tipo = db.tipo.Find(id);
            if (tipo == null)
            {
                return HttpNotFound();
            }
            return View(tipo);
        }

        // POST: Tipo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre")] tipo tipo)
        {
            bool f = false;
            tipo.nombre = tipo.nombre.ToUpperInvariant();
            var tipos = db.tipo.Select(a => a.nombre);
            foreach (var a in tipos)
            {

                if (a == tipo.nombre)
                {
                    ViewBag.Error = "Tipo ya Existe";
                    f = true;
                }


            }
            if (f == true)
            {
                return View(tipo);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tipo).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
           
            return View(tipo);
        }

        // GET: Tipo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo tipo = db.tipo.Find(id);
            if (tipo == null)
            {
                return HttpNotFound();
            }
            return View(tipo);
        }

        // POST: Tipo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tipo tipo = db.tipo.Find(id);
            try
            {
                db.tipo.Remove(tipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            
            return View(tipo);
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
