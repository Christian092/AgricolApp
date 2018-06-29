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
using System.Web.Helpers;

namespace ProyectoV1.Controllers
{
    public class Tratamiento_animalController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();

        // GET: Tratamiento_animal
        public ActionResult Index(int? page)
        {
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {

                DateTime mañana = DateTime.Today.AddDays(1);
                int alertaHoy = db.tratamiento_animal.Where(m => m.fecha.Equals(mañana)).Count();
                if (alertaHoy == 1)
                {
                    TempData["notice"] = "Recuerda Que este Proximo Dia Tienes 1 Tratamiento de un Animal";
                }
                else if (alertaHoy == 2)
                {
                    TempData["notice"] = "Recuerda Que este Proximo Dia Tienes 2 Tratamientos de Animales";
                }
                else if (alertaHoy >= 3)
                {
                    TempData["notice"] = "Recuerda Que este Proximo Dia Tienes Varios Tratamientos de Animales";
                }

            }
            var tratamiento_animal = db.tratamiento_animal.Include(t => t.animal).Include(t => t.tratador).Include(t => t.tipo_tratamiento);
            return View(tratamiento_animal.ToList().ToPagedList(page ?? 1, 5));
        }
        [HttpPost]
        public ActionResult Index(int? page, DateTime fecha1)
        {
            if (fecha1 == null)
            {
                ViewBag.Error = "Ingrese una fecha";
                var trat = db.tratamiento_animal;
                return View(trat.ToList().ToPagedList(page ?? 1, 5));

            }
            else
            {
                using (bdagricolaEntities dc = new bdagricolaEntities())
                {

                    var trat = db.tratamiento_animal.Where(m => m.fecha == (fecha1));
                    return View(trat.ToList().ToPagedList(page ?? 1, 5));
                }
            }



        }

        // GET: Tratamiento_animal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tratamiento_animal tratamiento_animal = db.tratamiento_animal.Find(id);
            if (tratamiento_animal == null)
            {
                return HttpNotFound();
            }
            return View(tratamiento_animal);
        }

        // GET: Tratamiento_animal/Create
        public ActionResult Create()
        {
            ViewBag.animal_id = new SelectList(db.animal.Where(a => a.estado.nombre != "VENDIDO" && a.estado.nombre != "MUERTO"), "id", "codigo_sag");
            ViewBag.tratador_id = new SelectList(db.tratador, "id", "nombre");
            ViewBag.tipo_tratamiento_id = new SelectList(db.tipo_tratamiento, "id", "nombre");
            return View();
        }

        // POST: Tratamiento_animal/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fecha,animal_id,tratador_id,tipo_tratamiento_id")] tratamiento_animal tratamiento_animal)
        {
            if (ModelState.IsValid)
            {
                var anim = db.animal.Where(a => a.id == tratamiento_animal.animal_id).FirstOrDefault();
                if (anim.fec_nac == null)
                {
                    db.tratamiento_animal.Add(tratamiento_animal);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (anim.fec_nac > tratamiento_animal.fecha)
                    {
                        ViewBag.Error = "Fecha del tratamiento no puede ser antes que la fecha de nacimiento del animal";
                    }
                    else
                    {
                        db.tratamiento_animal.Add(tratamiento_animal);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

            }

            ViewBag.animal_id = new SelectList(db.animal.Where(a => a.estado.nombre != "VENDIDO" && a.estado.nombre != "MUERTO"), "id", "codigo_sag", tratamiento_animal.animal_id);
            ViewBag.tratador_id = new SelectList(db.tratador, "id", "nombre", tratamiento_animal.tratador_id);
            ViewBag.tipo_tratamiento_id = new SelectList(db.tipo_tratamiento, "id", "nombre", tratamiento_animal.tipo_tratamiento_id);
            return View(tratamiento_animal);
        }

        // GET: Tratamiento_animal/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tratamiento_animal tratamiento_animal = db.tratamiento_animal.Find(id);
            if (tratamiento_animal == null)
            {
                return HttpNotFound();
            }
            ViewBag.animal_id = new SelectList(db.animal, "id", "codigo_sag", tratamiento_animal.animal_id);
            ViewBag.tratador_id = new SelectList(db.tratador, "id", "nombre", tratamiento_animal.tratador_id);
            ViewBag.tipo_tratamiento_id = new SelectList(db.tipo_tratamiento, "id", "nombre", tratamiento_animal.tipo_tratamiento_id);
            return View(tratamiento_animal);
        }

        // POST: Tratamiento_animal/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,animal_id,tratamiento_id")] tratamiento_animal tratamiento_animal)
        {
            if (ModelState.IsValid)
            {
                var anim = db.animal.Where(a => a.id == tratamiento_animal.animal_id).FirstOrDefault();
                if (anim.fec_nac == null)
                {
                    db.Entry(tratamiento_animal).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (anim.fec_nac != tratamiento_animal.fecha)
                    {
                        ViewBag.Error = "Fecha del secamiento debe ser igual a la del nacimiento del animal";
                    }
                    else
                    {
                        db.Entry(tratamiento_animal).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

            }
            ViewBag.animal_id = new SelectList(db.animal, "id", "codigo_sag", tratamiento_animal.animal_id);
            ViewBag.tratador_id = new SelectList(db.tratador, "id", "nombre", tratamiento_animal.tratador_id);
            ViewBag.tipo_tratamiento_id = new SelectList(db.tipo_tratamiento, "id", "nombre", tratamiento_animal.tipo_tratamiento_id);
            return View(tratamiento_animal);
        }

        // GET: Tratamiento_animal/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tratamiento_animal tratamiento_animal = db.tratamiento_animal.Find(id);
            if (tratamiento_animal == null)
            {
                return HttpNotFound();
            }
            return View(tratamiento_animal);
        }
        public void GetExcel()
        {
            List<animal> animal = new List<animal>();
            List<tratador> tratador = new List<tratador>();
            List<tipo_tratamiento> tipo_tratamiento = new List<tipo_tratamiento>();
            List<tratamiento_animal> tratamiento_animal = new List<tratamiento_animal>();
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                animal = dc.animal.ToList();
                tratamiento_animal = dc.tratamiento_animal.ToList();
                tratador = dc.tratador.ToList();
                tipo_tratamiento = dc.tipo_tratamiento.ToList();

            }

            WebGrid grid = new WebGrid(source: tratamiento_animal, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
                columns: grid.Columns(
                        grid.Column("id", "ID"),
                        grid.Column("fecha", "Fecha"),
                        grid.Column("animal.codigo_sag", "Animal"),
                        grid.Column("tratador.nombre", "Tratador"),
                        grid.Column("tipo_tratamiento.nombre", "Tipo")
                        )
                    ).ToString();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=TratamientosAnimales.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();
        }

        // POST: Tratamiento_animal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tratamiento_animal tratamiento_animal = db.tratamiento_animal.Find(id);
            try
            {
                db.tratamiento_animal.Remove(tratamiento_animal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(tratamiento_animal);

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