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
   public class CD_Area
    {
        public static CD_Area _instancia = null;

        private CD_Area()
        {

        }

        public static CD_Area Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_Area();
                }
                return _instancia;
            }
        }


        /// <summary>
        /// Metodo obtiene todo los datos de Area de la organizacion
        /// </summary>
        /// <param name="idArea"></param>
        /// <returns></returns>
        public List<tbArea> ObtenerAreaPorPorOrganizacionID(int idOrganizacion)
        {
            List<tbArea> rptListaArea = new List<tbArea>();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerAreaPorOrganizacionID", oConexion);
                cmd.Parameters.AddWithValue("OrganizacionID", idOrganizacion);
                cmd.CommandType = CommandType.StoredProcedure;
               
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaArea.Add(new tbArea()
                        {
                            AreaID = Convert.ToInt32(dr["AreaID"].ToString()),
                            OrganizacionID = Convert.ToInt32(dr["OrganizacionID"].ToString()),
                            Nombre = dr["Nombre"].ToString(),
                            Activo = Convert.ToBoolean(dr["Estado"].ToString()),
                            UsuarioCrea = dr["UsuarioCrea"].ToString(),
                            FechaCrea = Convert.ToDateTime(dr["UsuarioCrea"].ToString()),
                            UsuarioModifica = dr["UsuarioModifica"].ToString(),
                            FechaModifica = Convert.ToDateTime(dr["FechaModifica"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaArea;

                }
                catch (Exception ex)
                {
                    rptListaArea = null;
                    return rptListaArea;
                }
            }
        }

        public List<tbArea> ObtenerAreaPorPorOrganizacionIDXml(int idOrganizacion)
        {
            List<tbArea> rptListaArea = new List<tbArea>();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerAreaPorOrganizacionIdXml", oConexion);
                cmd.Parameters.AddWithValue("OrganizacionID", idOrganizacion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();

                    using (XmlReader dr = cmd.ExecuteXmlReader())
                    {

                        while (dr.Read())
                        {
                            XDocument doc = XDocument.Load(dr);
                            foreach (var res in doc.Descendants("Area"))
                            {
                                tbArea area = new tbArea
                                {
                                    AreaID = (int)res.Element("AreaID"),                                   
                                    OrganizacionID = (int)res.Element("OrganizacionID"),
                                    Nombre = (string)res.Element("Nombre"),
                                    Descripcion = (string)res.Element("Descripcion"),
                                    Activo = (bool)res.Element("Estado"),
                                    UsuarioCrea = (string)res.Element("UsuarioCrea"),
                                    FechaCrea = (DateTime)res.Element("FechaCrea"),
                                    UsuarioModifica = (string)res.Element("UsuarioModifica"),
                                    FechaModifica = (DateTime)res.Element("FechaModifica")
                                };
                                rptListaArea.Add(area);
                            }

                            dr.Close();


                        }

                        return rptListaArea;
                    }
                }
                catch (Exception ex)
                {
                    rptListaArea = null;
                    return rptListaArea;
                }

            }
        }
    }
}
