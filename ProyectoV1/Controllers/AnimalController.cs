using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoV1.Models;
using System.Web.Helpers;
using PagedList;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.Common;
using System.Windows.Media.Imaging;
using Rotativa;
using System.Web.UI;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;
namespace ProyectoV1.Controllers
{
    public class AnimalController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult CodigoQR(int? id)
        {
            animal animal = db.animal.Find(id);
            return View(animal);
        }
        public ActionResult CodigoQRAnimales()
        {
            var animal = db.animal.Include(a => a.estado);
            return View(animal);
        }
        public ActionResult ExportarQR()
        {
            return new ActionAsPdf("CodigoQR", db.animal);

        }
        public JsonResult codigoValido(string codigo_sag)
        {
            return Json(!db.animal.Any(a => a.codigo_sag == codigo_sag), JsonRequestBehavior.AllowGet);
        }
        public JsonResult fechaValida(DateTime fec_nac)
        {
            DateTime fechaActual = DateTime.Today;
            bool respuesta = false;
            if (fec_nac > fechaActual)
            {
                respuesta = false;
            }
            else
            {
                respuesta = true;
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        // GET: Animal
        public ActionResult Index(int? page)
        {
            var animal = db.animal.Include(a => a.estado).Include(a => a.raza).Include(a => a.tipo).Where(a => a.estado.nombre != "VENDIDO" && a.estado.nombre != "MUERTO");

            return View(animal.ToList().ToPagedList(page ?? 1, 5));
        }
        [HttpPost]
        public ActionResult Index(int? page, string nombre)
        {
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                var animales = db.animal.Include(a => a.estado).Include(a => a.raza).Include(a => a.tipo).Where(a => a.estado.nombre != "VENDIDO" && a.estado.nombre != "MUERTO");
                if (!string.IsNullOrEmpty(nombre))
                {
                    animales = db.animal.Where(m => m.codigo_sag.Contains(nombre)).Where(a => a.estado.nombre != "VENDIDO" && a.estado.nombre != "MUERTO");

                }
                return View(animales.ToList().ToPagedList(page ?? 1, 5));
                //var medicamentos = db.medicamento.Where(m => m.nombre.Equals(nombre) );
                //return View(medicamentos.ToList().ToPagedList(page ?? 1, 5));
            }

        }
        public ActionResult Historicos(int? page)
        {
            var animal = db.animal.Include(a => a.estado).Include(a => a.raza).Include(a => a.tipo);

            return View(animal.ToList().ToPagedList(page ?? 1, 5));
        }
        [HttpPost]
        public ActionResult Historicos(int? page, string nombre)
        {
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                var animales = from s in db.animal select s;
                if (!string.IsNullOrEmpty(nombre))
                {
                    animales = db.animal.Where(m => m.codigo_sag.Contains(nombre));

                }
                return View(animales.ToList().ToPagedList(page ?? 1, 5));
                //var medicamentos = db.medicamento.Where(m => m.nombre.Equals(nombre) );
                //return View(medicamentos.ToList().ToPagedList(page ?? 1, 5));
            }

        }
        public ActionResult PorVender(int? page)
        {

            List<animal> datos = new List<animal>();
            var animal = db.animal.Include(a => a.estado).Include(a => a.raza).Include(a => a.tipo);
            foreach (var anim in animal)
            {
                DateTime? naci = anim.fec_nac;
                DateTime nac_fec = Convert.ToDateTime(naci);
                DateTime edad = DateTime.MinValue + (DateTime.Today - nac_fec);
                int año = edad.Year - 1;
                if (año >= 2 && anim.fec_nac != null && anim.estado.nombre != "MUERTO" && anim.estado.nombre != "VENDIDO")
                {
                    datos.Add(anim);

                }
            }
            return View(datos.ToList().ToPagedList(page ?? 1, 5));

        }




        // GET: Animal/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            animal animal = db.animal.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                string error;


