using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using SistemaGestionReservas.Models;
using Dapper;
using System;

namespace SistemaGestionReservas.Repositories
{
    public class ReservaRepositorio
    {
        private readonly string _connectionString;

        public ReservaRepositorio(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Reserva> ObtenerReservas()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var reservas = connection.Query<Reserva>(
                    "spConsultarReservas",
                    commandType: CommandType.StoredProcedure
                );

                return reservas;
            }
        }

        public void AgregarReserva(Reserva reserva)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    reserva.SalaID,
                    reserva.FechaReserva,
                    reserva.Usuario
                };
                connection.Execute("spAgregarReserva", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void EditarReserva(Reserva reserva)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    reserva.ID,
                    reserva.SalaID,
                    reserva.FechaReserva,
                    reserva.Usuario
                };
                connection.Execute("spEditarReserva", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void EliminarReserva(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("spEliminarReserva", new { ID = id }, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<Reserva> FiltrarReservas(DateTime? fechaInicio, DateTime? fechaFin, int? salaId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    SalaID = salaId
                };

                var reservas = connection.Query<Reserva>(
                    "spFiltrarReservas",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return reservas;
            }
        }


    }
}