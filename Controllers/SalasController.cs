using System;
using System.Linq;
using System.Web.Mvc;
using SistemaGestionReservas.Models;
using SistemaGestionReservas.Repositories;

namespace SistemaGestionReservas.Controllers
{
    public class SalasController : Controller
    {
        private readonly SalaRepositorio _repositorio;

        public SalasController()
        {
            _repositorio = new SalaRepositorio(
                System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString
            );
        }

        // GET: Salas
        public ActionResult Index()
        {
            // Obtener todas las salas para la vista index
            var salas = _repositorio.ObtenerSalas(); 
            return View(salas);
        }

        // GET: Salas/Crear
        public ActionResult Crear()
        {

            var salasActivas = _repositorio.ObtenerSalas(1); 
            ViewBag.Salas = salasActivas;  

            return View(); 
        }




        // POST: Salas/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Sala sala)
        {
            if (ModelState.IsValid)
            {
                _repositorio.AgregarSala(sala); 
                return RedirectToAction("Index");
            }
            return View(sala);
        }

        // GET: Salas/Editar/
        public ActionResult Editar(int id)
        {
            var sala = _repositorio.ObtenerSalas().FirstOrDefault(s => s.ID == id);
            if (sala == null)
                return HttpNotFound();
            return View(sala);
        }

        // POST: Salas/Editar/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Sala sala)
        {
            if (ModelState.IsValid)
            {
                _repositorio.EditarSala(sala); 
                return RedirectToAction("Index");
            }
            return View(sala);
        }

        // GET: Salas/Eliminar/
        public ActionResult Eliminar(int id)
        {
            var sala = _repositorio.ObtenerSalas().FirstOrDefault(s => s.ID == id);
            if (sala == null)
                return HttpNotFound();
            return View(sala);
        }

        // POST: Salas/Eliminar/
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmado(int id)
        {
            _repositorio.EliminarSala(id); 
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult EliminarSalaJson(int id)
        {
            try
            {
                if (_repositorio.TieneReservas(id))
                {
                    return Json(new
                    {
                        success = false,
                        message = "No se puede eliminar la sala porque tiene reservas asociadas."
                    });
                }

                _repositorio.EliminarSala(id);
                return Json(new
                {
                    success = true,
                    message = "Sala eliminada correctamente."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Ocurrió un error al intentar eliminar la sala: " + ex.Message
                });
            }
        }

    }
}
