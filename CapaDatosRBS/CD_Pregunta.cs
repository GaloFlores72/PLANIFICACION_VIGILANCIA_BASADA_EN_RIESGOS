using CapaModeloRBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

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
                            Estadisticas = reader.GetInt32(reader.GetOrdinal("Estadisticas")),
                            CodigoPregunta = reader.IsDBNull(reader.GetOrdinal("CodigoPregunta"))
                                             ? null
                                             : reader.GetString(reader.GetOrdinal("CodigoPregunta"))
                        });
                    }

                    reader.Close();
                    return rptPregunta;
                }
                catch (Exception ex)
                {
                    rptPregunta = null;
                    Console.WriteLine("Error en ObtenerPreguntas: " + ex.Message);
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
                            Estadisticas = reader.IsDBNull(reader.GetOrdinal("Estadisticas")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Estadisticas")),
                            CodigoPregunta = reader["CodigoPregunta"]?.ToString()
                        });
                    }
                }
            }

            return preguntas;
        }

        public int RegistrarPregunta(tbPregunta opregunta)
        {
            int resultado = 0;

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
                    cmd.Parameters.AddWithValue("@CodigoPregunta", opregunta.CodigoPregunta);

                    SqlParameter output = new SqlParameter("@Resultado", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(output);

                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(output.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al registrar pregunta: " + ex.Message);
                    resultado = 0;
                }
            }

            return resultado;
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
                    cmd.Parameters.AddWithValue("@CodigoPregunta", objeto.CodigoPregunta);
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

        public tbPregunta ObtenerPreguntaPorId(int idPregunta)
        {
            tbPregunta oPregunta = null;

            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerPreguntaXml", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PreguntaID", idPregunta);

                try
                {
                    oConexion.Open();

                    using (XmlReader dr = cmd.ExecuteXmlReader())
                    {
                        while (dr.Read())
                        {
                            XDocument doc = XDocument.Load(dr);
                            //Console.WriteLine(doc.ToString()); // Para debug visual

                            var nodoPregunta = doc.Root;
                            if (nodoPregunta != null)
                            {
                                oPregunta = new tbPregunta
                                {
                                    PreguntaID = int.Parse(nodoPregunta.Element("PreguntaID")?.Value ?? "0"),
                                    SubtituloID = int.Parse(nodoPregunta.Element("SubtituloID")?.Value ?? "0"),
                                    Descripcion = nodoPregunta.Element("Descripcion")?.Value,
                                    Referencia = nodoPregunta.Element("Referencia")?.Value,
                                    Estado = nodoPregunta.Element("Estado")?.Value,
                                    Estadisticas = int.TryParse(nodoPregunta.Element("Estadisticas")?.Value, out int est) ? est : 0,
                                    CodigoPregunta = nodoPregunta.Element("CodigoPregunta")?.Value
                                };

                                var subtituloXml = nodoPregunta.Element("Subtitulo");
                                if (subtituloXml != null)
                                {
                                    oPregunta.oSubtitulo = new tbSubtitulo
                                    {
                                        SubtituloID = int.Parse(subtituloXml.Element("SubtituloID")?.Value ?? "0"),
                                        ListaID = int.Parse(subtituloXml.Element("ListaID")?.Value ?? "0"),
                                        Nombre = subtituloXml.Element("Nombre")?.Value,
                                        Descripcion = subtituloXml.Element("Descripcion")?.Value,
                                        Estado = bool.TryParse(subtituloXml.Element("Estado")?.Value, out bool estadoSub) ? estadoSub : (bool?)null
                                    };

                                    var listaXml = subtituloXml.Element("ListaVerificacion");
                                    if (listaXml != null)
                                    {
                                        oPregunta.oSubtitulo.oListaVerificacion = new tbListaDeVerificacion
                                        {
                                            ListaID = int.Parse(listaXml.Element("ListaID")?.Value ?? "0"),
                                            Nombre = listaXml.Element("Nombre")?.Value,
                                            Descripcion = listaXml.Element("Descripcion")?.Value,
                                            FechaCreacion = DateTime.Parse(listaXml.Element("FechaCreacion")?.Value),
                                            UsuarioCrea = listaXml.Element("UsuarioCrea")?.Value,
                                            FechaModifica = DateTime.TryParse(listaXml.Element("FechaModifica")?.Value, out DateTime fechaMod) ? fechaMod : (DateTime?)null,
                                            UsuarioModifica = listaXml.Element("UsuarioModifica")?.Value,
                                            Estado = bool.TryParse(listaXml.Element("Estado")?.Value, out bool estadoLista) ? estadoLista : (bool?)null,
                                            IdTipoProveedorServicio = int.TryParse(listaXml.Element("IdTipoProveedorServicio")?.Value, out int tipo) ? tipo : 0
                                        };
                                    }
                                }
                            }

                            dr.Close();
                        }

                        return oPregunta;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error en ObtenerPreguntaPorId: " + ex.Message);
                    return null;
                }
            }
        }


    }
}

