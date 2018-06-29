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
    public class MuerteController : Controller
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
        // GET: Muerte
        public ActionResult Index(int? page)
        {


            var muerte = db.muerte.Include(m => m.animal);
            return View(muerte.ToList().ToPagedList(page ?? 1, 5));
        }
        [HttpPost]
        public ActionResult Index(int? page, DateTime? fecha1, DateTime? fecha2)
        {
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {

                if (fecha1 > fecha2)
                {

                    ViewBag.Error = "Primera fecha no puede ser superior a la segunda";
                    var muerte = db.muerte;
                    return View(muerte.ToList().ToPagedList(page ?? 1, 5));
                }
                if (fecha1 == null || fecha2 == null)
                {
                    ViewBag.Error = "Debe rellenar ambos campos para buscar por fecha";
                    var muerte = db.muerte;
                    return View(muerte.ToList().ToPagedList(page ?? 1, 5));
                }
                else
                {

                    var muerte = db.muerte.Where(m => m.fecha >= (fecha1) && m.fecha <= fecha2);
                    int contador = muerte.Count();
                    if (contador == 0)
                    {
                        ViewBag.Error = "No existen datos entre esos rangos";
                        var muerte2 = db.muerte;
                        return View(muerte2.ToList().ToPagedList(page ?? 1, 5));
                    }
                    else
                    {
                        return View(muerte.ToList().ToPagedList(page ?? 1, 5));
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
            List<animal> animal = new List<animal>();
            List<muerte> muerte = new List<muerte>();
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                muerte = dc.muerte.ToList();
                animal = dc.animal.ToList();

            }

            WebGrid grid = new WebGrid(source: muerte, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
                columns: grid.Columns(
                        grid.Column("id", "ID"),
                        grid.Column("fecha", "Fecha"),
                        grid.Column("causa", "Causa"),
                        grid.Column("animal.codigo_sag", "Animal")

                        )
                    ).ToString();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Muertes.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();
        }

        // GET: Muerte/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            muerte muerte = db.muerte.Find(id);
            if (muerte == null)
            {
                return HttpNotFound();
            }
            return View(muerte);
        }

        // GET: Muerte/Create
        public ActionResult Create()
        {
            ViewBag.animal_id = new SelectList(db.animal.Where(a => a.estado.nombre != "MUERTO" && a.estado.nombre != "VENDIDO"), "id", "codigo_sag");
            return View();
        }

        // POST: Muerte/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fecha,causa,animal_id")] muerte muerte)
        {
            if (ModelState.IsValid)
            {
                var anim = db.animal.Where(a => a.id == muerte.animal_id).FirstOrDefault();
                if (anim.fec_nac == null)
                {
                    int sag = muerte.animal_id;

                    animal ani = db.animal.Where(a => a.id == sag).FirstOrDefault();
                    estado est = db.estado.Where(e => e.nombre == "MUERTO").FirstOrDefault();

                    ani.estado = est;

                    db.muerte.Add(muerte);
                    db.SaveChanges();


                    return RedirectToAction("Index");
                }
                else
                {
                    if (anim.fec_nac > muerte.fecha)
                    {
                        ViewBag.Error = "Fecha de muerte no debe ser antes de la fecha de nacimiento del animal";
                    }
                    else
                    {
                        int sag = muerte.animal_id;

                        animal ani = db.animal.Where(a => a.id == sag).FirstOrDefault();
                        estado est = db.estado.Where(e => e.nombre == "MUERTO").FirstOrDefault();

                        ani.estado = est;
                        db.muerte.Add(muerte);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

            }

            ViewBag.animal_id = new SelectList(db.animal, "id", "codigo_sag", muerte.animal_id);
            return View(muerte);
        }

        // GET: Muerte/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            muerte muerte = db.muerte.Find(id);
            if (muerte == null)
            {
                return HttpNotFound();
            }
            ViewBag.animal_id = new SelectList(db.animal, "id", "codigo_sag", muerte.animal_id);
            return View(muerte);
        }

        // POST: Muerte/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fecha,causa,animal_id")] muerte muerte)
        {
            if (ModelState.IsValid)
            {
                var anim = db.animal.Where(a => a.id == muerte.animal_id).FirstOrDefault();
                if (anim.fec_nac == null)
                {
                    db.Entry(muerte).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (anim.fec_nac > muerte.fecha)
                    {
                        ViewBag.Error = "Fecha de muerte no debe ser antes de la fecha de nacimiento del animal";
                    }
                    else
                    {
                        db.Entry(muerte).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

            }
            ViewBag.animal_id = new SelectList(db.animal, "id", "codigo_sag", muerte.animal_id);
            return View(muerte);
        }

        // GET: Muerte/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            muerte muerte = db.muerte.Find(id);
            if (muerte == null)
            {
                return HttpNotFound();
            }
            return View(muerte);
        }

        // POST: Muerte/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            muerte muerte = db.muerte.Find(id);
            try
            {
                int sag = muerte.animal_id;

                animal ani = db.animal.Where(a => a.id == sag).FirstOrDefault();
                estado est = db.estado.Where(e => e.nombre == "NORMAL").FirstOrDefault();

                ani.estado = est;
                db.muerte.Remove(muerte);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(muerte);


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