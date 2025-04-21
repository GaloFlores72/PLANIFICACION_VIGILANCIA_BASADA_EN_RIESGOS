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
    public class CD_Constatacion
    {
        public static CD_Constatacion _instancia = null;

        private CD_Constatacion()
        {

        }

        public static CD_Constatacion Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_Constatacion();
                }
                return _instancia;
            }
        }

        public tbConstatacion ObtenerContatacionPorId(int orientacionID)
        {
            tbConstatacion oContatacion = new tbConstatacion();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerConstantacionPorConstatacionIDXml", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ConstatacionID", orientacionID);
                try
                {
                    oConexion.Open();
                    using (XmlReader reader = cmd.ExecuteXmlReader())
                    {
                        if (reader.Read())
                        {
                            XDocument doc = XDocument.Load(reader);

                            if (doc.Element("Constatacion") != null)
                            {
                                if (doc.Element("Constatacion").Element("DetalleEvidencia") != null)
                                {
                                    oContatacion = new tbConstatacion
                                    {
                                        ConstatacionID = (int)doc.Element("Constatacion")?.Element("ConstatacionID"),
                                        RespuestaOrientacionID = (int)doc.Element("Constatacion")?.Element("RespuestaOrientacionID"),
                                        AreaID = (int)doc.Element("Constatacion")?.Element("AreaID"),
                                        FechaConstatacion = (string)doc.Element("Constatacion")?.Element("FechaConstatacion") ?? "",
                                        PresuntaInfraccion = (bool)doc.Element("Constatacion")?.Element("PresuntaInfraccion"),
                                        DescripcionConstatacion = (string)doc.Element("Constatacion")?.Element("DescripcionConstatacion") ?? "",
                                        AfectaSO = (bool)doc.Element("Constatacion")?.Element("AfectaSO"),
                                        NotaAfectaSO = (string)doc.Element("Constatacion")?.Element("NotaAfectaSO") ?? "",
                                        UsuarioCreaID = (int)doc.Element("Constatacion")?.Element("UsuarioCreaID"),
                                        FechaCreacion = (DateTime)doc.Element("Constatacion")?.Element("FechaCreacion"),
                                        UsuarioModificaID = (int)doc.Element("Constatacion")?.Element("UsuarioModificaID"),
                                        FechaModifica = (DateTime)doc.Element("Constatacion")?.Element("FechaModifica"),
                                        oArea = (from area in doc.Element("Constatacion").Elements("Area")
                                                 select new tbArea()
                                                 {
                                                     AreaID = (int)area.Element("AreaID"),
                                                     OrganizacionID = (int)area.Element("OrganizacionID"),
                                                     Nombre = (string)area.Element("Nombre"),
                                                     Descripcion = (string)area.Element("Descripcion")
                                                 }).FirstOrDefault(),
                                        oEvidencias = (from eviden in doc.Element("Constatacion").Element("DetalleEvidencia").Elements("Evidencia")
                                                       select new tbEvidencia()
                                                       {
                                                           EvidenciaID = (int)eviden.Element("EvidenciaID"),
                                                           ConstatacionID = (int)eviden.Element("ConstatacionID"),
                                                           Descripcion = (string)eviden.Element("Descripcion"),
                                                           Path = (string)eviden.Element("Path"),
                                                           UsuarioCreaID = (int)eviden.Element("EvidenciaID"),
                                                           FechaCreacion = (DateTime)eviden.Element("FechaCreacion"),
                                                           UsuarioModificaID = (int)eviden.Element("UsuarioModificaID"),
                                                           FechaModifica = (DateTime)eviden.Element("FechaModifica")
                                                       }).ToList()
                                    };

                                }
                                else
                                {
                                    oContatacion = new tbConstatacion
                                    {
                                        ConstatacionID = (int)doc.Element("Constatacion")?.Element("ConstatacionID"),
                                        RespuestaOrientacionID = (int)doc.Element("Constatacion")?.Element("RespuestaOrientacionID"),
                                        AreaID = (int)doc.Element("Constatacion")?.Element("AreaID"),
                                        FechaConstatacion = (string)doc.Element("Constatacion")?.Element("FechaConstatacion") ?? "",
                                        PresuntaInfraccion = (bool)doc.Element("Constatacion")?.Element("PresuntaInfraccion"),
                                        DescripcionConstatacion = (string)doc.Element("Constatacion")?.Element("DescripcionConstatacion") ?? "",
                                        AfectaSO = (bool)doc.Element("Constatacion")?.Element("AfectaSO"),
                                        NotaAfectaSO = (string)doc.Element("Constatacion")?.Element("DescripcionConstatacion") ?? "",
                                        UsuarioCreaID = (int)doc.Element("Constatacion")?.Element("UsuarioCreaID"),
                                        FechaCreacion = (DateTime)doc.Element("Constatacion")?.Element("FechaCreacion"),
                                        UsuarioModificaID = (int)doc.Element("Constatacion")?.Element("UsuarioModificaID"),
                                        FechaModifica = (DateTime)doc.Element("Constatacion")?.Element("FechaModifica"),
                                        oArea = (from area in doc.Element("Constatacion").Elements("Area")
                                                 select new tbArea()
                                                 {
                                                     AreaID = (int)area.Element("AreaID"),
                                                     OrganizacionID = (int)area.Element("OrganizacionID"),
                                                     Nombre = (string)area.Element("Nombre"),
                                                     Descripcion = (string)area.Element("Descripcion")
                                                 }).FirstOrDefault()
                                    };
                                }

                                   
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return oContatacion;
                }

                return oContatacion;
            }
        }

        public List<tbConstatacion> ObtenerConstantacionPorOrientacionId(int OrientacionId)
        {
            List<tbConstatacion> oConstataciones = new List<tbConstatacion>();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerConstantacionPorOrientacionIdXml", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrientacionId", OrientacionId);
                try
                {
                    oConexion.Open();

                    using (XmlReader dr = cmd.ExecuteXmlReader())
                    {
                        while (dr.Read())
                        {
                            XDocument doc = XDocument.Load(dr);
                            foreach (var res in doc.Descendants("Constatacion"))
                            {
                                tbConstatacion oConstatacion = new tbConstatacion()
                                {
                                    ConstatacionID = (int)res.Element("ConstatacionID"),
                                    RespuestaOrientacionID = (int)res.Element("RespuestaOrientacionID"),
                                    AreaID = (int)res.Element("AreaID"),
                                    FechaConstatacion = (string)res.Element("FechaConstatacion"),
                                    PresuntaInfraccion = (bool)res.Element("PresuntaInfraccion"),
                                    DescripcionConstatacion = (string)res.Element("ReferenciaRegulatoria"),
                                    UsuarioCreaID = (int)res.Element("UsuarioCreaID"),
                                    FechaCreacion = (DateTime)res.Element("FechaCreacion"),
                                    UsuarioModificaID = (int)res.Element("UsuarioModificaID"),
                                    FechaModifica = (DateTime)res.Element("FechaCreacion"),
                                    oArea = new tbArea()
                                    {
                                        AreaID = (int)res.Element("Constatacion").Element("AreaID"),
                                        OrganizacionID = (int)res.Element("Constatacion").Element("OrganizacionID"),
                                        Nombre = (string)res.Element("Constatacion").Element("Nombre"),
                                        Descripcion = (string)res.Element("Constatacion").Element("Descripcion"),
                                        Activo = (bool)res.Element("Constatacion").Element("Activo"),
                                        UsuarioCrea = (string)res.Element("Constatacion").Element("UsuarioCrea"),
                                        FechaCrea = (DateTime)res.Element("Constatacion").Element("FechaCrea"),
                                        UsuarioModifica = (string)res.Element("Constatacion").Element("UsuarioModifica"),
                                        FechaModifica = (DateTime)res.Element("Constatacion").Element("FechaModifica")
                                    }
                                };

                                oConstataciones.Add(oConstatacion);
                            }
                            dr.Close();


                        }

                        return oConstataciones;
                    }
                }
                catch (Exception ex)
                {
                    oConstataciones = null;
                    return oConstataciones;
                }
            }
        }

        public tbRespuestaOrientacion ObtenerDetalleRespuestaPorConstatacionPorId(int DetalleRespuestaId)
        {
            tbRespuestaOrientacion oRespuestaOrientacion = new tbRespuestaOrientacion();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerDetalleRespuestaConstantacionesPorDetalleRespuestaIdXml", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DetalleRespuestaID", DetalleRespuestaId);
                try
                {
                    oConexion.Open();
                    using (XmlReader reader = cmd.ExecuteXmlReader())
                    {
                        if (reader.Read())
                        {
                            XDocument doc = XDocument.Load(reader);

                            if (doc.Element("RespuestaOrientacion") != null)
                            {

                                if (doc.Element("RespuestaOrientacion").Element("DetalleConstatacion") != null) {
                                    oRespuestaOrientacion = new tbRespuestaOrientacion()
                                    {
                                        RespuestaOrientacionID = (int)doc.Element("RespuestaOrientacion")?.Element("RespuestaOrientacionID"),
                                        DetalleRespuestaID = (int)doc.Element("RespuestaOrientacion")?.Element("DetalleRespuestaID"),
                                        OrientacionID = (int)doc.Element("RespuestaOrientacion")?.Element("OrientacionID"),
                                        EstadoImplementacionID = (int)doc.Element("RespuestaOrientacion")?.Element("EstadoImplementacionID"),
                                        Comentario = (string)doc.Element("RespuestaOrientacion")?.Element("Comentario") ?? "",
                                        CodigoPeligro = (string)doc.Element("RespuestaOrientacion")?.Element("CodigoPeligro") ?? "",
                                        DescripcionOrientacion = (string)doc.Element("RespuestaOrientacion")?.Element("DescripcionOrientacion") ?? "",
                                        Color = (string)doc.Element("RespuestaOrientacion")?.Element("Color") ?? "",
                                        oDetalleRespuesta = (from detalleResp in doc.Element("RespuestaOrientacion").Elements("DetalleRespuesta")
                                                             select new tbDetalleRespuesta()
                                                             {
                                                                 DetalleRespuestaID = (int)detalleResp.Element("DetalleRespuestaID"),
                                                                 RespuestaID = (int)detalleResp.Element("RespuestaID"),
                                                                 PreguntaID = (int)detalleResp.Element("PreguntaID"),
                                                                 Estado = (string)detalleResp.Element("Estado"),
                                                                 Comentario = (string)detalleResp.Element("Comentario"),
                                                                 SubtituloID = (int)detalleResp.Element("SubtituloID"),
                                                                 NombreSubtitulo = (string)detalleResp.Element("NombreSubtitulo"),
                                                                 CodigoPregunta = (string)detalleResp.Element("CodigoPregunta"),
                                                                 DescripcionPregunta = (string)detalleResp.Element("DescripcionPregunta"),
                                                                 ReferenciaPregunta = (string)detalleResp.Element("ReferenciaPregunta"),
                                                                 Color = (string)detalleResp.Element("Color"),
                                                                 UsuarioCrea = (string)detalleResp.Element("UsuarioCrea"),
                                                                 FechaCreacion = (DateTime)detalleResp.Element("FechaCreacion")
                                                             }).FirstOrDefault(),
                                        oConstatacion = (from constat in doc.Element("RespuestaOrientacion").Element("DetalleConstatacion").Elements("Constatacion")
                                                         select new tbConstatacion()
                                                         {
                                                             ConstatacionID = (int)constat.Element("ConstatacionID"),
                                                             RespuestaOrientacionID = (int)constat.Element("RespuestaOrientacionID"),
                                                             AreaID = (int)constat.Element("AreaID"),
                                                             FechaConstatacion = (string)constat.Element("FechaConstatacion"),
                                                             PresuntaInfraccion = (bool)constat.Element("PresuntaInfraccion"),
                                                             DescripcionConstatacion = (string)constat.Element("DescripcionConstatacion"),
                                                             AfectaSO = (bool)constat.Element("PresuntaInfraccion"),
                                                             NotaAfectaSO = (string)constat.Element("NotaAfectaSO"),
                                                             EstadoConstatacion = (string)constat.Element("EstadoConstatacion"),
                                                             FechaEnvio = (DateTime)constat.Element("FechaEnvio"),
                                                             UsuarioEnvioId = (int)constat.Element("UsuarioEnvioId"),
                                                             UsuarioCreaID = (int)constat.Element("UsuarioCreaID"),
                                                             FechaCreacion = (DateTime)constat.Element("FechaCreacion"),
                                                             UsuarioModificaID = (int)constat.Element("UsuarioModificaID"),
                                                             FechaModifica = (DateTime)constat.Element("FechaModifica"),
                                                             oArea = (from area in constat.Elements("Area")
                                                                      select new tbArea()
                                                                      {
                                                                          AreaID = (int)area.Element("AreaID"),
                                                                          OrganizacionID = (int)area.Element("OrganizacionID"),
                                                                          Nombre = (string)area.Element("Nombre"),
                                                                          Descripcion = (string)area.Element("Descripcion")
                                                                      }).FirstOrDefault()
                                                         }).ToList()


                                    };

                                }
                                else
                                {
                                    oRespuestaOrientacion = new tbRespuestaOrientacion()
                                    {
                                        RespuestaOrientacionID = (int)doc.Element("RespuestaOrientacion")?.Element("RespuestaOrientacionID"),
                                        DetalleRespuestaID = (int)doc.Element("RespuestaOrientacion")?.Element("DetalleRespuestaID"),
                                        OrientacionID = (int)doc.Element("RespuestaOrientacion")?.Element("OrientacionID"),
                                        EstadoImplementacionID = (int)doc.Element("RespuestaOrientacion")?.Element("EstadoImplementacionID"),
                                        Comentario = (string)doc.Element("RespuestaOrientacion")?.Element("Comentario") ?? "",
                                        CodigoPeligro = (string)doc.Element("RespuestaOrientacion")?.Element("CodigoPeligro") ?? "",
                                        DescripcionOrientacion = (string)doc.Element("RespuestaOrientacion")?.Element("DescripcionOrientacion") ?? "",
                                        Color = (string)doc.Element("RespuestaOrientacion")?.Element("Color") ?? "",
                                        oDetalleRespuesta = (from detalleResp in doc.Element("RespuestaOrientacion").Elements("DetalleRespuesta")
                                                             select new tbDetalleRespuesta()
                                                             {
                                                                 DetalleRespuestaID = (int)detalleResp.Element("DetalleRespuestaID"),
                                                                 RespuestaID = (int)detalleResp.Element("RespuestaID"),
                                                                 PreguntaID = (int)detalleResp.Element("PreguntaID"),
                                                                 Estado = (string)detalleResp.Element("Estado"),
                                                                 Comentario = (string)detalleResp.Element("Comentario"),
                                                                 SubtituloID = (int)detalleResp.Element("SubtituloID"),
                                                                 NombreSubtitulo = (string)detalleResp.Element("NombreSubtitulo"),
                                                                 CodigoPregunta = (string)detalleResp.Element("CodigoPregunta"),
                                                                 DescripcionPregunta = (string)detalleResp.Element("DescripcionPregunta"),
                                                                 ReferenciaPregunta = (string)detalleResp.Element("ReferenciaPregunta"),
                                                                 Color = (string)detalleResp.Element("Color"),
                                                                 UsuarioCrea = (string)detalleResp.Element("UsuarioCrea"),
                                                                 FechaCreacion = (DateTime)detalleResp.Element("FechaCreacion")
                                                             }).FirstOrDefault()                                     

                                    };

                                }

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    oRespuestaOrientacion = null;
                }

                return oRespuestaOrientacion;
            }

        }
        public int RegistraConstacion(tbConstatacion oConstatacion)
        {
            bool respuesta = true;
            int ConstatacionID = 0;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarConstatacion", oConexion);
                    cmd.Parameters.AddWithValue("RespuestaOrientacionID", oConstatacion.RespuestaOrientacionID);
                    cmd.Parameters.AddWithValue("AreaID", oConstatacion.AreaID);
                    cmd.Parameters.AddWithValue("FechaConstatacion", oConstatacion.FechaConstatacion);
                    cmd.Parameters.AddWithValue("PresuntaInfraccion", oConstatacion.PresuntaInfraccion);
                    cmd.Parameters.AddWithValue("DescripcionConstatacion", oConstatacion.DescripcionConstatacion);
                    cmd.Parameters.AddWithValue("AfectaSO", oConstatacion.AfectaSO);
                    cmd.Parameters.AddWithValue("NotaAfectaSO", oConstatacion.NotaAfectaSO);
                    cmd.Parameters.AddWithValue("EstadoConstatacion", oConstatacion.EstadoConstatacion);
                    cmd.Parameters.AddWithValue("UsuarioCreaID", oConstatacion.UsuarioCreaID);
                    cmd.Parameters.Add("ConstatacionID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    respuesta = Convert.ToBoolean(cmd.ExecuteNonQuery());                    
                    ConstatacionID = Convert.ToInt32(cmd.Parameters["ConstatacionID"].Value);

                }
                catch (Exception ex)
                {
                    ConstatacionID = 0;
                    respuesta = false;
                }
            }
            return ConstatacionID;
        }

        public bool ModificaConstacion(tbConstatacion oConstatacion)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_ModificarConstatacion", oConexion);
                    cmd.Parameters.AddWithValue("RespuestaOrientacionID", oConstatacion.RespuestaOrientacionID);
                    cmd.Parameters.AddWithValue("ConstatacionID", oConstatacion.ConstatacionID);
                    cmd.Parameters.AddWithValue("AreaID", oConstatacion.AreaID);
                    cmd.Parameters.AddWithValue("PresuntaInfraccion", oConstatacion.PresuntaInfraccion);
                    cmd.Parameters.AddWithValue("DescripcionConstatacion", campoNull(oConstatacion.DescripcionConstatacion));
                    cmd.Parameters.AddWithValue("AfectaSO", oConstatacion.AfectaSO);
                    cmd.Parameters.AddWithValue("NotaAfectaSO", campoNull(oConstatacion.NotaAfectaSO));
                    cmd.Parameters.AddWithValue("UsuarioModificaID", oConstatacion.UsuarioModificaID);
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

        public bool EliminarConstacion(int id)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_EliminarConstatacion", oConexion);
                    cmd.Parameters.AddWithValue("ConstatacionID", id);
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

        private string campoNull(string campo)
        {
            if (String.IsNullOrEmpty(campo))
                campo = "";
            return campo;
        }

    }
}
