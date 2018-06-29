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
using ProyectoV1.Models;
using PagedList.Mvc;
namespace ProyectoV1.Controllers
{
    public class UsuarioController : Controller
    {
        private bdagricolaEntities db = new bdagricolaEntities();
        public JsonResult LoginAndroid(string rut, string pass)
        {
            usuario logeado = new usuario();
            if (rut != null && pass != null)
            {
                try
                {
                    usuario user = db.usuario.Where(x => x.rut == rut).FirstOrDefault();
                    if (user != null)
                    {
                        bool resultadoComparacion = PasswordHash.PasswordHash.ValidatePassword(pass, user.clave);
                        if(resultadoComparacion==true)
                        {
                            logeado.id = user.id;
                            logeado.rut = user.rut;
                        }
                    }
                }
                catch (Exception e)
                {

                }

            }





            return Json(logeado, JsonRequestBehavior.AllowGet);
        }
        private List<alerta> alertas = new List<alerta>();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(usuario us)
        {
            // this action is for handle post (login)

            using (bdagricolaEntities db = new bdagricolaEntities())
            {
                if (us.rut == "admin" && us.clave == "admin")
                {
                    List<usuario> x = db.usuario.Where(a => a.rol.Equals("Administrador")).ToList();
                    if (x.Count == 0)
                    {
                        return RedirectToAction("CrearAdministrador");
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        usuario user = db.usuario.Where(x => x.rut == us.rut).FirstOrDefault();

                        if (user != null)
                        {
                            bool resultadoComparacion = PasswordHash.PasswordHash.ValidatePassword(us.clave, user.clave);
                            if (resultadoComparacion == true)
                            {
                                if (user.rol == "Usuario")
                                {
                                    Session["rut"] = user.rut;
                                    Session["id"] = user.id;
                                    Session["rol"] = user.rol;
                                    return RedirectToAction("Index");
                                }
                                else if (user.rol == "Administrador")
                                {
                                    Session["rut"] = user.rut;
                                    Session["id"] = user.id;
                                    Session["rol"] = user.rol;
                                    return RedirectToAction("InicioAdministrador");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(String.Empty, "Rut o contraseña no Encontrado en el Sistema");
                            }


                        }
                        else
                        {
                            ModelState.AddModelError(String.Empty, "Rut o contraseña no Encontrado en el Sistema");
                        }
                    }
                    else
                    {
                        return View(us);
                    }

                }

                return View(us);
            }
        }

                
            
            




        

        // GET: Usuario
        public ActionResult Index()
        {


            using (bdagricolaEntities dc = new bdagricolaEntities())
            {

                DateTime mañana = DateTime.Today.AddDays(1);
                int alertaHoy = db.tratamiento.Where(m => m.fecha.Equals(mañana)).Count();
                if (alertaHoy >= 1)
                {
                    TempData["notice"] = "Existen alertas para este proximo dia, por favor revisalas";
                }

            }
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {

                DateTime mañana = DateTime.Today.AddDays(1);
                int alertaHoy = db.alerta_mantencion.Where(m => m.fecha.Equals(mañana)).Count();
                if (alertaHoy >= 1)
                {
                    TempData["notice"] = "Existen alertas para este proximo dia, por favor revisalas";
                }

            }
            using (bdagricolaEntities dc = new bdagricolaEntities())
            {

                DateTime mañana = DateTime.Today.AddDays(1);
                int alertaHoy = db.tratamiento_animal.Where(m => m.fecha.Equals(mañana)).Count();
                if (alertaHoy >= 1)
                {
                    TempData["notice"] = "Existen alertas para este proximo dia, por favor revisalas";
                }

            }

            //TempData["notice"] = "Recuerda que hoy Tienes un tratamiento";


            return View();
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario usuario = db.usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,rut,clave,rol")] usuario usuario)
        {
            bool f = false;
            var rut = db.usuario.Select(a => a.rut);
            foreach(var a in rut)
            {
                
                if (a == usuario.rut)
                {
                    ViewBag.Error = "Rut ya Existe";
                    f = true;
                }


            }
            
            if(f == true)
            {
                return View(usuario);
            }
            else
            {
                if (usuario.rol == null)
                {
                    ViewBag.Error = "Rol no puede ser nulo";
                }
                else
                {
                    var encriptar = usuario.clave;
                    if (encriptar != null)
                    {
                        var Encriptada = PasswordHash.PasswordHash.CreateHash(encriptar);
                        usuario.clave = Encriptada;

                    }

                    ModelState.Clear();
                    TryValidateModel(usuario);
                    if (ModelState.IsValid)
                    {
                        db.usuario.Add(usuario);
                        db.SaveChanges();
                        return RedirectToAction("InicioAdministrador");
                    }
                }
                
            }
            
            return View(usuario);
        }


            
        
        public ActionResult InicioAdministrador(int? page)
        {
            var usuarios = db.usuario.ToList();

            return View(usuarios.ToPagedList(page ?? 1, 5));
        }
        public ActionResult InicioAdmin()
        {
            return View(db.usuario.ToList());
        }

        public ActionResult CrearAdministrador()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearAdministrador([Bind(Include = "id,rut,clave,rol")]usuario us)
        {
            var encriptar = us.clave;
            if (encriptar != null)
            {
                var Encriptada = PasswordHash.PasswordHash.CreateHash(encriptar);
                us.clave = Encriptada;
            }
            us.rol = "Administrador";
            ModelState.Clear();
            TryValidateModel(us);
            if (ModelState.IsValid)
            {
                db.usuario.Add(us);
                db.SaveChanges();
                return RedirectToAction("InicioAdministrador");
            }
            return View(us);
        }
        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario usuario = db.usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,rut,clave,rol")] usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var encriptar = usuario.clave;
                if (encriptar != null)
                {
                    var Encriptada = PasswordHash.PasswordHash.CreateHash(encriptar);
                    usuario.clave = Encriptada;
                    
                }
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("InicioAdministrador");
            }
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario usuario = db.usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            usuario usuario = db.usuario.Find(id);
            try
            {
                db.usuario.Remove(usuario);
                db.SaveChanges();
                if (usuario.rol == "Administrador")
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    return RedirectToAction("InicioAdministrador");
                }
            }
            catch
            {
                ViewBag.Error = "No se puede eliminar debido a que existen datos asociados";
            }
            return View(usuario);
            
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
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
