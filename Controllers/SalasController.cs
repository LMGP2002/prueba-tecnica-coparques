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
            // Lee la cadena de conexión desde web.config
            _repositorio = new SalaRepositorio(
                System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString
            );
        }

        // GET: Salas
        public ActionResult Index()
        {
            // Obtener todas las salas para la vista index
            var salas = _repositorio.ObtenerSalas();  // Sin filtro, obtiene todas las salas
            return View(salas);
        }

        // GET: Salas/Crear
        public ActionResult Crear()
        {
            // Aquí pasamos '1' para obtener solo las salas activas
            var salasActivas = _repositorio.ObtenerSalas(1);  // '1' representa salas activas
            ViewBag.Salas = salasActivas;  // Pasamos las salas activas a la vista

            return View();  // Retorna la vista de creación
        }




        // POST: Salas/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Sala sala)
        {
            if (ModelState.IsValid)
            {
                _repositorio.AgregarSala(sala); // Llama al repositorio para agregar la sala
                return RedirectToAction("Index");
            }
            return View(sala);
        }

        // GET: Salas/Editar/5
        public ActionResult Editar(int id)
        {
            var sala = _repositorio.ObtenerSalas().FirstOrDefault(s => s.ID == id); // Busca la sala por ID
            if (sala == null)
                return HttpNotFound();
            return View(sala);
        }

        // POST: Salas/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Sala sala)
        {
            if (ModelState.IsValid)
            {
                _repositorio.EditarSala(sala); // Llama al repositorio para editar la sala
                return RedirectToAction("Index");
            }
            return View(sala);
        }

        // GET: Salas/Eliminar/5
        public ActionResult Eliminar(int id)
        {
            var sala = _repositorio.ObtenerSalas().FirstOrDefault(s => s.ID == id);
            if (sala == null)
                return HttpNotFound();
            return View(sala);
        }

        // POST: Salas/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmado(int id)
        {
            _repositorio.EliminarSala(id); // Llama al repositorio para eliminar la sala
            return RedirectToAction("Index");
        }

        // API: Salas/EliminarSalaJson (con respuesta JSON para eliminación dinámica)
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
