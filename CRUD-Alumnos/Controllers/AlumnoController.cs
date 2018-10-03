using CRUD_Alumnos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUD_Alumnos.Controllers
{
    public class AlumnoController : Controller
    {
        // GET: Alumno
        public ActionResult Index()
        {
            try
            {
                //int edad = 17;
                //string sql = @"
                //             select a.Id, a.Nombres, a.Apellidos, a.Edad, a.Sexo, a.FechaRegistro, c.Nombre as NombreCiudad
                //             from Alumno a
                //             inner join Ciudad c on a.CodCiudad = c.Id
                //             where a.Edad > @edad";
                using (var db = new AlumnosContext())
                {
                    var data = from a in db.Alumno
                               join c in db.Ciudad on a.CodCiudad equals c.Id
                               select new AlumnoCE()
                               {
                                   Id = a.Id,
                                   Nombres = a.Nombres,
                                   Apellidos = a.Apellidos,
                                   Edad = a.Edad,
                                   Sexo = a.Sexo,
                                   NombreCiudad = c.Nombre,
                                   FechaRegistro = a.FechaRegistro
                               };
                    //List<Alumno> lista = db.Alumno.Where(a => a.Edad > 18).ToList();
                    return View(data.ToList());
                    //return View(db.Database.SqlQuery<AlumnoCE>(sql, new SqlParameter("@edad", edad)).ToList());
                    //return View(db.Alumno.ToList());
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            //AlumnosContext db = new AlumnosContext();

            
        }

        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Alumno a)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                //using (AlumnosContext db = new AlumnosContext())
                using (var db = new AlumnosContext())
                {
                    a.FechaRegistro = DateTime.Now;
                    db.Alumno.Add(a);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Error al registrar Alumno - "+ ex.Message);
                return View();
            }
            
        }

        public ActionResult Agregar2()
        {
            return View();
        }

        public ActionResult ListaCiudades()
        {
            using (var db = new AlumnosContext())
            {
                return PartialView(db.Ciudad.ToList());
            }
        }


        public ActionResult Editar(int id)
        {
            try
            {
                using (var db = new AlumnosContext())
                {
                    //Alumno al = db.Alumno.Where(a => a.Id == id).FirstOrDefault();
                    Alumno alu = db.Alumno.Find(id);
                    return View(alu);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
                
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Alumno a)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();
                               
                using (var db = new AlumnosContext())
                {
                    Alumno al = db.Alumno.Find(a.Id);
                    al.Nombres = a.Nombres;
                    al.Apellidos = a.Apellidos;
                    al.Edad = a.Edad;
                    al.Sexo = a.Sexo;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                    
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult DetalleAlumno(int id)
        {
            using (var db = new AlumnosContext())
            {                
                Alumno alu = db.Alumno.Find(id);
                return View(alu);
            }
        }

        public ActionResult ElimiarAlumno(int id)
        {
            using (var db = new AlumnosContext())
            {
                Alumno alu = db.Alumno.Find(id);
                db.Alumno.Remove(alu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public static string NombreCiudad(int CodCiudad)
        {
            using (var db = new AlumnosContext())
            {
                return db.Ciudad.Find(CodCiudad).Nombre;
            }
        }
        
    }
    
}