                DateTime? naci = animal.fec_nac;
                if (naci == null)
                {

                }
                else
                {
                    if (animal.estado.nombre.Equals("MUERTO") || animal.estado.nombre.Equals("VENDIDO"))
                    {

                    }
                    else
                    {

                        DateTime nac_fec = Convert.ToDateTime(naci);
                        DateTime edad = DateTime.MinValue + (DateTime.Today - nac_fec);
                        int año = edad.Year - 1;
                        if (año >= 2)
                        {
                            TempData["notice"] = "Este animal esta en la edad Optima para ser Vendido";
                        }
                    }

                }
            }
            return View(animal);
        }

        // GET: Animal/Create
        public ActionResult Create()
        {
            ViewBag.estado_id = new SelectList(db.estado, "id", "nombre");
            ViewBag.raza_id = new SelectList(db.raza, "id", "nombre");
            ViewBag.tipo_id = new SelectList(db.tipo, "id", "nombre");
            return View();
        }

        // POST: Animal/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codigo_sag,sexo,fec_nac,fecha_ing,tipo_id,raza_id,estado_id")] animal animal)
        {

            if (ModelState.IsValid)
            {

                if (animal.fec_nac > animal.fecha_ing)
                {
                    ViewBag.Error = "Fecha de nacimiento no debe ser superior a la de ingreso al fundo";

                }
                else
                {
                    DateTime? hoy = DateTime.Today;
                    if (animal.fec_nac > hoy || animal.fecha_ing > hoy)
                    {
                        ViewBag.Error = "Las fechas ingresadas no deben superar el dia actual";
                    }
                    else
                    {
                        db.animal.Add(animal);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

            }

            ViewBag.estado_id = new SelectList(db.estado, "id", "nombre", animal.estado_id);
            ViewBag.raza_id = new SelectList(db.raza, "id", "nombre", animal.raza_id);
            ViewBag.tipo_id = new SelectList(db.tipo, "id", "nombre", animal.tipo_id);
            return View(animal);
        }

        // GET: Animal/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            animal animal = db.animal.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            ViewBag.estado_id = new SelectList(db.estado, "id", "nombre", animal.estado_id);
            ViewBag.raza_id = new SelectList(db.raza, "id", "nombre", animal.raza_id);
            ViewBag.tipo_id = new SelectList(db.tipo, "id", "nombre", animal.tipo_id);

            return View(animal);
        }

        // POST: Animal/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codigo_sag,sexo,fec_nac,fecha_ing,tipo_id,raza_id,estado_id")] animal animal)
        {
            if (ModelState.IsValid)
            {
                if (animal.fec_nac > animal.fecha_ing)
                {
                    ViewBag.Error = "Fecha de nacimiento no debe ser superior a la de ingreso al fundo";

                }
                else
                {
                    DateTime? hoy = DateTime.Today;
                    if (animal.fec_nac > hoy || animal.fecha_ing > hoy)
                    {
                        ViewBag.Error = "Las fechas ingresadas no deben superar el dia actual";
                    }
                    else
                    {
                        db.Entry(animal).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

            }
            ViewBag.estado_id = new SelectList(db.estado, "id", "nombre", animal.estado_id);
            ViewBag.raza_id = new SelectList(db.raza, "id", "nombre", animal.raza_id);
            ViewBag.tipo_id = new SelectList(db.tipo, "id", "nombre", animal.tipo_id);
            return View(animal);
        }

        // GET: Animal/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            animal animal = db.animal.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        // POST: Animal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            animal animal = db.animal.Find(id);
            try
            {

                db.animal.Remove(animal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados a el animal";


            }
            return View(animal);
        }
        //public ActionResult Inicio()
        //{
        //    List<animal> allCust = new List<animal>();
        //    using (MyDatabaseEntities dc = new MyDatabaseEntities())
        //    {
        //        allCust = dc.CustomerInfoes.ToList();
        //    }
        //    return View(allCust);
        //}
        public void GetExcel()
        {
            List<animal> animal = new List<animal>();

            List<tipo> tipo = new List<tipo>();
            List<raza> raza = new List<raza>();
            List<estado> estado = new List<estado>();
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                animal = dc.animal.Where(a => a.estado.nombre != "VENDIDO" && a.estado.nombre != "MUERTO").ToList();

                tipo = dc.tipo.ToList();
                raza = dc.raza.ToList();
                estado = dc.estado.ToList();
            }

            WebGrid grid = new WebGrid(source: animal, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
                columns: grid.Columns(
                        grid.Column("id", "ID"),
                        grid.Column("codigo_sag", "Codigo de SAG"),
                        grid.Column("sexo", "Sexo"),
                        grid.Column("fec_nac", "Nacimiento"),
                        grid.Column("fecha_ing", "Ingreso"),
                        grid.Column("tipo.nombre", "Tipo"),
                        grid.Column("raza.nombre", "Raza"),
                        grid.Column("estado.nombre", "Estado")
                        )
                    ).ToString();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=AnimalesFundo.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();
        }
        public void GetExcel2()
        {
            List<animal> animal = new List<animal>();

            List<tipo> tipo = new List<tipo>();
            List<raza> raza = new List<raza>();
            List<estado> estado = new List<estado>();
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {
                animal = dc.animal.ToList();
                tipo = dc.tipo.ToList();
                raza = dc.raza.ToList();
                estado = dc.estado.ToList();
            }

            WebGrid grid = new WebGrid(source: animal, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
                columns: grid.Columns(
                        grid.Column("id", "ID"),
                        grid.Column("codigo_sag", "Codigo de SAG"),
                        grid.Column("sexo", "Sexo"),
                        grid.Column("fec_nac", "Nacimiento"),
                        grid.Column("fecha_ing", "Ingreso"),
                        grid.Column("tipo.nombre", "Tipo"),
                        grid.Column("raza.nombre", "Raza"),
                        grid.Column("estado.nombre", "Estado")
                        )
                    ).ToString();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Animales.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();
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