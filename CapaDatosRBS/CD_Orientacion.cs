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
using System.Xml.Serialization;

namespace CapaDatosRBS
{
    public class CD_Orientacion
    {
        public static CD_Orientacion _instancia = null;

        private CD_Orientacion()
        {

        }

        public static CD_Orientacion Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_Orientacion();
                }
                return _instancia;
            }
        }

        public List<tbOrientacion> ObtenerOrientacion()
        {
            List<tbOrientacion> rptOrientacion = new List<tbOrientacion>();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerOrientaciones", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        rptOrientacion.Add(new tbOrientacion()
                        {
                            OrientacionID = reader.GetInt32(reader.GetOrdinal("OrientacionID")),
                            PreguntaID = reader.GetInt32(reader.GetOrdinal("PreguntaID")),
                            CodigoPeligro = reader.GetString(reader.GetOrdinal("CodigoPeligro")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                            CodigoOrientacion = reader.IsDBNull(reader.GetOrdinal("CodigoOrientacion")) ? null : reader.GetString(reader.GetOrdinal("CodigoOrientacion"))
                        });
                    }
                    reader.Close();

                    return rptOrientacion;
                }
                catch (Exception ex)
                {
                    rptOrientacion = null;
                    return rptOrientacion;
                }
            }
        }

        public List<tbOrientacion> ObtenerOrientacionesPorPreguntasPorIdPregunta(int PreguntaID)
        {
            var preguntas = new List<tbOrientacion>();
            string query = " SELECT ori.OrientacionID, ori.PreguntaID, ori.CodigoPeligro, ori.Nombre, ori.Descripcion "
                + " FROM Orientaciones ori "
                + " inner join Preguntas pr on(pr.PreguntaID = ori.PreguntaID)"
                + " inner join Subtitulos st on(pr.SubtituloID = st.SubtituloID) "
                + " inner join ListasDeVerificacion lv on(st.ListaID = lv.ListaID) "
                + " where ori.PreguntaID = @PreguntaID";
            using (var connection = new SqlConnection(ConexionSqlServer.CN))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PreguntaID", PreguntaID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        preguntas.Add(new tbOrientacion
                        {
                            OrientacionID = reader.GetInt32(reader.GetOrdinal("OrientacionID")),
                            PreguntaID = reader.GetInt32(reader.GetOrdinal("PreguntaID")),
                            CodigoPeligro = reader.IsDBNull(reader.GetOrdinal("CodigoPeligro")) ? null : reader.GetString(reader.GetOrdinal("CodigoPeligro")),
                            Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? null : reader.GetString(reader.GetOrdinal("Nombre")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion"))
                        });
                    }
                }
            }

            return preguntas;
        }

        public List<tbOrientacion> ObtenerOrientacionesPorIdPregunta(int PreguntaID)
        {
            if (PreguntaID <= 0)
            {
                throw new ArgumentException("El ID de la pregunta no es válido.");
            }

            var preguntas = new List<tbOrientacion>();
            string query = "SELECT ori.OrientacionID, ori.PreguntaID, ori.CodigoPeligro, ori.Nombre, ori.Descripcion, ori.CodigoOrientacion "
                           + "FROM Orientaciones ori "
                           + "INNER JOIN Preguntas pr ON(pr.PreguntaID = ori.PreguntaID) "
                           + "INNER JOIN Subtitulos st ON(pr.SubtituloID = st.SubtituloID) "
                           + "INNER JOIN ListasDeVerificacion lv ON(st.ListaID = lv.ListaID) "
                           + "WHERE ori.PreguntaID = @PreguntaID";

            try
            {
                using (var connection = new SqlConnection(ConexionSqlServer.CN))
                {
                    var command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PreguntaID", PreguntaID);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            preguntas.Add(new tbOrientacion
                            {
                                OrientacionID = reader.GetInt32(reader.GetOrdinal("OrientacionID")),
                                PreguntaID = reader.GetInt32(reader.GetOrdinal("PreguntaID")),
                                CodigoPeligro = reader.IsDBNull(reader.GetOrdinal("CodigoPeligro")) ? null : reader.GetString(reader.GetOrdinal("CodigoPeligro")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? null : reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                                CodigoOrientacion = reader.IsDBNull(reader.GetOrdinal("CodigoOrientacion")) ? null : reader.GetString(reader.GetOrdinal("CodigoOrientacion"))

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener orientaciones: " + ex.Message);
                return new List<tbOrientacion>();
            }

            return preguntas;
        }

        public int RegistrarOrientacion(tbOrientacion orientacion)
        {
            int resultado = 0;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarOrientacion", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PreguntaID", orientacion.PreguntaID);
                    cmd.Parameters.AddWithValue("@CodigoPeligro", orientacion.CodigoPeligro ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Nombre", orientacion.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", orientacion.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoOrientacion", orientacion.CodigoOrientacion);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                }
                catch (Exception ex)
                {
                    resultado = 0;
                    Console.WriteLine("Error al registrar orientación: " + ex.Message);
                }
            }
            return resultado;
        }


        public int ModificarOrientacion(tbOrientacion objeto)
        {
            int resultado = 0;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_ModificarOrientacion", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@OrientacionID", objeto.OrientacionID);
                    cmd.Parameters.AddWithValue("@PreguntaID", objeto.PreguntaID);
                    cmd.Parameters.AddWithValue("@CodigoPeligro", objeto.CodigoPeligro);
                    cmd.Parameters.AddWithValue("@Nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", objeto.Descripcion);
                    cmd.Parameters.AddWithValue("@CodigoOrientacion", objeto.CodigoOrientacion);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                }
                catch (Exception ex)
                {
                    resultado = 0; // Error
                }
            }
            return resultado;
        }


        public bool EliminarOrientacion(int orientacionID)
        {
            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_EliminarOrientacion", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@OrientacionID", orientacionID);
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

        public tbOrientacion ObtieneOrientacionConstatacionesPororientacionId(int orientacionID)
        {
            tbOrientacion oOrientacion = new tbOrientacion();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerOrientacionConstantacionPorOrientacionIdXml", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrientacionId", orientacionID);
                try
                {
                    oConexion.Open();
                    using (XmlReader reader = cmd.ExecuteXmlReader())
                    {
                        if (reader.Read())
                        {
                            XDocument doc = XDocument.Load(reader);

                            if (doc.Element("Orientacion") != null)
                            {
                                oOrientacion = new tbOrientacion
                                {
                                    OrientacionID = (int)doc.Element("Orientacion")?.Element("OrientacionID"),
                                    PreguntaID = (int)doc.Element("Orientacion")?.Element("PreguntaID"),
                                    CodigoPeligro = (string)doc.Element("Orientacion")?.Element("CodigoPeligro") ?? "",
                                    Nombre = (string)doc.Element("Orientacion")?.Element("Nombre") ?? "",
                                    Descripcion = (string)doc.Element("Orientacion")?.Element("Descripcion") ?? "",
                                };

                                // Validar si existen constataciones antes de leer
                                XElement detalleConstatacion = doc.Element("Orientacion")?.Element("DetalleConstatacion");
                                if (detalleConstatacion != null)
                                {
                                    oOrientacion.oContataciones = (from constatacion in detalleConstatacion.Elements("Constatacion")
                                                                   select new tbConstatacion
                                                                   {
                                                                       ConstatacionID = (int)constatacion.Element("ConstatacionID"),
                                                                       RespuestaOrientacionID = (int)constatacion.Element("RespuestaOrientacionID"),
                                                                       AreaID = (int)constatacion.Element("AreaID"),
                                                                       FechaConstatacion = (string)(constatacion.Element("FechaConstatacion")),
                                                                       PresuntaInfraccion = (bool)constatacion.Element("PresuntaInfraccion"),
                                                                       DescripcionConstatacion = (string)constatacion.Element("DescripcionConstatacion") ?? "",
                                                                       AfectaSO = (bool)constatacion.Element("AfectaSO"),
                                                                       NotaAfectaSO = (string)constatacion.Element("NotaAfectaSO") ?? "",
                                                                       UsuarioCreaID = (int)constatacion.Element("UsuarioCreaID"),
                                                                       FechaCreacion = (DateTime)constatacion.Element("FechaCreacion"),
                                                                       UsuarioModificaID = (int)constatacion.Element("UsuarioModificaID"),
                                                                       FechaModifica = (DateTime)constatacion.Element("FechaModifica"),
                                                                       oArea = (from area in constatacion.Elements("Area")
                                                                                select new tbArea()
                                                                                {
                                                                                    AreaID = (int)area.Element("AreaID"),
                                                                                    OrganizacionID = (int)area.Element("OrganizacionID"),
                                                                                    Nombre = (string)area.Element("Nombre"),
                                                                                    Descripcion = (string)area.Element("Descripcion")
                                                                                }).FirstOrDefault()
                                }).ToList();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {                    
                    return oOrientacion;
                }

                return oOrientacion;
            }
        }

        private DateTime? ConvertirFecha(XElement elemento)
        {
            return DateTime.TryParse((string)elemento, out DateTime fecha) ? fecha : (DateTime?)null;
        }

        public tbOrientacion ObtenerOrientacionPorId(int orientacionID)
        {
            tbOrientacion oOrientacion = null;

            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerOrientacionXml", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrientacionID", orientacionID);

                try
                {
                    oConexion.Open();

                    using (XmlReader dr = cmd.ExecuteXmlReader())
                    {
                        while (dr.Read())
                        {
                            XDocument doc = XDocument.Load(dr);

                            // Paso 3: Validación del XML y visualización
                            if (doc.Root == null)
                            {
                                Console.WriteLine("El XML no tiene raíz. Posiblemente no se devolvió ningún dato.");
                                return null;
                            }

                            Console.WriteLine("XML recibido desde SQL:");
                            Console.WriteLine(doc.ToString());

                            var nodoOrientacion = doc.Root;
                            if (nodoOrientacion != null)
                            {
                                oOrientacion = new tbOrientacion
                                {
                                    OrientacionID = int.Parse(nodoOrientacion.Element("OrientacionID")?.Value ?? "0"),
                                    PreguntaID = int.Parse(nodoOrientacion.Element("PreguntaID")?.Value ?? "0"),
                                    CodigoPeligro = nodoOrientacion.Element("CodigoPeligro")?.Value,
                                    Nombre = nodoOrientacion.Element("Nombre")?.Value,
                                    Descripcion = nodoOrientacion.Element("Descripcion")?.Value,
                                    CodigoOrientacion = nodoOrientacion.Element("CodigoOrientacion")?.Value
                                };

                                var preguntaXML = nodoOrientacion.Element("Pregunta");
                                if (preguntaXML != null)
                                {
                                    oOrientacion.oPregunta = new tbPregunta
                                    {
                                        PreguntaID = int.Parse(preguntaXML.Element("PreguntaID")?.Value ?? "0"),
                                        SubtituloID = int.Parse(preguntaXML.Element("SubtituloID")?.Value ?? "0"),
                                        Descripcion = preguntaXML.Element("Descripcion")?.Value,
                                        Referencia = preguntaXML.Element("Referencia")?.Value,
                                        Estado = preguntaXML.Element("Estado")?.Value,
                                        Estadisticas = int.TryParse(preguntaXML.Element("Estadisticas")?.Value, out int est) ? est : 0,
                                        CodigoPregunta = preguntaXML.Element("CodigoPregunta")?.Value
                                    };

                                    var subtituloXML = preguntaXML.Element("Subtitulo");
                                    if (subtituloXML != null)
                                    {
                                        oOrientacion.oPregunta.oSubtitulo = new tbSubtitulo
                                        {
                                            SubtituloID = int.Parse(subtituloXML.Element("SubtituloID")?.Value ?? "0"),
                                            ListaID = int.Parse(subtituloXML.Element("ListaID")?.Value ?? "0"),
                                            Nombre = subtituloXML.Element("Nombre")?.Value,
                                            Descripcion = subtituloXML.Element("Descripcion")?.Value,
                                            Estado = bool.TryParse(subtituloXML.Element("Estado")?.Value, out bool estadoSub) ? estadoSub : (bool?)null
                                        };

                                        var listaXml = subtituloXML.Element("ListaVerificacion");
                                        if (listaXml != null)
                                        {
                                            oOrientacion.oPregunta.oSubtitulo.oListaVerificacion = new tbListaDeVerificacion
                                            {
                                                ListaID = int.Parse(listaXml.Element("ListaID")?.Value ?? "0"),
                                                Nombre = listaXml.Element("Nombre")?.Value,
                                                Descripcion = listaXml.Element("Descripcion")?.Value,
                                                FechaCreacion = DateTime.Parse(listaXml.Element("FechaCreacion")?.Value),
                                                UsuarioCrea = listaXml.Element("UsuarioCrea")?.Value,
                                                FechaModifica = DateTime.TryParse(listaXml.Element("FechaModifica")?.Value, out DateTime fechaMod) ? fechaMod : (DateTime?)null,
                                                UsuarioModifica = listaXml.Element("UsuarioModifica")?.Value,
                                                Estado = bool.TryParse(listaXml.Element("Estado")?.Value, out bool estadoLista) ? estadoLista : (bool?)null,
                                                AreaID = int.Parse(listaXml.Element("AreaID")?.Value ?? "0"),
                                                IdTipoProveedorServicio = int.TryParse(listaXml.Element("IdTipoProveedorServicio")?.Value, out int tipo) ? tipo : 0
                                            };
                                        }
                                    }
                                }
                            }
                            dr.Close();
                        }

                        return oOrientacion;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error en ObtenerOrientacionPorId: " + ex.Message);
                    return null;
                }
            }
        }

    }
}
