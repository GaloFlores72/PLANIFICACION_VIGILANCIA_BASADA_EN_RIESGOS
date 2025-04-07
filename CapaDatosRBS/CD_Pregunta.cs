using CapaModeloRBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatosRBS
{
    public class CD_Pregunta
    {
        public static CD_Pregunta _instancia = null;

        private CD_Pregunta()
        {

        }

        public static CD_Pregunta Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_Pregunta();
                }
                return _instancia;
            }
        }

        public List<tbPregunta> ObtenerPreguntas()
        {
            List<tbPregunta> rptPregunta = new List<tbPregunta>();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerPreguntasTodos", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        rptPregunta.Add(new tbPregunta()
                        {
                            PreguntaID = reader.GetInt32(reader.GetOrdinal("PreguntaID")),
                            SubtituloID = reader.GetInt32(reader.GetOrdinal("SubtituloID")),
                            Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                            Referencia = reader.GetString(reader.GetOrdinal("Referencia")),
                            Estado = reader.GetString(reader.GetOrdinal("Estado")),
                            Estadisticas = reader.GetInt32(reader.GetOrdinal("Estadisticas"))
                        });
                    }
                    reader.Close();

                    return rptPregunta;
                }
                catch (Exception ex)
                {
                    rptPregunta = null;
                    return rptPregunta;
                }
            }
        }
        public List<tbPregunta> ObtenerPreguntasPorSubtitulo(int subtituloID)
        {
            List<tbPregunta> preguntas = new List<tbPregunta>();

            using (var connection = new SqlConnection(ConexionSqlServer.CN))
            {
                var command = new SqlCommand("SELECT * FROM Preguntas WHERE SubtituloID = @SubtituloID", connection);
                command.Parameters.AddWithValue("@SubtituloID", subtituloID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        preguntas.Add(new tbPregunta
                        {
                            PreguntaID = reader.GetInt32(reader.GetOrdinal("PreguntaID")),
                            SubtituloID = reader.GetInt32(reader.GetOrdinal("SubtituloID")),
                            Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                            Referencia = reader.IsDBNull(reader.GetOrdinal("Referencia")) ? null : reader.GetString(reader.GetOrdinal("Referencia")),
                            Estado = reader.GetString(reader.GetOrdinal("Estado")),
                            Estadisticas = reader.IsDBNull(reader.GetOrdinal("Estadisticas")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Estadisticas"))
                        });
                    }
                }
            }

            return preguntas;
        }

        public bool RegistrarPregunta(tbPregunta opregunta)
        {
            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarPregunta", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SubtituloID", opregunta.SubtituloID);
                    cmd.Parameters.AddWithValue("@Descripcion", opregunta.Descripcion);
                    cmd.Parameters.AddWithValue("@Referencia", opregunta.Referencia);
                    cmd.Parameters.AddWithValue("@Estado", opregunta.Estado);
                    cmd.Parameters.AddWithValue("@Estadisticas", opregunta.Estadisticas);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                }
                catch (Exception ex)
                {
                    respuesta = false;
                    Console.WriteLine(ex.Message);
                }
            }
            return respuesta;
        }

        public bool ModificarPregunta(tbPregunta objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_ModificarPregunta", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PreguntaID", objeto.PreguntaID);
                    cmd.Parameters.AddWithValue("@SubtituloID", objeto.SubtituloID);
                    cmd.Parameters.AddWithValue("@Descripcion", objeto.Descripcion);
                    cmd.Parameters.AddWithValue("@Referencia", objeto.Referencia);
                    cmd.Parameters.AddWithValue("@Estado", objeto.Estado);
                    cmd.Parameters.AddWithValue("@Estadisticas", objeto.Estadisticas);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool EliminarPregunta(int preguntaID)
        {
            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_EliminarPregunta", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PreguntaID", preguntaID);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Console.WriteLine("Resultado de la eliminación: " + respuesta);
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

    }
}

