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
    public class PartoController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();
        private List<parto> partos = new List<parto>();
        // GET: Parto
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
        public ActionResult Index(int? page)
        {
            var parto = db.parto.Include(p => p.animal).Include(p => p.animal1);
            return View(parto.ToList().ToPagedList(page ?? 1, 5));
        }
        [HttpPost]
        public ActionResult Index(int? page, DateTime? fecha1, DateTime? fecha2)
        {
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {

                if (fecha1 > fecha2)
                {

                    ViewBag.Error = "Primera fecha no puede ser superior a la segunda";
                    var parto = db.parto;
                    return View(parto.ToList().ToPagedList(page ?? 1, 5));
                }
                if (fecha1 == null || fecha2 == null)
                {
                    ViewBag.Error = "Debe rellenar ambos campos para buscar por fecha";
                    var parto = db.parto;
                    return View(parto.ToList().ToPagedList(page ?? 1, 5));
                }
                else
                {

                    var parto = db.parto.Where(m => m.fecha >= (fecha1) && m.fecha <= fecha2);
                    int contador = parto.Count();
                    if (contador == 0)
                    {
                        ViewBag.Error = "No existen datos entre esos rangos";
                        var parto2 = db.parto;
                        return View(parto2.ToList().ToPagedList(page ?? 1, 5));
                    }
                    else
                    {
                        return View(parto.ToList().ToPagedList(page ?? 1, 5));
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
            List<parto> parto = new List<parto>();

            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                animal = dc.animal.ToList();
                parto = dc.parto.ToList();


            }

            WebGrid grid = new WebGrid(source: parto, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
                columns: grid.Columns(
                        grid.Column("id", "ID"),
                        grid.Column("fecha", "Fecha"),
                        grid.Column("animal.codigo_sag", "Madre"),
                        grid.Column("animal.codigo_sag", "Animal Nacido")

                        )
                    ).ToString();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Partos.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();
        }
        // GET: Parto/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            parto parto = db.parto.Find(id);
            if (parto == null)
            {
                return HttpNotFound();
            }
            return View(parto);
        }

        // GET: Parto/Create
        public ActionResult Create()
        {
            ViewBag.madre_id = new SelectList(db.animal.Where(a => a.estado.nombre != "MUERTO" && a.estado.nombre != "VENDIDO" && a.sexo.Equals("H")), "id", "codigo_sag");
            ViewBag.animal_id = new SelectList(db.animal.Where(a => a.estado.nombre != "MUERTO" && a.estado.nombre != "VENDIDO" && a.tipo.nombre.Equals("TERNERO") || a.tipo.nombre.Equals("TERNERA")), "id", "codigo_sag");
            return View();
        }

        // POST: Parto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fecha,madre_id,animal_id")] parto parto)
        {
            if (parto.madre_id == parto.animal_id)
            {
                ViewBag.Error = "Madre no puede ser igual al animal nacido o viceversa";
            }
            else
            {

                if (ModelState.IsValid)
                {
                    var anim = db.animal.Where(a => a.id == parto.animal_id).FirstOrDefault();
                    if (anim.fec_nac == null)
                    {
                        db.parto.Add(parto);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (anim.fec_nac != parto.fecha)
                        {
                            ViewBag.Error = "Fecha del parto debe ser igual a la del nacimiento del animal";
                        }
                        else
                        {
                            db.parto.Add(parto);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }

                }

            }

            ViewBag.madre_id = new SelectList(db.animal.Where(a => a.estado.nombre != "MUERTO" && a.estado.nombre != "VENDIDO" && a.sexo.Equals("H")), "id", "codigo_sag");
            ViewBag.animal_id = new SelectList(db.animal.Where(a => a.estado.nombre != "MUERTO" && a.estado.nombre != "VENDIDO" && a.tipo.nombre.Equals("TERNERO") || a.tipo.nombre.Equals("TERNERA")), "id", "codigo_sag");
            return View(parto);
        }

        // GET: Parto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            parto parto = db.parto.Find(id);
            if (parto == null)
            {
                return HttpNotFound();
            }
            ViewBag.madre_id = new SelectList(db.animal.Where(a => a.estado.nombre != "MUERTO" && a.estado.nombre != "VENDIDO" && a.sexo.Equals("H")), "id", "codigo_sag");
            ViewBag.animal_id = new SelectList(db.animal.Where(a => a.estado.nombre != "MUERTO" && a.estado.nombre != "VENDIDO" && a.tipo.nombre.Equals("TERNERO") || a.tipo.nombre.Equals("TERNERA")), "id", "codigo_sag");
            return View(parto);
        }

        // POST: Parto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fecha,madre_id,animal_id")] parto parto)
        {
            if (parto.madre_id == parto.animal_id)
            {
                ViewBag.Error = "Madre no puede ser igual al animal nacido o viceversa";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var anim = db.animal.Where(a => a.id == parto.animal_id).FirstOrDefault();
                    if (anim.fec_nac == null)
                    {
                        db.Entry(parto).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (anim.fec_nac != parto.fecha)
                        {
                            ViewBag.Error = "Fecha del parto debe ser igual a la del nacimiento del animal";
                        }
                        else
                        {
                            db.Entry(parto).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }

                }
            }

            ViewBag.madre_id = new SelectList(db.animal.Where(a => a.estado.nombre != "MUERTO" && a.estado.nombre != "VENDIDO" && a.sexo.Equals("H")), "id", "codigo_sag");
            ViewBag.animal_id = new SelectList(db.animal.Where(a => a.estado.nombre != "MUERTO" && a.estado.nombre != "VENDIDO" && a.tipo.nombre.Equals("TERNERO") || a.tipo.nombre.Equals("TERNERA")), "id", "codigo_sag");
            return View(parto);
        }

        // GET: Parto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            parto parto = db.parto.Find(id);
            if (parto == null)
            {
                return HttpNotFound();
            }
            return View(parto);
        }

        // POST: Parto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            parto parto = db.parto.Find(id);
            try
            {
                db.parto.Remove(parto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(parto);


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