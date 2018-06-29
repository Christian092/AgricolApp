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
    public class VentaController : Controller
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
        // GET: Venta
        public ActionResult Index(int? page)
        {
            var venta = db.venta.Include(v => v.animal).Include(v => v.comprador);
            return View(venta.ToList().ToPagedList(page ?? 1, 5));
        }
        [HttpPost]
        public ActionResult Index(int? page, DateTime? fecha1, DateTime? fecha2)
        {
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {

                if (fecha1 > fecha2)
                {

                    ViewBag.Error = "Primera fecha no puede ser superior a la segunda";
                    var venta = db.venta;
                    return View(venta.ToList().ToPagedList(page ?? 1, 5));
                }
                if (fecha1 == null || fecha2 == null)
                {
                    ViewBag.Error = "Debe rellenar ambos campos para buscar por fecha";
                    var venta = db.venta;
                    return View(venta.ToList().ToPagedList(page ?? 1, 5));
                }
                else
                {

                    var venta = db.venta.Where(m => m.fecha >= (fecha1) && m.fecha <= fecha2);
                    int contador = venta.Count();
                    if (contador == 0)
                    {
                        ViewBag.Error = "No existen datos entre esos rangos";
                        var venta2 = db.venta;
                        return View(venta2.ToList().ToPagedList(page ?? 1, 5));
                    }
                    else
                    {
                        return View(venta.ToList().ToPagedList(page ?? 1, 5));
                    }

                }


            }


        }
        public void GetExcel()
        {
            List<venta> venta = new List<venta>();
            List<animal> animal = new List<animal>();
            List<comprador> comprador = new List<comprador>();
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                venta = dc.venta.ToList();
                animal = dc.animal.ToList();
                comprador = dc.comprador.ToList();
            }

            WebGrid grid = new WebGrid(source: venta, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
                columns: grid.Columns(
                        grid.Column("id", "ID"),
                        grid.Column("fecha", "Fecha"),
                        grid.Column("pesaje", "Pesaje"),
                        grid.Column("precio", "Precio"),
                        grid.Column("animal.codigo_sag", "Animal"),
                        grid.Column("comprador.nombre", "Comprador")
                        )
                    ).ToString();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Ventas.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();
        }
        // GET: Venta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            venta venta = db.venta.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        // GET: Venta/Create
        public ActionResult Create()
        {
            ViewBag.animal_id = new SelectList(db.animal.Where(a => a.estado.nombre != "MUERTO" && a.estado.nombre != "VENDIDO"), "id", "codigo_sag");
            ViewBag.comprador_id = new SelectList(db.comprador, "id", "nombre");
            return View();
        }

        // POST: Venta/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fecha,pesaje,precio,animal_id,comprador_id")] venta venta)
        {
            if (ModelState.IsValid)
            {
                var anim = db.animal.Where(a => a.id == venta.animal_id).FirstOrDefault();
                if (anim.fec_nac == null)
                {
                    int codigo_animal = venta.animal_id;

                    animal ani = db.animal.Where(a => a.id == codigo_animal).FirstOrDefault();
                    estado est = db.estado.Where(e => e.nombre == "VENDIDO").FirstOrDefault();

                    ani.estado = est;
                    db.venta.Add(venta);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (anim.fec_nac <= venta.fecha)
                    {
                        int codigo_animal = venta.animal_id;

                        animal ani = db.animal.Where(a => a.id == codigo_animal).FirstOrDefault();
                        estado est = db.estado.Where(e => e.nombre == "VENDIDO").FirstOrDefault();

                        ani.estado = est;
                        db.venta.Add(venta);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Error = "Fecha de Venta no puede ser menor a la fecha de nacimiento del animal";

                    }
                }


            }

            ViewBag.animal_id = new SelectList(db.animal, "id", "codigo_sag", venta.animal_id);
            ViewBag.comprador_id = new SelectList(db.comprador, "id", "nombre", venta.comprador_id);
            return View(venta);
        }

        // GET: Venta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            venta venta = db.venta.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            ViewBag.animal_id = new SelectList(db.animal, "id", "codigo_sag", venta.animal_id);
            ViewBag.comprador_id = new SelectList(db.comprador, "id", "nombre", venta.comprador_id);
            return View(venta);
        }

        // POST: Venta/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fecha,pesaje,precio,animal_id,comprador_id")] venta venta)
        {
            if (ModelState.IsValid)
            {
                var anim = db.animal.Where(a => a.id == venta.animal_id).FirstOrDefault();
                if (anim.fec_nac <= venta.fecha)
                {
                    db.Entry(venta).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Fecha de Venta no puede ser menor a la fecha de nacimiento del animal";
                    ViewBag.animal_id = new SelectList(db.animal, "id", "codigo_sag", venta.animal_id);
                    ViewBag.comprador_id = new SelectList(db.comprador, "id", "nombre", venta.comprador_id);
                    return View(venta);
                }

            }
            ViewBag.animal_id = new SelectList(db.animal, "id", "codigo_sag", venta.animal_id);
            ViewBag.comprador_id = new SelectList(db.comprador, "id", "nombre", venta.comprador_id);
            return View(venta);
        }

        // GET: Venta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            venta venta = db.venta.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        // POST: Venta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            venta venta = db.venta.Find(id);
            try
            {
                db.venta.Remove(venta);
                int codigo_animal = venta.animal_id;

                animal ani = db.animal.Where(a => a.id == codigo_animal).FirstOrDefault();
                estado est = db.estado.Where(e => e.nombre == "NORMAL").FirstOrDefault();

                ani.estado = est;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(venta);




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