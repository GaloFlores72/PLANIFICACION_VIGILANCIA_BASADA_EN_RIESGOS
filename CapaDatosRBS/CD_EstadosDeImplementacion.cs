using CapaModeloRBS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatosRBS
{
   public class CD_EstadosDeImplementacion
    {
        public static CD_EstadosDeImplementacion _instancia = null;

        private CD_EstadosDeImplementacion()
        {

        }

        public static CD_EstadosDeImplementacion Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_EstadosDeImplementacion();
                }
                return _instancia;
            }
        }



        public List<tbEstadoDeImplementacion> ObtenerEstadoDeImplementacion()
        {
            var listas = new List<tbEstadoDeImplementacion>();

            using (var connection = new SqlConnection(ConexionSqlServer.CN))
            {
                var command = new SqlCommand("SELECT * FROM EstadosDeImplementacion", connection);                
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listas.Add(new tbEstadoDeImplementacion
                        {
                           EstadoID = reader.GetInt32(reader.GetOrdinal("EstadoID")),
                            Descripcion = reader.GetString(reader.GetOrdinal("Descripcion"))
                        });
                    }
                }
            }

            return listas;
        }

        public tbEstadoDeImplementacion ObtenerEstadoDeImplementacionPorEstadoId(int EstadoID)
        {
            tbEstadoDeImplementacion oEstadoDeImplementacion = new tbEstadoDeImplementacion();

            using (var connection = new SqlConnection(ConexionSqlServer.CN))
            {
                var command = new SqlCommand("SELECT * FROM EstadosDeImplementacion where EstadoID = @EstadoID", connection);
                command.Parameters.AddWithValue("@EstadoID", EstadoID);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        oEstadoDeImplementacion.EstadoID = reader.GetInt32(reader.GetOrdinal("EstadoID"));
                        oEstadoDeImplementacion.Descripcion = reader.GetString(reader.GetOrdinal("Descripcion"));
                    }
                }
            }

            return oEstadoDeImplementacion;
        }

    }
}
