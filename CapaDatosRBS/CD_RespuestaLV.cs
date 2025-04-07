using CapaModeloRBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;


namespace CapaDatosRBS
{
    public class CD_RespuestaLV
    {
        public static CD_RespuestaLV _instancia = null;

        private CD_RespuestaLV()
        {

        }

        public static CD_RespuestaLV Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_RespuestaLV();
                }
                return _instancia;
            }
        }

        public List<tbRespuestaLV> ObtenerRespuestaPorIDUsuario(string usuarioId)
        {
            var respuestas = new List<tbRespuestaLV>();

            using (var connection = new SqlConnection(ConexionSqlServer.CN))
            {
                string query = "SELECT * FROM tbRespuestaLV WHERE UsuarioID = @RespuestaID";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UsuarioID", usuarioId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        respuestas.Add(new tbRespuestaLV
                        {
                            RespuestaID = Convert.ToInt32(reader["RespuestaID"]),
                            ListaID = Convert.ToInt32(reader["ListaID"]),
                            OrganizacionID = Convert.ToInt32(reader["OrganizacionID"]),
                            UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                            Fecha = Convert.ToDateTime(reader["Fecha"]),
                            FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                            FechaFin = Convert.ToDateTime(reader["FechaFin"]),
                            InspectorResponsableID = Convert.ToInt32(reader["InspectorResponsableID"]),
                            oOrganizacion = CD_Organizacion.Instancia.ObtenerOrganizacion(Convert.ToInt32(reader["OrganizacionID"]))
                        });
                    }
                }
            }

            return respuestas;
        }



        public List<tbRespuestaLV> ObtenerRespuestaCabeceraTodos()
        {
            List<tbRespuestaLV> olistaRespuesta = new List<tbRespuestaLV>();
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
                                tbRespuestaLV respuesta = new tbRespuestaLV
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
                                    oUsuario = new tbUsuario
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
                                    },
                                    oListaDeVerificacion = new tbListaDeVerificacion
                                    {
                                        ListaID = (int)res.Element("ListaVerificacion").Element("ListaID"),
                                        Nombre = (string)res.Element("ListaVerificacion").Element("Nombre"),
                                        Descripcion = (string)res.Element("ListaVerificacion").Element("Descripcion"),
                                        oTipoProveedorServicio = new tbTipoProveedorServicio
                                        {
                                            IdTipoProveedor = (int)res.Element("ListaVerificacion").Element("IdTipoProveedor"),
                                            DescripcionTipoProveedor = (string)res.Element("ListaVerificacion").Element("DescripcionTipoProveedor")
                                        }
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

        public tbRespuestaLV ObtenerRespuestaPorId(int listaID)
        {
            tbRespuestaLV respuesta = new tbRespuestaLV();
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerRespuestaPorId", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RespuestaID", listaID);
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

                                respuesta.RespuestaID = (int)res.Element("RespuestaID");
                                respuesta.ListaID = (int)res.Element("ListaID");
                                respuesta.OrganizacionID = (int)res.Element("OrganizacionID");
                                respuesta.UsuarioID = (int)res.Element("UsuarioID");
                                respuesta.Fecha = (DateTime)res.Element("Fecha");
                                respuesta.FechaInicio = (DateTime)res.Element("FechaInicio");
                                respuesta.FechaFin = (DateTime)res.Element("FechaFin");
                                respuesta.InspectorResponsableID = (int)res.Element("InspectorResponsableID");
                                respuesta.UsuarioCrea = (string)res.Element("UsuarioCrea");
                                respuesta.FechaCrea = (DateTime)res.Element("FechaCrea");
                                respuesta.UsuarioModifica = (string)res.Element("UsuarioModifica");
                                respuesta.FechaModifica = (DateTime)res.Element("FechaModifica");
                                respuesta.oUsuario = new tbUsuario
                                {
                                    IdUsuario = (int)res.Element("Usuario").Element("IdUsuario"),
                                    Nombres = (string)res.Element("Usuario").Element("Nombres"),
                                    Apellidos = (string)res.Element("Usuario").Element("Apellidos"),
                                    Correo = (string)res.Element("Usuario").Element("Correo")
                                };
                                respuesta.oOrganizacion = new tbOrganizacion
                                {
                                    OrganizacionID = (int)res.Element("Organizacion").Element("OrganizacionID"),
                                    Nombre = (string)res.Element("Organizacion").Element("Nombre"),
                                    Direccion = (string)res.Element("Organizacion").Element("Direccion"),
                                    Telefono = (string)res.Element("Organizacion").Element("Telefono"),
                                    Correo = (string)res.Element("Organizacion").Element("Correo"),
                                    GerenteResponsable = (string)res.Element("Organizacion").Element("GerenteResponsable")
                                };
                                respuesta.oListaDeVerificacion = new tbListaDeVerificacion
                                {
                                    ListaID = (int)res.Element("ListaVerificacion").Element("ListaID"),
                                    Nombre = (string)res.Element("ListaVerificacion").Element("Nombre"),
                                    Descripcion = (string)res.Element("ListaVerificacion").Element("Descripcion"),
                                    oTipoProveedorServicio = new tbTipoProveedorServicio
                                    {
                                        IdTipoProveedor = (int)res.Element("ListaVerificacion").Element("IdTipoProveedor"),
                                        DescripcionTipoProveedor = (string)res.Element("ListaVerificacion").Element("DescripcionTipoProveedor")
                                    }
                                };
                                foreach (var insp in res.Descendants("ListaInspectores"))
                                {
                                    respuesta.oInpectores.Add(
                                         new tbUsuario()
                                         {
                                             IdUsuario = (int)insp.Element("Inspector").Element("IdUsuario"),
                                             Nombres = (string)insp.Element("Inspector").Element("Nombres"),
                                             Apellidos = (string)insp.Element("Inspector").Element("Apellidos"),
                                             Correo = (string)insp.Element("Inspector").Element("Correo")
                                         });
                                }

                            }

                            dr.Close();
                        }

                        return respuesta;
                    }
                }
                catch (Exception ex)
                {
                    respuesta = null;
                    return respuesta;
                }
            }
        }

        public List<tbRespuestaLV> ObtenerRespuestaCabeceraTodos1()
        {
            List<tbRespuestaLV> olistaRespuesta = new List<tbRespuestaLV>();
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
                            if (doc.Element("DetalleRespuesta") != null)
                            {
                                olistaRespuesta = (from dato in doc.Element("DetalleRespuesta").Elements("Respuesta")
                                                   select new tbRespuestaLV()
                                                   {
                                                       RespuestaID = int.Parse(dato.Element("RespuestaID").Value),
                                                       ListaID = int.Parse(dato.Element("ListaID").Value),
                                                       OrganizacionID = int.Parse(dato.Element("OrganizacionID").Value),
                                                       UsuarioID = int.Parse(dato.Element("UsuarioID").Value),
                                                       FechaInicio = DateTime.Parse(dato.Element("FechaInicio").Value),
                                                       FechaFin = DateTime.Parse(dato.Element("FechaFin").Value),
                                                       UsuarioCrea = dato.Element("UsuarioCrea").Value,
                                                       FechaCrea = DateTime.Parse(dato.Element("FechaCrea").Value),
                                                       UsuarioModifica = dato.Element("UsuarioModifica").Value,
                                                       FechaModifica = DateTime.Parse(dato.Element("FechaModifica").Value),
                                                       oUsuario = (from usua in doc.Element("DetalleRespuesta").Element("Respuesta").Elements("Usuario")
                                                                   select new tbUsuario()
                                                                   {
                                                                       IdUsuario = int.Parse(usua.Element("IdUsuario").Value),
                                                                       Nombres = usua.Element("Nombres").Value,
                                                                       Apellidos = usua.Element("Apellidos").Value,
                                                                       Correo = usua.Element("Correo").Value
                                                                   }).FirstOrDefault(),
                                                       oOrganizacion = (from organiz in doc.Element("DetalleRespuesta").Element("Respuesta").Elements("Organizacion")
                                                                        select new tbOrganizacion()
                                                                        {
                                                                            OrganizacionID = int.Parse(organiz.Element("OrganizacionID").Value),
                                                                            Nombre = organiz.Element("Nombre").Value,
                                                                            Direccion = organiz.Element("Direccion").Value,
                                                                            Telefono = organiz.Element("Telefono").Value,
                                                                            Correo = organiz.Element("Correo").Value,
                                                                            GerenteResponsable = organiz.Element("GerenteResponsable").Value
                                                                        }).FirstOrDefault(),
                                                       oListaDeVerificacion = (from listaV in dato.Element("Respuesta").Elements("ListaVerificacion")
                                                                               select new tbListaDeVerificacion()
                                                                               {
                                                                                   ListaID = int.Parse(listaV.Element("ListaID").Value),
                                                                                   Nombre = listaV.Element("Nombre").Value,
                                                                                   Descripcion = listaV.Element("Descripcion").Value,
                                                                                   IdTipoProveedorServicio = int.Parse(listaV.Element("IdTipoProveedor").Value),
                                                                                   oTipoProveedorServicio = new tbTipoProveedorServicio()
                                                                                   {
                                                                                       IdTipoProveedor = int.Parse(listaV.Element("IdTipoProveedor").Value),
                                                                                       DescripcionTipoProveedor = listaV.Element("DescripcionTipoProveedor").Value,
                                                                                   }


                                                                               }).FirstOrDefault()
                                                   }).ToList();
                            }
                            else
                            {
                                olistaRespuesta = null;
                            }
                        }

                        dr.Close();


                    }

                    return olistaRespuesta;
                }
                catch (Exception ex)
                {
                    olistaRespuesta = null;
                    return olistaRespuesta;
                }
            }
        }

        public bool GrabarRespuestaCabcera(tbRespuestaLV orespuesta)
        {
            bool respuesta = false;
            int RespuestaID = 0;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    // cmd.Parameters.AddWithValue("IdTipoProveedorServicio", orespuesta.IdTipoProveedorServicio);
                    SqlCommand cmd = new SqlCommand("usp_RegistrarRespuestaCabecera", oConexion);
                    cmd.Parameters.AddWithValue("ListaID", orespuesta.ListaID);
                    cmd.Parameters.AddWithValue("OrganizacionID", orespuesta.OrganizacionID);
                    cmd.Parameters.AddWithValue("UsuarioID", orespuesta.UsuarioID);
                    cmd.Parameters.AddWithValue("FechaInicio", orespuesta.FechaInicio);
                    cmd.Parameters.AddWithValue("FechaFin", orespuesta.FechaFin);
                    cmd.Parameters.AddWithValue("UsuarioCrea", orespuesta.UsuarioCrea);
                    cmd.Parameters.AddWithValue("NombreLista", orespuesta.NombreLista);
                    cmd.Parameters.AddWithValue("DescripcionLista", orespuesta.DescripcionLista);
                    cmd.Parameters.Add("RespuestaID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    respuesta = Convert.ToBoolean(cmd.ExecuteNonQuery());
                    RespuestaID = Convert.ToInt32(cmd.Parameters["RespuestaID"].Value);
                    orespuesta.RespuestaID = RespuestaID;
                    foreach (var item in orespuesta.oUsuarioRespuestaLV)
                    {
                        item.IdRespuestaLV = RespuestaID;
                        item.UsuarioCrea = orespuesta.UsuarioCrea;

                        respuesta = CD_UsuarioRespuestaLV.Instancia.RegistrarUsuarioRespuesta(item);
                    }

                    //grabo el detalle de la respuesta
                    if (respuesta)
                    {
                        foreach (var item in orespuesta.oListaDeVerificacion.oSubtitulos)
                        {
                            foreach (var itemPreg in item.oListaPreguntas)
                            {
                                tbDetalleRespuestaLV odetalleRespuesta = new tbDetalleRespuestaLV()
                                {
                                    DetalleRespuestaID = 0,
                                    RespuestaID = RespuestaID,
                                    PreguntaID = itemPreg.PreguntaID,
                                    Estado = "",
                                    Comentario = "",
                                    SubtituloID = item.SubtituloID,
                                    NombreSubtitulo = item.Nombre,
                                    DescripcionPregunta = itemPreg.Descripcion,
                                    ReferenciaPregunta = itemPreg.Referencia,
                                    UsuarioCrea = orespuesta.UsuarioCrea
                                };
                                odetalleRespuesta.DetalleRespuestaID = CD_DetalleRespuestaLV.Instancia.RegistrarDetalleRespuesta(odetalleRespuesta);
                                if (odetalleRespuesta.DetalleRespuestaID > 0)
                                {
                                    foreach (var itemOri in itemPreg.oListaOrientaciones)
                                    {
                                        tbRespuestaOrientacion oRespuestaOrientacion = new tbRespuestaOrientacion()
                                        {
                                            RespuestaOrientacionID =0,
                                            DetalleRespuestaID = odetalleRespuesta.DetalleRespuestaID,
                                            OrientacionID = itemOri.OrientacionID,
                                            EstadoImplementacionID = 0,
                                            Comentario = "",
                                            UsuarioCreaId = 0,
                                            CodigoPeligro = itemOri.CodigoPeligro,
                                            DescripcionOrientacion = itemOri.Descripcion
                                        };
                                        respuesta = CD_RespuestaOrientacion.Instancia.RegistrarRespuestaOrientacion(oRespuestaOrientacion);
                                    }
                                }

                            }
                        }
                    }

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
