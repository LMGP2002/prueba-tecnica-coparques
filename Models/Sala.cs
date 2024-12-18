using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGestionReservas.Models
{
    public class Sala
    {
        public int ID { get; set; } // ID autogenerado
        public string Nombre { get; set; } // Nombre de la sala
        public int Capacidad { get; set; } // Número máximo de personas
        public bool Disponibilidad { get; set; } 
    }
}