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
    public class CD_ListaDeVerificacion
    {
        public static CD_ListaDeVerificacion _instancia = null;

        private CD_ListaDeVerificacion()
        {

        }

        public static CD_ListaDeVerificacion Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_ListaDeVerificacion();
                }
                return _instancia;
            }
        }

        public List<tbListaDeVerificacion> ObtenerListas()
        {
            List<tbListaDeVerificacion> rptListaVerificacion = new List<tbListaDeVerificacion>();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerListaVerificacionTodos", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        rptListaVerificacion.Add(new tbListaDeVerificacion()
                        {
                            ListaID = reader.GetInt32(reader.GetOrdinal("ListaID")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                            Estado = reader.GetBoolean(reader.GetOrdinal("Estado")),
                            IdTipoProveedorServicio = reader.GetInt32(reader.GetOrdinal("IdTipoProveedor")),
                            DescripcionTipoProveedor = reader.GetString(reader.GetOrdinal("DescripcionTipoProveedor"))

                        });
                    }
                    reader.Close();

                    return rptListaVerificacion;

                }
                catch (Exception ex)
                {
                    rptListaVerificacion = null;
                    return rptListaVerificacion;
                }
            }
        }

        public tbListaDeVerificacion ObtenerListaVerificacionPorOidXml(int listaID)
        {
            tbListaDeVerificacion olistaVerificacion = new tbListaDeVerificacion();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerListaVerificacionXml", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ListaID", listaID);
                try
                {
                    oConexion.Open();
                    using (XmlReader dr = cmd.ExecuteXmlReader())
                    {
                        while (dr.Read())
                        {
                            XDocument doc = XDocument.Load(dr);
                            if (doc.Element("ListaVerificacion") != null)
                            {
                                olistaVerificacion = (from dato in doc.Elements("ListaVerificacion")
                                                      select new tbListaDeVerificacion()
                                                      {
                                                          ListaID = int.Parse(dato.Element("ListaID").Value),
                                                          Nombre = dato.Element("Nombre").Value,
                                                          Descripcion = dato.Element("Descripcion").Value,
                                                          Estado = (dato.Element("Estado").Value.ToString() == "1" ? true : false),
                                                          UsuarioCrea = dato.Element("UsuarioCrea").Value,
                                                          FechaCreacion = DateTime.Parse(dato.Element("FechaCreacion").Value),
                                                          UsuarioModifica = dato.Element("UsuarioModifica").Value,
                                                          FechaModifica = DateTime.Parse(dato.Element("FechaModifica").Value)
                                                      }).FirstOrDefault();
                                olistaVerificacion.oSubtitulos = (from subtitulo in doc.Element("ListaVerificacion").Element("DetalleSubtitulo").Elements("Subtitulo")
                                                                  select new tbSubtitulo()
                                                                  {
                                                                      SubtituloID = int.Parse(subtitulo.Element("SubtituloID").Value),
                                                                      ListaID = int.Parse(subtitulo.Element("ListaID").Value),
                                                                      Nombre = subtitulo.Element("Nombre").Value,
                                                                      Descripcion = subtitulo.Element("Descripcion").Value,
                                                                      oListaPreguntas = (from pregunta in subtitulo.Element("DetallePreguntas").Elements("Pregunta")
                                                                                         select new tbPregunta()
                                                                                         {
                                                                                             PreguntaID = int.Parse(pregunta.Element("PreguntaID").Value),
                                                                                             SubtituloID = int.Parse(pregunta.Element("SubtituloID").Value),
                                                                                             Descripcion = pregunta.Element("Descripcion").Value,
                                                                                             Referencia = pregunta.Element("Referencia").Value,
                                                                                             Estado = pregunta.Element("Estado").Value,
                                                                                             Estadisticas = int.Parse(pregunta.Element("Estadisticas").Value),
                                                                                             oListaOrientaciones = (from orientacion in pregunta.Element("DetalleOrientaciones").Elements("Orientacion")
                                                                                                                    select new tbOrientacion()
                                                                                                                    {
                                                                                                                        OrientacionID = int.Parse(orientacion.Element("OrientacionID").Value),
                                                                                                                        PreguntaID = int.Parse(orientacion.Element("PreguntaID").Value),
                                                                                                                        CodigoPeligro = orientacion.Element("CodigoPeligro").Value,
                                                                                                                        Nombre = orientacion.Element("Nombre").Value,
                                                                                                                        Descripcion = orientacion.Element("Descripcion").Value,
                                                                                                                        oOrientacionesEstados = (from orientacionEstado in orientacion.Element("DetalleOrientacionesEstados").Elements("OrientacionEstado")
                                                                                                                                                 select new tbOrientacionEstado()
                                                                                                                                                 {
                                                                                                                                                     OrientacionEstadoID = int.Parse(orientacionEstado.Element("OrientacionEstadoID").Value),
                                                                                                                                                     OrientacionID = int.Parse(orientacionEstado.Element("OrientacionID").Value),
                                                                                                                                                     EstadoID = int.Parse(orientacionEstado.Element("EstadoID").Value),
                                                                                                                                                     Color = orientacionEstado.Element("Color").Value,
                                                                                                                                                     oEstadoDeImplementacion = new tbEstadoDeImplementacion()
                                                                                                                                                     {
                                                                                                                                                         EstadoID = int.Parse(orientacionEstado.Element("EstadoID").Value),
                                                                                                                                                         Descripcion = orientacionEstado.Element("Descripcion").Value
                                                                                                                                                     }
                                                                                                                                                 }).ToList()
                                                                                                                    }
                                                                                                                  ).ToList()

                                                                                         }).ToList()

                                                                  }).ToList();
                            }
                            else
                            {
                                olistaVerificacion = null;
                            }
                        }

                        dr.Close();

                    }

                    return olistaVerificacion;
                }
                catch (Exception ex)
                {
                    olistaVerificacion = null;
                    return olistaVerificacion;
                }
            }
        }

        public tbListaDeVerificacion ObtenerListaVerificacionPorOidTodos(int listaID)
        {
            tbListaDeVerificacion olistaVerificacion = new tbListaDeVerificacion();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerListaVerificacionXml", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ListaID", listaID);
                try
                {
                    oConexion.Open();

                    using (XmlReader dr = cmd.ExecuteXmlReader())
                    {

                        while (dr.Read())
                        {
                            XDocument doc = XDocument.Load(dr);
                            foreach (var dato in doc.Descendants("ListaVerificacion"))
                            {
                                olistaVerificacion.ListaID = int.Parse(dato.Element("ListaID").Value);
                                olistaVerificacion.Nombre = dato.Element("Nombre").Value;
                                olistaVerificacion.Descripcion = dato.Element("Descripcion").Value;
                                olistaVerificacion.Estado = (dato.Element("Estado").Value.ToString() == "1" ? true : false);
                                olistaVerificacion.UsuarioCrea = dato.Element("UsuarioCrea").Value;
                                olistaVerificacion.FechaCreacion = DateTime.Parse(dato.Element("FechaCreacion").Value);
                                olistaVerificacion.UsuarioModifica = dato.Element("UsuarioModifica").Value;
                                olistaVerificacion.FechaModifica = DateTime.Parse(dato.Element("FechaModifica").Value);
                                foreach (var subtitulo in dato.Descendants("Subtitulo"))
                                {
                                    tbSubtitulo osubtitulo = new tbSubtitulo();

                                    osubtitulo.SubtituloID = int.Parse(subtitulo.Element("SubtituloID").Value);
                                    osubtitulo.ListaID = int.Parse(subtitulo.Element("ListaID").Value);
                                    osubtitulo.Nombre = subtitulo.Element("Nombre").Value;
                                    osubtitulo.Descripcion = subtitulo.Element("Descripcion").Value;
                                    foreach (var pregunta in dato.Descendants("Pregunta"))
                                    {
                                        if (osubtitulo.SubtituloID == int.Parse(pregunta.Element("SubtituloID").Value))
                                        {
                                            tbPregunta opregunta = new tbPregunta();
                                            opregunta.PreguntaID = int.Parse(pregunta.Element("PreguntaID").Value);
                                            opregunta.SubtituloID = int.Parse(pregunta.Element("SubtituloID").Value);
                                            opregunta.Descripcion = pregunta.Element("Descripcion").Value;
                                            opregunta.Referencia = pregunta.Element("Referencia").Value;
                                            opregunta.Estado = pregunta.Element("Estado").Value;
                                            opregunta.Estadisticas = int.Parse(pregunta.Element("Estadisticas").Value);
                                            foreach (var orientacion in pregunta.Descendants("Orientacion"))
                                            {
                                                if (opregunta.PreguntaID == int.Parse(orientacion.Element("PreguntaID").Value))
                                                {
                                                    tbOrientacion oOrientacion = new tbOrientacion();
                                                    oOrientacion.OrientacionID = int.Parse(orientacion.Element("OrientacionID").Value);
                                                    oOrientacion.PreguntaID = int.Parse(orientacion.Element("PreguntaID").Value);
                                                    oOrientacion.CodigoPeligro = orientacion.Element("CodigoPeligro").Value;
                                                    oOrientacion.Nombre = orientacion.Element("Nombre").Value;
                                                    oOrientacion.Descripcion = orientacion.Element("Descripcion").Value;
                                                    foreach (var orientacionEstado in orientacion.Descendants("OrientacionEstado"))
                                                    {
                                                        if (oOrientacion.OrientacionID == int.Parse(orientacionEstado.Element("OrientacionID").Value))
                                                        {
                                                            tbOrientacionEstado oOrientEstado = new tbOrientacionEstado();
                                                            oOrientEstado.OrientacionEstadoID = int.Parse(orientacionEstado.Element("OrientacionEstadoID").Value);
                                                            oOrientEstado.OrientacionID = int.Parse(orientacionEstado.Element("OrientacionID").Value);
                                                            oOrientEstado.EstadoID = int.Parse(orientacionEstado.Element("EstadoID").Value);
                                                            oOrientEstado.Color = orientacionEstado.Element("Color").Value;
                                                            oOrientEstado.oEstadoDeImplementacion = new tbEstadoDeImplementacion()
                                                            {
                                                                EstadoID = int.Parse(orientacionEstado.Element("EstadoID").Value),
                                                                Descripcion = orientacionEstado.Element("Descripcion").Value
                                                            };
                                                            oOrientacion.oOrientacionesEstados.Add(oOrientEstado);
                                                        }

                                                    }

                                                    opregunta.oListaOrientaciones.Add(oOrientacion);
                                                }

                                            }
                                            osubtitulo.oListaPreguntas.Add(opregunta);
                                        }
                                    }
                                    olistaVerificacion.oSubtitulos.Add(osubtitulo);
                                }


                            }

                            dr.Close();


                        }

                        return olistaVerificacion;
                    }
                }
                catch (Exception ex)
                {
                    olistaVerificacion = null;
                    return olistaVerificacion;
                }
            }


        }

        public tbListaDeVerificacion ObtenerListaVerificacionPorOid(int listaID)
        {
            tbListaDeVerificacion olistaVerificacion = new tbListaDeVerificacion();
            using (var connection = new SqlConnection(ConexionSqlServer.CN))
            {
                var command = new SqlCommand("SELECT * FROM ListasDeVerificacion where ListaID = " + listaID, connection);
                command.Parameters.AddWithValue("@ListaID", listaID);
                connection.Open();
                //,  oSubtitulos = CD_Subtitulos.Instancia.ObtenerSubtitulosPorLista(reader.GetInt32(reader.GetOrdinal("ListaID")))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        olistaVerificacion = new tbListaDeVerificacion
                        {
                            ListaID = reader.GetInt32(reader.GetOrdinal("ListaID")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                            Estado = reader.GetBoolean(reader.GetOrdinal("Estado")),
                            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                            UsuarioCrea = reader.GetString(reader.GetOrdinal("UsuarioCrea")),
                            FechaModifica = reader.IsDBNull(reader.GetOrdinal("FechaModifica")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModifica")),
                            UsuarioModifica = reader.IsDBNull(reader.GetOrdinal("UsuarioModifica")) ? null : reader.GetString(reader.GetOrdinal("UsuarioModifica"))
                        };
                    }
                }
            }

            return olistaVerificacion;
        }

        public tbListaDeVerificacion ObtenerListaVerificacionSubTitulosPorOid(int listaID)
        {
            tbListaDeVerificacion olistaVerificacion = new tbListaDeVerificacion();
            using (var connection = new SqlConnection(ConexionSqlServer.CN))
            {
                var command = new SqlCommand("SELECT * FROM ListasDeVerificacion WHERE ListaID = @ListaID", connection);
                command.Parameters.AddWithValue("@ListaID", listaID);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        olistaVerificacion = new tbListaDeVerificacion
                        {
                            ListaID = reader.GetInt32(reader.GetOrdinal("ListaID")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                            Estado = reader.GetBoolean(reader.GetOrdinal("Estado")),
                            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                            UsuarioCrea = reader.GetString(reader.GetOrdinal("UsuarioCrea")),
                            FechaModifica = reader.IsDBNull(reader.GetOrdinal("FechaModifica")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModifica")),
                            UsuarioModifica = reader.IsDBNull(reader.GetOrdinal("UsuarioModifica")) ? null : reader.GetString(reader.GetOrdinal("UsuarioModifica")),
                            oSubtitulos = CD_Subtitulos.Instancia.ObtenerSubtitulosPorListaId(reader.GetInt32(reader.GetOrdinal("ListaID")))
                        };
                    }
                }
            }

            return olistaVerificacion;
        }
        public List<tbListaDeVerificacion> ObtenerListasPorOid(int listaID)
        {
            var listas = new List<tbListaDeVerificacion>();

            using (var connection = new SqlConnection(ConexionSqlServer.CN))
            {
                var command = new SqlCommand("SELECT * FROM ListasDeVerificacion", connection);
                command.Parameters.AddWithValue("@ListaID", listaID);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listas.Add(new tbListaDeVerificacion
                        {
                            ListaID = reader.GetInt32(reader.GetOrdinal("ListaID")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                            Estado = reader.GetBoolean(reader.GetOrdinal("Estado")),
                            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                            UsuarioCrea = reader.GetString(reader.GetOrdinal("UsuarioCrea")),
                            FechaModifica = reader.IsDBNull(reader.GetOrdinal("FechaModifica")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModifica")),
                            UsuarioModifica = reader.IsDBNull(reader.GetOrdinal("UsuarioModifica")) ? null : reader.GetString(reader.GetOrdinal("UsuarioModifica")),
                            oSubtitulos = CD_Subtitulos.Instancia.ObtenerSubtitulosPorListaId(reader.GetInt32(reader.GetOrdinal("ListaID")))
                        });
                    }
                }
            }

            return listas;
        }

        public bool RegistrarListaVerificacion(tbListaDeVerificacion oListaVerificacion)
        {
            bool respuesta = false; // Cambio de true a false por defecto
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistraListaVerificacion", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar correctamente todos los parámetros requeridos
                    cmd.Parameters.AddWithValue("Nombre", oListaVerificacion.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", oListaVerificacion.Descripcion);
                    cmd.Parameters.AddWithValue("IdTipoProveedorServicio", oListaVerificacion.IdTipoProveedorServicio); // Se agregó este parámetro
                    cmd.Parameters.AddWithValue("UsuarioCrea", oListaVerificacion.UsuarioCrea);
                    cmd.Parameters.AddWithValue("FechaCreacion", oListaVerificacion.FechaCreacion);

                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error en RegistrarListaVerificacion: " + ex.Message);
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool ModificarListaVerificacion(tbListaDeVerificacion oListaVerificacion)
        {
            bool respuesta = false; // Cambiado a false por defecto
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_ModificarListaVerificacion", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Se agregan todos los parámetros
                    cmd.Parameters.AddWithValue("ListaID", oListaVerificacion.ListaID);
                    cmd.Parameters.AddWithValue("Nombre", oListaVerificacion.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", oListaVerificacion.Descripcion);
                    cmd.Parameters.AddWithValue("IdTipoProveedorServicio", oListaVerificacion.IdTipoProveedorServicio); // Agregado
                    cmd.Parameters.AddWithValue("Activo", oListaVerificacion.Estado ?? (object)DBNull.Value); // Manejo de valores nulos
                    cmd.Parameters.AddWithValue("UsuarioModifica", oListaVerificacion.UsuarioModifica);
                    cmd.Parameters.AddWithValue("FechaModifica", oListaVerificacion.FechaModifica);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error en ModificarListaVerificacion: " + ex.Message);
                    respuesta = false;
                }
            }
            return respuesta;
        }


        public bool EliminarListaVerificacion(int ListaID)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_EliminarListaVerificacion", oConexion);
                    cmd.Parameters.AddWithValue("ListaID", ListaID);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

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
