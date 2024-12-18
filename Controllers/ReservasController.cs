using SistemaGestionReservas.Models;
using SistemaGestionReservas.Repositories;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace SistemaGestionReservas.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ReservaRepositorio _repositorioReserva;
        private readonly SalaRepositorio _repositorioSala;

        public ReservasController()
        {
            // Inicializa los repositorios con la conexión configurada en web.config
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _repositorioReserva = new ReservaRepositorio(connectionString);
            _repositorioSala = new SalaRepositorio(connectionString);
        }

        // GET: Reservas
        public ActionResult Index()
        {
            var reservas = _repositorioReserva.ObtenerReservas();
            ViewBag.Salas = _repositorioSala.ObtenerSalas();

            return View(reservas);
        }

        // GET: Reservas/Crear
        public ActionResult Crear()
        {
            // Solo obtiene las salas activas (Disponibilidad = 1)
            ViewBag.Salas = _repositorioSala.ObtenerSalas(1);
            return View();
        }

        // POST: Reservas/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repositorioReserva.AgregarReserva(reserva);
                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 50001)
                    {
                        // Pasa el mensaje de error para mostrarlo en SweetAlert
                        TempData["ErrorMessage"] = ex.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Hubo un error al procesar la solicitud.";
                    }
                }
            }
            ViewBag.Salas = _repositorioSala.ObtenerSalas();
            return View(reserva);
        }

        // GET: Reservas/Editar/
        public ActionResult Editar(int id)
        {
            var reserva = _repositorioReserva.ObtenerReservas().FirstOrDefault(r => r.ID == id);
            if (reserva == null)
                return HttpNotFound();

            ViewBag.Salas = _repositorioSala.ObtenerSalas(1); 
            return View(reserva);
        }

        // POST: Reservas/Editar/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                _repositorioReserva.EditarReserva(reserva);
                return RedirectToAction("Index");
            }
            ViewBag.Salas = _repositorioSala.ObtenerSalas(1);
            return View(reserva);
        }

        // POST: Reservas/Eliminar
        [HttpPost]
        public JsonResult EliminarReservaJson(int id)
        {
            try
            {
                _repositorioReserva.EliminarReserva(id);
                return Json(new { success = true, message = "Reserva eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar la reserva: " + ex.Message });
            }
        }

        public ActionResult Filtrar(DateTime? fechaInicio, DateTime? fechaFin, int? salaId)
        {
            var reservas = _repositorioReserva.FiltrarReservas(fechaInicio, fechaFin, salaId);
            ViewBag.Salas = _repositorioSala.ObtenerSalas();
            return View("Index", reservas);
        }

        public PartialViewResult FiltrarJson(DateTime? fechaInicio, DateTime? fechaFin, int? salaId)
        {
            var reservas = _repositorioReserva.FiltrarReservas(fechaInicio, fechaFin, salaId);
            return PartialView("_ListaReservas", reservas);
        }





    }
}