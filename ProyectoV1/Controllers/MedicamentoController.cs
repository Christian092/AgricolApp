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
using PagedList.Mvc;
using System.Web.Helpers;

namespace ProyectoV1.Controllers
{
    public class MedicamentoController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();
        private List<medicamento> medicamentos = new List<medicamento>();
        // GET: Medicamento

        public ActionResult Inicio(int? page)
        {
            return View(db.medicamento.ToList().ToPagedList(page ?? 1, 5));

        }
        [HttpPost]
        public ActionResult Inicio(int? page, string nombre)
        {
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                var medicamentos = from s in db.medicamento select s;
                if (!string.IsNullOrEmpty(nombre))
                {
                    medicamentos = db.medicamento.Where(m => m.nombre.Contains(nombre));

                }
                return View(medicamentos.ToList().ToPagedList(page ?? 1, 5));
                //var medicamentos = db.medicamento.Where(m => m.nombre.Equals(nombre) );
                //return View(medicamentos.ToList().ToPagedList(page ?? 1, 5));
            }

        }

        public void GetExcel()
        {
            List<medicamento> medicamento = new List<medicamento>();
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                medicamento = dc.medicamento.ToList();
            }

            WebGrid grid = new WebGrid(source: medicamento, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
                columns: grid.Columns(
                        grid.Column("id", "ID"),
                        grid.Column("nombre", "Nombre"),
                        grid.Column("descripcion", "Descripcion")

                        )
                    ).ToString();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Medicamentos.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();
        }
        // GET: Medicamento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medicamento medicamento = db.medicamento.Find(id);
            if (medicamento == null)
            {
                return HttpNotFound();
            }
            return View(medicamento);
        }

        // GET: Medicamento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medicamento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,descripcion")] medicamento medicamento)
        {
            if (ModelState.IsValid)
            {
                bool f = false;
                medicamento.nombre = medicamento.nombre.ToUpperInvariant();
                var medicamentos = db.medicamento.Select(a => a.nombre);
                foreach (var a in medicamentos)
                {

                    if (a == medicamento.nombre)
                    {
                        ViewBag.Error = "Medicamento ya Existe";
                        f = true;
                    }


                }
                if (f == true)
                {
                    return View(medicamento);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        db.medicamento.Add(medicamento);
                        db.SaveChanges();
                        return RedirectToAction("Inicio");
                    }
                }

            }

            return View(medicamento);
        }

        // GET: Medicamento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medicamento medicamento = db.medicamento.Find(id);
            if (medicamento == null)
            {
                return HttpNotFound();
            }
            return View(medicamento);
        }

        // POST: Medicamento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,descripcion")] medicamento medicamento)
        {
            bool f = false;
            medicamento.nombre = medicamento.nombre.ToUpperInvariant();
            var medicamentos = db.medicamento.Select(a => a.nombre);
            foreach (var a in medicamentos)
            {

                if (a == medicamento.nombre)
                {
                    ViewBag.Error = "Medicamento ya Existe";
                    f = true;
                }


            }
            if (f == true)
            {
                return View(medicamento);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(medicamento).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Inicio");
                }
            }

            return View(medicamento);
        }

        // GET: Medicamento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medicamento medicamento = db.medicamento.Find(id);
            if (medicamento == null)
            {
                return HttpNotFound();
            }
            return View(medicamento);
        }

        // POST: Medicamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            medicamento medicamento = db.medicamento.Find(id);
            try
            {
                db.medicamento.Remove(medicamento);
                db.SaveChanges();
                return RedirectToAction("Inicio");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(medicamento);

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