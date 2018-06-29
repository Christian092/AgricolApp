using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoV1.Models;
using System.Web.Helpers;
using PagedList;

namespace ProyectoV1.Controllers
{
    public class InseminadorController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();
        public ActionResult Index(int? page)
        {
            var inseminador = db.inseminador;
            return View(inseminador.ToList().ToPagedList(page ?? 1, 5));
        }
        // GET: Inseminador
        public void GetExcel()
        {
            List<inseminador> inseminador = new List<inseminador>();
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                inseminador = dc.inseminador.ToList();
            }

            WebGrid grid = new WebGrid(source: inseminador, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
                columns: grid.Columns(
                        grid.Column("id", "ID"),
                        grid.Column("nombre", "Nombre"),
                        grid.Column("apellido", "Apellido"),
                        grid.Column("numero", "Telefono")

                        )
                    ).ToString();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Inseminadores.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();
        }


        // GET: Inseminador/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inseminador inseminador = db.inseminador.Find(id);
            if (inseminador == null)
            {
                return HttpNotFound();
            }
            return View(inseminador);
        }

        // GET: Inseminador/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inseminador/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,apellido,numero")] inseminador inseminador)
        {
            if (ModelState.IsValid)
            {
                db.inseminador.Add(inseminador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inseminador);
        }

        // GET: Inseminador/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inseminador inseminador = db.inseminador.Find(id);
            if (inseminador == null)
            {
                return HttpNotFound();
            }
            return View(inseminador);
        }

        // POST: Inseminador/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,apellido,numero")] inseminador inseminador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inseminador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inseminador);
        }

        // GET: Inseminador/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inseminador inseminador = db.inseminador.Find(id);
            if (inseminador == null)
            {
                return HttpNotFound();
            }
            return View(inseminador);
        }

        // POST: Inseminador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            inseminador inseminador = db.inseminador.Find(id);
            try
            {
                db.inseminador.Remove(inseminador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(inseminador);

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