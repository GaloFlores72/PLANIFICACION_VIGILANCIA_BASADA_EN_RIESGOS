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
    public class CD_Respuesta
    {
        public static CD_Respuesta _instancia = null;

        private CD_Respuesta()
        {

        }

        public static CD_Respuesta Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_Respuesta();
                }
                return _instancia;
            }
        }

        public List<tbRespuesta> ObtenerRespuestaCabeceraTodos()
        {
            List<tbRespuesta> olistaRespuesta = new List<tbRespuesta>();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerRespuestaCabeceraTodos", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@ListaID", listaID);
                try
                {
                    oConexion.Open();

                    using (XmlReader dr = cmd.ExecuteXmlReader())
                    {

                        while (dr.Read())
                        {
                            XDocument doc = XDocument.Load(dr);
                            foreach (var res in doc.Descendants("Respuesta"))
                            {
                                tbRespuesta respuesta = new tbRespuesta
                                {
                                    RespuestaID = (int)res.Element("RespuestaID"),
                                    ListaID = (int)res.Element("ListaID"),
                                    OrganizacionID = (int)res.Element("OrganizacionID"),
                                    UsuarioID = (int)res.Element("UsuarioID"),
                                    Fecha = (DateTime)res.Element("Fecha"),
                                    FechaInicio = (DateTime)res.Element("FechaInicio"),
                                    FechaFin = (DateTime)res.Element("FechaFin"),
                                    InspectorResponsableID = (int)res.Element("InspectorResponsableID"),
                                    UsuarioCrea = (string)res.Element("UsuarioCrea"),
                                    FechaCrea = (DateTime)res.Element("FechaCrea"),
                                    UsuarioModifica = (string)res.Element("UsuarioModifica"),
                                    FechaModifica = (DateTime)res.Element("FechaModifica"),
                                    oInspector = new tbUsuario
                                    {
                                        IdUsuario = (int)res.Element("Usuario").Element("IdUsuario"),
                                        Nombres = (string)res.Element("Usuario").Element("Nombres"),
                                        Apellidos = (string)res.Element("Usuario").Element("Apellidos"),
                                        Correo = (string)res.Element("Usuario").Element("Correo")
                                    },
                                    oOrganizacion = new tbOrganizacion
                                    {
                                        OrganizacionID = (int)res.Element("Organizacion").Element("OrganizacionID"),
                                        Nombre = (string)res.Element("Organizacion").Element("Nombre"),
                                        Direccion = (string)res.Element("Organizacion").Element("Direccion"),
                                        Telefono = (string)res.Element("Organizacion").Element("Telefono"),
                                        Correo = (string)res.Element("Organizacion").Element("Correo"),
                                        GerenteResponsable = (string)res.Element("Organizacion").Element("GerenteResponsable")
                                    }
                                };
                                olistaRespuesta.Add(respuesta);
                            }

                            dr.Close();


                        }

                        return olistaRespuesta;
                    }
                }
                catch (Exception ex)
                {
                    olistaRespuesta = null;
                    return olistaRespuesta;
                }
            }
        }

        public tbRespuesta ObtenerRespuestaPorId(int IdRespuesta)
        {
            tbRespuesta oRespuesta = new tbRespuesta();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerRespuestaPorIdRespuesta", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RespuestaID", IdRespuesta);
                try
                {
                    oConexion.Open();

                    using (XmlReader dr = cmd.ExecuteXmlReader())
                    {

                        while (dr.Read())
                        {
                            XDocument doc = XDocument.Load(dr);
                            if (doc.Element("Respuesta") != null)
                            {
                                oRespuesta = (from dato in doc.Elements("Respuesta")
                                              select new tbRespuesta()
                                              {
                                                  RespuestaID = (int)dato.Element("RespuestaID"),
                                                  ListaID = (int)dato.Element("ListaID"),
                                                  OrganizacionID = (int)dato.Element("OrganizacionID"),
                                                  UsuarioID = (int)dato.Element("UsuarioID"),
                                                  Fecha = (DateTime)dato.Element("Fecha"),
                                                  FechaInicio = (DateTime)dato.Element("FechaInicio"),
                                                  FechaFin = (DateTime)dato.Element("FechaFin"),
                                                  InspectorResponsableID = (int)dato.Element("InspectorResponsableID"),
                                                  UsuarioCrea = (string)dato.Element("UsuarioCrea"),
                                                  FechaCrea = (DateTime)dato.Element("FechaCrea"),
                                                  UsuarioModifica = (string)dato.Element("UsuarioModifica"),
                                                  FechaModifica = (DateTime)dato.Element("FechaModifica"),
                                                  NombreLista = (string)dato.Element("NombreLista"),
                                                  DescripcionLista = (string)dato.Element("DescripcionLista"),
                                              }).FirstOrDefault();
                                oRespuesta.oInspector = (from dato in doc.Element("Respuesta").Elements("oInspector")
                                                         select new tbUsuario()
                                                         {
                                                             IdUsuario = (int)dato.Element("IdUsuario"),
                                                             Nombres = (string)dato.Element("Nombres"),
                                                             Apellidos = (string)dato.Element("Apellidos"),
                                                             Correo = (string)dato.Element("Correo")

                                                         }).FirstOrDefault();
                                oRespuesta.oOrganizacion = (from dato in doc.Element("Respuesta").Elements("Organizacion")
                                                           select new tbOrganizacion()
                                                           {
                                                               OrganizacionID = (int)dato.Element("OrganizacionID"),
                                                               Nombre = (string)dato.Element("Nombre"),
                                                               Direccion = (string)dato.Element("Direccion"),
                                                               Telefono = (string)dato.Element("Telefono"),
                                                               Correo = (string)dato.Element("Correo"),
                                                               GerenteResponsable = (string)dato.Element("GerenteResponsable")
                                                           }).FirstOrDefault();
                                oRespuesta.oTipoProveedorServicio = (from dato in doc.Element("Respuesta").Elements("TipoProveedorServicio")
                                                                     select new tbTipoProveedorServicio()
                                                                     {
                                                                         IdTipoProveedor= (int)dato.Element("IdTipoProveedor"),
                                                                         DescripcionTipoProveedor = (string)dato.Element("DescripcionTipoProveedor")

                                                                     }).FirstOrDefault();
                                oRespuesta.oInpectores = (from inpectores in doc.Element("Respuesta").Element("ListaInspectores").Elements("Inspectores")
                                                          select new tbUsuario()
                                                          {
                                                              IdUsuario = (int)inpectores.Element("IdUsuario"),
                                                              Nombres = (string)inpectores.Element("Nombres"),
                                                              Apellidos = (string)inpectores.Element("Apellidos"),
                                                              Correo = (string)inpectores.Element("Correo")

                                                          }).ToList();
                                oRespuesta.oRespuestaSubtitulo = (from Subtitulo in doc.Element("Respuesta").Element("DetalleSubtitulo").Elements("Subtitulo")
                                                                  select new tbRespuestaSubtitulo()
                                                                  {
                                                                      RespuestaID = oRespuesta.RespuestaID,
                                                                      ListaID = oRespuesta.ListaID,
                                                                      SubtituloID = (int)Subtitulo.Element("SubtituloID"),
                                                                      Nombre = (string)Subtitulo.Element("NombreSubtitulo"),
                                                                      oDetalleRespuesta = (from respuesta in Subtitulo.Element("DetallePreguntas").Elements("Pregunta")
                                                                                           select new tbDetalleRespuesta()
                                                                                           {
                                                                                               DetalleRespuestaID = (int)respuesta.Element("DetalleRespuestaID"),
                                                                                               RespuestaID = (int)respuesta.Element("RespuestaID"),
                                                                                               PreguntaID = (int)respuesta.Element("PreguntaID"),
                                                                                               SubtituloID = (int)respuesta.Element("SubtituloID"),
                                                                                               CodigoPregunta = (string)respuesta.Element("CodigoPregunta"),
                                                                                               DescripcionPregunta = (string)respuesta.Element("DescripcionPregunta"),
                                                                                               ReferenciaPregunta = (string)respuesta.Element("ReferenciaPregunta"),
                                                                                               Estado = (string)respuesta.Element("Estado"),
                                                                                               Color = (string)respuesta.Element("Color"),
                                                                                               oRespuestaOrientacion = (from orientacion in respuesta.Element("DetalleOrientaciones").Elements("Orientacion")
                                                                                                                        select new tbRespuestaOrientacion()
                                                                                                                        {
                                                                                                                            RespuestaOrientacionID = (int)orientacion.Element("RespuestaOrientacionID"),
                                                                                                                            DetalleRespuestaID = (int)orientacion.Element("DetalleRespuestaID"),
                                                                                                                            OrientacionID = (int)orientacion.Element("OrientacionID"),
                                                                                                                            EstadoImplementacionID = (int)orientacion.Element("EstadoImplementacionID"),
                                                                                                                            Color = (string)orientacion.Element("Color"),
                                                                                                                            Comentario = (string)orientacion.Element("Comentario"),
                                                                                                                            CodigoPeligro = (string)orientacion.Element("CodigoPeligro"),
                                                                                                                            DescripcionOrientacion = (string)orientacion.Element("DescripcionOrientacion"),
                                                                                                                            oOrientacionEstado = (from orientacionEstado in orientacion.Element("DetalleOrientacionesEstados").Elements("OrientacionEstado")
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
                                                                                                                        }).ToList()
                                                                                           }).ToList()

                                                                  }).ToList();
                            }
                           
                            dr.Close();


                        }

                        return oRespuesta;
                    }
                }
                catch (Exception ex)
                {
                    oRespuesta = null;
                    return oRespuesta;
                }
            }
        }

        public bool ActualizaEstadoOrientacion(int respuestaId, int orientacionId, int estadoId, string comentario, string color)
        {
            //queda pendiente por implementar
            bool respuesta = false;
            int Resultado = 0;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {                   
                    SqlCommand cmd = new SqlCommand("usp_ActualizaEstadoOrientacion", oConexion);
                    cmd.Parameters.AddWithValue("RespuestaOrientacionID", orientacionId);
                    cmd.Parameters.AddWithValue("DetalleRespuestaID", respuestaId);
                   
                    cmd.Parameters.AddWithValue("OrientacionEstadoID", estadoId);
                    cmd.Parameters.AddWithValue("Comentario", comentario);
                    cmd.Parameters.AddWithValue("Color", color);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    respuesta = Convert.ToBoolean(cmd.ExecuteNonQuery());
                   
                   

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
