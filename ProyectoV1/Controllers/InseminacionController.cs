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
    public class InseminacionController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();
        public JsonResult fechaValida(DateTime fecha)
        {
            DateTime fechaActual = DateTime.Today;
            bool respuesta = false;
            if (fecha > fechaActual)
            {
                respuesta = false;
            }
            else
            {
                respuesta = true;
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        // GET: Inseminacion
        public ActionResult Index(int? page)
        {
            var inseminacion = db.inseminacion.Include(i => i.animal).Include(i => i.inseminador).Include(i => i.procedencia_semen);
            return View(inseminacion.ToList().ToPagedList(page ?? 1, 5));
        }
        [HttpPost]
        public ActionResult Index(int? page, DateTime? fecha1, DateTime? fecha2)
        {
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {

                if (fecha1 > fecha2)
                {

                    ViewBag.Error = "Primera Fecha no puede ser superior a la segunda";
                    var inseminacion = db.inseminacion;
                    return View(inseminacion.ToList().ToPagedList(page ?? 1, 5));
                }
                if (fecha1 == null || fecha2 == null)
                {
                    ViewBag.Error = "Debe rellenar ambos campos para buscar por fecha";
                    var inseminacion = db.inseminacion;
                    return View(inseminacion.ToList().ToPagedList(page ?? 1, 5));
                }
                else
                {

                    var inseminacion = db.inseminacion.Where(m => m.fecha >= (fecha1) && m.fecha <= fecha2);
                    int contador = inseminacion.Count();
                    if (contador == 0)
                    {
                        ViewBag.Error = "No existen datos entre esos rangos";
                        var inseminacion2 = db.inseminacion;
                        return View(inseminacion2.ToList().ToPagedList(page ?? 1, 5));
                    }
                    else
                    {
                        return View(inseminacion.ToList().ToPagedList(page ?? 1, 5));
                    }
                }

            }

            //using (bdagricolaEntities dc = new bdagricolaEntities())
            //{

            //    var ordenas = db.ordena.Where(m => m.fecha >= (fecha1) && m.fecha <= fecha2);
            //    return View(ordenas.ToList().ToPagedList(page ?? 1, 5));
            //}

        }

        public void GetExcel()
        {
            List<inseminacion> inseminacion = new List<inseminacion>();
            List<inseminador> inseminador = new List<inseminador>();
            List<animal> animal = new List<animal>();
            List<procedencia_semen> procedencia_semen = new List<procedencia_semen>();
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                inseminacion = dc.inseminacion.ToList();
                animal = dc.animal.ToList();
                inseminador = dc.inseminador.ToList();
                procedencia_semen = dc.procedencia_semen.ToList();

            }

            WebGrid grid = new WebGrid(source: inseminacion, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
                columns: grid.Columns(
                        grid.Column("id", "ID"),
                        grid.Column("fecha", "Fecha"),
                        grid.Column("inseminador.nombre", "Inseminador"),
                        grid.Column("animal.codigo_sag", "Animal"),
                        grid.Column("procedencia_semen.nombre", "Procedencia Semen")
                        )
                    ).ToString();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Inseminaciones.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();
        }
        // GET: Inseminacion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inseminacion inseminacion = db.inseminacion.Find(id);
            if (inseminacion == null)
            {
                return HttpNotFound();
            }
            return View(inseminacion);
        }

        // GET: Inseminacion/Create
        public ActionResult Create()
        {
            ViewBag.animal_id = new SelectList(db.animal.Where(a => a.estado.nombre != "MUERTO" && a.estado.nombre != "VENDIDO"), "id", "codigo_sag");
            ViewBag.inseminador_id = new SelectList(db.inseminador, "id", "nombre");
            ViewBag.procedencia_semen_id = new SelectList(db.procedencia_semen, "id", "nombre");
            return View();
        }

        // POST: Inseminacion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fecha,inseminador_id,animal_id,procedencia_semen_id")] inseminacion inseminacion)
        {

            if (ModelState.IsValid)
            {
                var anim = db.animal.Where(a => a.id == inseminacion.animal_id).FirstOrDefault();
                if (anim.fec_nac == null)
                {
                    db.inseminacion.Add(inseminacion);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (anim.fec_nac > inseminacion.fecha)
                    {
                        ViewBag.Error = "Fecha de inseminacion no debe ser antes de la fecha de nacimiento del animal";
                    }
                    else
                    {
                        db.inseminacion.Add(inseminacion);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

            }

            ViewBag.animal_id = new SelectList(db.animal, "id", "codigo_sag", inseminacion.animal_id);
            ViewBag.inseminador_id = new SelectList(db.inseminador, "id", "nombre", inseminacion.inseminador_id);
            ViewBag.procedencia_semen_id = new SelectList(db.procedencia_semen, "id", "nombre", inseminacion.procedencia_semen_id);
            return View(inseminacion);
        }

        // GET: Inseminacion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inseminacion inseminacion = db.inseminacion.Find(id);
            if (inseminacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.animal_id = new SelectList(db.animal, "id", "codigo_sag", inseminacion.animal_id);
            ViewBag.inseminador_id = new SelectList(db.inseminador, "id", "nombre", inseminacion.inseminador_id);
            ViewBag.procedencia_semen_id = new SelectList(db.procedencia_semen, "id", "nombre", inseminacion.procedencia_semen_id);
            return View(inseminacion);
        }

        // POST: Inseminacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fecha,inseminador_id,animal_id,procedencia_semen_id")] inseminacion inseminacion)
        {
            if (ModelState.IsValid)
            {
                var anim = db.animal.Where(a => a.id == inseminacion.animal_id).FirstOrDefault();
                if (anim.fec_nac == null)
                {
                    db.Entry(inseminacion).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (anim.fec_nac > inseminacion.fecha)
                    {
                        ViewBag.Error = "Fecha de inseminacion no debe ser antes de la fecha de nacimiento del animal";
                    }
                    else
                    {
                        db.Entry(inseminacion).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

            }
            ViewBag.animal_id = new SelectList(db.animal.Where(a => a.estado.nombre != "MUERTO" && a.estado.nombre != "VENDIDO"), "id", "codigo_sag", inseminacion.animal_id);
            ViewBag.inseminador_id = new SelectList(db.inseminador, "id", "nombre", inseminacion.inseminador_id);
            ViewBag.procedencia_semen_id = new SelectList(db.procedencia_semen, "id", "nombre", inseminacion.procedencia_semen_id);
            return View(inseminacion);
        }

        // GET: Inseminacion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inseminacion inseminacion = db.inseminacion.Find(id);
            if (inseminacion == null)
            {
                return HttpNotFound();
            }
            return View(inseminacion);
        }

        // POST: Inseminacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            inseminacion inseminacion = db.inseminacion.Find(id);
            try
            {
                db.inseminacion.Remove(inseminacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(inseminacion);


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