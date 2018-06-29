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
    public class SecamientoController : Controller
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
        // GET: Secamiento
        public ActionResult Index(int? page)
        {
            var secamiento = db.secamiento.Include(s => s.animal).Include(s => s.medicamento);
            return View(secamiento.ToList().ToPagedList(page ?? 1, 5));
        }
        [HttpPost]
        public ActionResult Index(int? page, DateTime? fecha1, DateTime? fecha2)
        {
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {

                if (fecha1 > fecha2)
                {

                    ViewBag.Error = "Primera fecha no puede ser superior a la segunda";
                    var secamiento = db.secamiento;
                    return View(secamiento.ToList().ToPagedList(page ?? 1, 5));
                }
                if (fecha1 == null || fecha2 == null)
                {
                    ViewBag.Error = "Debe rellenar ambos campos para buscar por fecha";
                    var secamiento1 = db.secamiento;
                    return View(secamiento1.ToList().ToPagedList(page ?? 1, 5));
                }
                else
                {

                    var secamiento2 = db.secamiento.Where(m => m.fecha >= (fecha1) && m.fecha <= fecha2);
                    int contador = secamiento2.Count();
                    if (contador == 0)
                    {
                        ViewBag.Error = "No existen datos entre esos rangos";
                        var secamiento = db.secamiento;
                        return View(secamiento.ToList().ToPagedList(page ?? 1, 5));
                    }
                    else
                    {
                        return View(secamiento2.ToList().ToPagedList(page ?? 1, 5));
                    }

                }


            }


        }
        public void GetExcel()
        {
            List<secamiento> secamiento = new List<secamiento>();
            List<animal> animal = new List<animal>();
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                secamiento = dc.secamiento.ToList();
                animal = dc.animal.ToList();
            }

            WebGrid grid = new WebGrid(source: secamiento, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
                columns: grid.Columns(
                        grid.Column("id", "ID"),
                        grid.Column("fecha", "Fecha"),
                        grid.Column("cantidad_ubres", "Tetas Secadas"),
                        grid.Column("animal.codigo_sag", "Animal")
                        )
                    ).ToString();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Secamientos.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();
        }
        // GET: Secamiento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            secamiento secamiento = db.secamiento.Find(id);
            if (secamiento == null)
            {
                return HttpNotFound();
            }
            return View(secamiento);
        }

        // GET: Secamiento/Create
        public ActionResult Create()
        {
            ViewBag.animal_id = new SelectList(db.animal.Where(a => a.estado.nombre != "MUERTO" && a.estado.nombre != "VENDIDO" && a.sexo.Equals("H")), "id", "codigo_sag");
            ViewBag.medicamento_id = new SelectList(db.medicamento, "id", "nombre");
            return View();
        }

        // POST: Secamiento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fecha,cantidad_ubres,animal_id,medicamento_id")] secamiento secamiento)
        {

            if (ModelState.IsValid)
            {
                var anim = db.animal.Where(a => a.id == secamiento.animal_id).FirstOrDefault();
                if (anim.fec_nac == null)
                {
                    db.secamiento.Add(secamiento);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (anim.fec_nac > secamiento.fecha)
                    {
                        ViewBag.Error = "Fecha del secamiento no debe ser antes de la fecha de nacimiento del animal";
                    }
                    else
                    {
                        db.secamiento.Add(secamiento);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

            }


            ViewBag.animal_id = new SelectList(db.animal, "id", "codigo_sag", secamiento.animal_id);
            ViewBag.medicamento_id = new SelectList(db.medicamento, "id", "nombre", secamiento.medicamento_id);
            return View(secamiento);
        }

        // GET: Secamiento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            secamiento secamiento = db.secamiento.Find(id);
            if (secamiento == null)
            {
                return HttpNotFound();
            }
            ViewBag.animal_id = new SelectList(db.animal, "id", "codigo_sag", secamiento.animal_id);
            ViewBag.medicamento_id = new SelectList(db.medicamento, "id", "nombre", secamiento.medicamento_id);
            return View(secamiento);
        }

        // POST: Secamiento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fecha,cantidad_ubres,animal_id,medicamento_id")] secamiento secamiento)
        {
            if (ModelState.IsValid)
            {
                var anim = db.animal.Where(a => a.id == secamiento.animal_id).FirstOrDefault();
                if (anim.fec_nac == null)
                {
                    db.Entry(secamiento).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (anim.fec_nac > secamiento.fecha)
                    {
                        ViewBag.Error = "Fecha del secamiento no debe ser antes de la fecha de nacimiento del animal";
                    }
                    else
                    {
                        db.Entry(secamiento).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

            }

            ViewBag.animal_id = new SelectList(db.animal, "id", "codigo_sag", secamiento.animal_id);
            ViewBag.medicamento_id = new SelectList(db.medicamento, "id", "nombre", secamiento.medicamento_id);
            return View(secamiento);
        }

        // GET: Secamiento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            secamiento secamiento = db.secamiento.Find(id);
            if (secamiento == null)
            {
                return HttpNotFound();
            }
            return View(secamiento);
        }

        // POST: Secamiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            secamiento secamiento = db.secamiento.Find(id);
            try
            {
                db.secamiento.Remove(secamiento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(secamiento);

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