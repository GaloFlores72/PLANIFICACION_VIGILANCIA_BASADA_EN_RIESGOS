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
  public  class CD_Usuario
    {
        public static CD_Usuario _instancia = null;

        private CD_Usuario()
        {

        }

        public static CD_Usuario Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_Usuario();
                }
                return _instancia;
            }
        }

        public int LoginUsuario(string Usuario, string Clave)
        {
            int respuesta = 0;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_LoginUsuarioPorCodigo", oConexion);
                    cmd.Parameters.AddWithValue("CodigoUsuario", Usuario);
                    cmd.Parameters.AddWithValue("Clave", Clave);
                    cmd.Parameters.Add("IdUsuario", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(cmd.Parameters["IdUsuario"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = 0;
                }
            }
            return respuesta;
        }

        public tbUsuario ObtenerDetalleUsuario(int IdUsuario)
        {
            tbUsuario rptUsuario = new tbUsuario();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerDetalleUsuario", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);

                try
                {
                    oConexion.Open();
                    using (XmlReader dr = cmd.ExecuteXmlReader())
                    {
                        while (dr.Read())
                        {
                            XDocument doc = XDocument.Load(dr);
                            if (doc.Element("Usuario") != null)
                            {
                                rptUsuario = (from dato in doc.Elements("Usuario")
                                              select new tbUsuario()
                                              {
                                                  IdUsuario = int.Parse(dato.Element("IdUsuario").Value),
                                                  Nombres = dato.Element("Nombres").Value,
                                                  Apellidos = dato.Element("Apellidos").Value,
                                                  Correo = dato.Element("Correo").Value,
                                                  Clave = dato.Element("Clave").Value,

                                              }).FirstOrDefault();                                
                                rptUsuario.oRol = (from dato in doc.Element("Usuario").Elements("DetalleRol")
                                                   select new tbRol()
                                                   {
                                                       DescripcionRol = dato.Element("DescripcionRol").Value
                                                   }).FirstOrDefault();
                                rptUsuario.oListaMenu = (from menu in doc.Element("Usuario").Element("DetalleMenu").Elements("Menu")
                                                         select new tbMenu()
                                                         {
                                                             NombreMenu = menu.Element("NombreMenu").Value,
                                                             Icono = menu.Element("Icono").Value,
                                                             oSubMenu = (from submenu in menu.Element("DetalleSubMenu").Elements("SubMenu")
                                                                         select new tbSubMenu()
                                                                         {
                                                                             NombreSubMenu = submenu.Element("NombreSubMenu").Value,
                                                                             Controlador = submenu.Element("Controlador").Value,
                                                                             Vista = submenu.Element("Vista").Value,
                                                                             Icono = submenu.Element("Icono").Value,
                                                                             Activo = (submenu.Element("Activo").Value.ToString() == "1" ? true : false),

                                                                         }).ToList()

                                                         }).ToList();
                            }
                            else
                            {
                                rptUsuario = null;
                            }
                        }

                        dr.Close();

                    }

                    return rptUsuario;
                }
                catch (Exception ex)
                {
                    rptUsuario = null;
                    return rptUsuario;
                }
            }
        }

        public List<tbUsuario> ObtenerUsuarios()
        {
            List<tbUsuario> rptListaUsuario = new List<tbUsuario>();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerUsuario", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaUsuario.Add(new tbUsuario()
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"].ToString()),
                            CodigoUsuario = dr["CodigoUsuario"].ToString(),
                            Nombres = dr["Nombres"].ToString(),
                            Apellidos = dr["Apellidos"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Clave = dr["Clave"].ToString(),                            
                            IdRol = Convert.ToInt32(dr["IdRol"].ToString()),
                            oRol = new tbRol() { DescripcionRol = dr["DescripcionRol"].ToString() },
                            Activo = Convert.ToBoolean(dr["Activo"])

                        });
                    }
                    dr.Close();

                    return rptListaUsuario;

                }
                catch (Exception ex)
                {
                    rptListaUsuario = null;
                    return rptListaUsuario;
                }
            }
        }

        public List<tbUsuario> ObtenerUsuariosInspectores()
        {
            List<tbUsuario> rptListaUsuario = new List<tbUsuario>();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerUsuarioInpector", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaUsuario.Add(new tbUsuario()
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"].ToString()),
                            CodigoUsuario = dr["CodigoUsuario"].ToString(),
                            Nombres = dr["Nombres"].ToString(),
                            Apellidos = dr["Apellidos"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Clave = dr["Clave"].ToString(),
                            IdRol = Convert.ToInt32(dr["IdRol"].ToString()),
                            oRol = new tbRol() { DescripcionRol = dr["DescripcionRol"].ToString() },
                            Activo = Convert.ToBoolean(dr["Activo"])

                        });
                    }
                    dr.Close();

                    return rptListaUsuario;

                }
                catch (Exception ex)
                {
                    rptListaUsuario = null;
                    return rptListaUsuario;
                }
            }
        }

        public bool RegistrarUsuario(tbUsuario oUsuario)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarUsuario", oConexion);
                    cmd.Parameters.AddWithValue("CodigoUsuario", oUsuario.CodigoUsuario);
                    cmd.Parameters.AddWithValue("Nombres", oUsuario.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", oUsuario.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", oUsuario.Correo);
                    cmd.Parameters.AddWithValue("Clave", oUsuario.Clave);
                    cmd.Parameters.AddWithValue("IdRol", oUsuario.IdRol);
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

        public bool ModificarUsuario(tbUsuario oUsuario)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_ModificarUsuario", oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario", oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("CodigoUsuario", oUsuario.CodigoUsuario);
                    cmd.Parameters.AddWithValue("Nombres", oUsuario.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", oUsuario.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", oUsuario.Correo);
                    cmd.Parameters.AddWithValue("Clave", oUsuario.Clave);
                    cmd.Parameters.AddWithValue("IdRol", oUsuario.IdRol);
                    cmd.Parameters.AddWithValue("Activo", oUsuario.Activo);
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

        public bool EliminarUsuario(int IdUsuario)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_EliminarUsuario", oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario", IdUsuario);
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
