using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGestionReservas.Models
{
    public class Reserva
    {
        public int ID { get; set; }
        public int SalaID { get; set; }
        public DateTime FechaReserva { get; set; }
        public string Usuario { get; set; }

        // Propiedad auxiliar que permitirá recibir el nombre de la sala desde la base de datos
        public string NombreSala { get; set; }
    }
}