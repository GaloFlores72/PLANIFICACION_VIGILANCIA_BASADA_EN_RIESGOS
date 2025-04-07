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
  public  class CD_Constatacion
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
                                oContatacion = new tbConstatacion
                                {
                                    ConstatacionID = (int)doc.Element("Constatacion")?.Element("ConstatacionID"),
                                    OrientacionID = (int)doc.Element("Constatacion")?.Element("OrientacionID"),
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
                                    OrientacionID = (int)res.Element("OrientacionID"),
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


        public int RegistraConstacion(tbConstatacion oConstatacion)
        {
            bool respuesta = true;
            int ConstatacionID = 0;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarConstatacion", oConexion);
                    cmd.Parameters.AddWithValue("OrientacionID", oConstatacion.OrientacionID);
                    cmd.Parameters.AddWithValue("AreaID", oConstatacion.AreaID);
                    cmd.Parameters.AddWithValue("FechaConstatacion", oConstatacion.FechaConstatacion);
                    cmd.Parameters.AddWithValue("PresuntaInfraccion", oConstatacion.PresuntaInfraccion);
                    cmd.Parameters.AddWithValue("DescripcionConstatacion", oConstatacion.DescripcionConstatacion);
                    cmd.Parameters.AddWithValue("AfectaSO", oConstatacion.AfectaSO);
                    cmd.Parameters.AddWithValue("NotaAfectaSO", oConstatacion.NotaAfectaSO);
                    cmd.Parameters.AddWithValue("EstadoConstatacion", oConstatacion.EstadoConstatacion);
                    cmd.Parameters.AddWithValue("UsuarioCreaID", oConstatacion.UsuarioCreaID); 
                    cmd.Parameters.Add("ConstatacionID", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["ConstatacionID"].Value);

                }
                catch (Exception ex)
                {
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
                    cmd.Parameters.AddWithValue("OrientacionID", oConstatacion.OrientacionID);
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
