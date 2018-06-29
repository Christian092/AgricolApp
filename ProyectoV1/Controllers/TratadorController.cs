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
    public class TratadorController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Tratador
        public ActionResult Index()
        {
            return View(db.tratador.ToList());
        }

        // GET: Tratador/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tratador tratador = db.tratador.Find(id);
            if (tratador == null)
            {
                return HttpNotFound();
            }
            return View(tratador);
        }

        // GET: Tratador/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tratador/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,apellido,telefono")] tratador tratador)
        {
            if (ModelState.IsValid)
            {
                db.tratador.Add(tratador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tratador);
        }

        // GET: Tratador/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tratador tratador = db.tratador.Find(id);
            if (tratador == null)
            {
                return HttpNotFound();
            }
            return View(tratador);
        }

        // POST: Tratador/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,apellido,telefono")] tratador tratador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tratador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tratador);
        }

        // GET: Tratador/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tratador tratador = db.tratador.Find(id);
            if (tratador == null)
            {
                return HttpNotFound();
            }
            return View(tratador);
        }

        // POST: Tratador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tratador tratador = db.tratador.Find(id);
            try
            {
                db.tratador.Remove(tratador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(tratador);

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