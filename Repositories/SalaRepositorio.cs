using System.Data.SqlClient;
using System.Data;
using SistemaGestionReservas.Models;
using Dapper;
using System.Collections.Generic;

namespace SistemaGestionReservas.Repositories
{
    public class SalaRepositorio
    {
        private readonly string _connectionString;

        public SalaRepositorio(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Sala> ObtenerSalas(int? estado = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new { Estado = estado };  // Estado puede ser 0, 1 o NULL
                var salas = connection.Query<Sala>(
                    "spConsultarSalas",  // Procedimiento almacenado modificado
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return salas;
            }
        }



        public void AgregarSala(Sala sala)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    sala.Nombre,
                    sala.Capacidad,
                    sala.Disponibilidad
                };
                connection.Execute("spAgregarSala", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void EditarSala(Sala sala)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    sala.ID,
                    sala.Nombre,
                    sala.Capacidad,
                    sala.Disponibilidad
                };
                connection.Execute("spEditarSala", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void EliminarSala(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("spEliminarSala", new { ID = id }, commandType: CommandType.StoredProcedure);
            }
        }

        public bool TieneReservas(int salaId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new { SalaID = salaId };
                int resultado = connection.ExecuteScalar<int>(
                    "spTieneReservas",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return resultado == 1;
            }
        }



    }
}