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
  public  class CD_TipoProveedorServicio
    {
        public static CD_TipoProveedorServicio _instancia = null;

        private CD_TipoProveedorServicio()
        {

        }

        public static CD_TipoProveedorServicio Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_TipoProveedorServicio();
                }
                return _instancia;
            }
        }

        public tbTipoProveedorServicio ObtenerTipoProveedorServicioPorId(int IdTipoProveedor)
        {
            tbTipoProveedorServicio otipoProveedorServicio = new tbTipoProveedorServicio();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("[usp_ObtenerTipoProveedorServicioPorId]", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdTipoProveedor", IdTipoProveedor);
                try
                {
                    oConexion.Open();
                    using (XmlReader dr = cmd.ExecuteXmlReader())
                    {
                        while (dr.Read())
                        {
                            XDocument doc = XDocument.Load(dr);
                            if (doc.Element("TipoProveedorServicio") != null)
                            {
                                otipoProveedorServicio = (from dato in doc.Elements("TipoProveedorServicio")
                                                          select new tbTipoProveedorServicio()
                                                          {
                                                              IdTipoProveedor = int.Parse(dato.Element("IdTipoProveedor").Value),
                                                              DescripcionTipoProveedor = dato.Element("DescripcionTipoProveedor").Value,
                                                              Estado = (dato.Element("Estado").Value.ToString() == "1" ? true : false),
                                                              UsuarioCrea = dato.Element("UsuarioCrea").Value,
                                                              FechaCrea = DateTime.Parse(dato.Element("FechaCrea").Value),
                                                              UsuarioModifica = dato.Element("UsuarioModifica").Value,
                                                              FechaModifica = DateTime.Parse(dato.Element("FechaModifica").Value),
                                                              oListaDeVerificacion = (from listaVerificacion in doc.Element("TipoProveedorServicio").Element("DetalleListaVerificacion").Elements("ListaVerificacion")
                                                                                      select new tbListaDeVerificacion()
                                                                                      {
                                                                                          ListaID = int.Parse(listaVerificacion.Element("ListaID").Value),
                                                                                          Nombre = listaVerificacion.Element("Nombre").Value,
                                                                                          Descripcion = listaVerificacion.Element("Descripcion").Value,
                                                                                          Estado = (listaVerificacion.Element("Estado").Value.ToString() == "1" ? true : false),
                                                                                          UsuarioCrea = listaVerificacion.Element("UsuarioCrea").Value,
                                                                                          FechaCreacion = DateTime.Parse(listaVerificacion.Element("FechaCreacion").Value),
                                                                                          UsuarioModifica = listaVerificacion.Element("UsuarioModifica").Value,
                                                                                          FechaModifica = DateTime.Parse(listaVerificacion.Element("FechaModifica").Value),
                                                                                          IdTipoProveedorServicio = int.Parse(listaVerificacion.Element("IdTipoProveedorServicio").Value),
                                                                                      }).ToList()


                                                          }).FirstOrDefault();

                            }
                            else
                            {
                                otipoProveedorServicio = null;
                            }
                        }

                        dr.Close();

                    }

                    return otipoProveedorServicio;
                }
                catch (Exception ex)
                {
                    otipoProveedorServicio = null;
                    return otipoProveedorServicio;
                }
            }
        }


        public List<tbTipoProveedorServicio> ObtenerTipoProveedorServicio()
        {
            List<tbTipoProveedorServicio> rptListaTipoProveedorServicio = new List<tbTipoProveedorServicio>();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerTipoProveedorServicio", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaTipoProveedorServicio.Add(new tbTipoProveedorServicio()
                        {
                            IdTipoProveedor = Convert.ToInt32(dr["IdTipoProveedor"].ToString()),
                            DescripcionTipoProveedor = dr["DescripcionTipoProveedor"].ToString(),
                            Estado = Convert.ToBoolean(dr["Estado"].ToString()),
                            UsuarioCrea = dr["UsuarioCrea"].ToString(),
                            FechaCrea = Convert.ToDateTime(dr["FechaCrea"].ToString()),
                            UsuarioModifica = dr["UsuarioModifica"].ToString(),
                            FechaModifica = Convert.ToDateTime(dr["FechaModifica"].ToString()),
                        });
                    }
                    dr.Close();

                    return rptListaTipoProveedorServicio;

                }
                catch (Exception ex)
                {
                    rptListaTipoProveedorServicio = null;
                    return rptListaTipoProveedorServicio;
                }
            }
        }
        // Insertar un nuevo Tipo de Proveedor
        public bool RegistrarTipoProveedorServicio(tbTipoProveedorServicio entidad)
        {
            bool respuesta = false;

            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarTipoProveedorServicio", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DescripcionTipoProveedor", entidad.DescripcionTipoProveedor);
                    cmd.Parameters.AddWithValue("@Estado", entidad.Estado);
                    cmd.Parameters.AddWithValue("@UsuarioCrea", entidad.UsuarioCrea);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

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

        // Modificar un Tipo de Proveedor
        public bool ModificarTipoProveedorServicio(tbTipoProveedorServicio entidad)
        {
            bool respuesta = false;

            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_ModificarTipoProveedorServicio", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdTipoProveedor", entidad.IdTipoProveedor);
                    cmd.Parameters.AddWithValue("@DescripcionTipoProveedor", entidad.DescripcionTipoProveedor);
                    cmd.Parameters.AddWithValue("@Estado", entidad.Estado);
                    cmd.Parameters.AddWithValue("@UsuarioModifica", entidad.UsuarioModifica);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

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

        // Eliminar un Tipo de Proveedor (actualiza estado en lugar de eliminación física)
        public bool EliminarTipoProveedorServicio(int id, string usuarioModifica)
        {
            bool respuesta = false;

            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_EliminarTipoProveedorServicio", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdTipoProveedor", id);
                    cmd.Parameters.AddWithValue("@UsuarioModifica", usuarioModifica);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

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
