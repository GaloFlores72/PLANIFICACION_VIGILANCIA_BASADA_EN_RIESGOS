using CapaModeloRBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatosRBS
{
    public class CD_Organizacion
    {
        public static CD_Organizacion _instancia = null;

        private CD_Organizacion()
        {

        }

        public static CD_Organizacion Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_Organizacion();
                }
                return _instancia;
            }
        }

        /// <summary>
        /// Metodo obtiuene todos los registros de Organizacion
        /// </summary>
        /// <returns></returns>
        public List<tbOrganizacion> ObtenerOrganizaciones()
        {
            List<tbOrganizacion> oListOrganizaciones = new List<tbOrganizacion>();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerOrganizacionTodos", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        oListOrganizaciones.Add(new tbOrganizacion()
                        {
                            OrganizacionID = Convert.ToInt32(dr["OrganizacionID"].ToString()),
                            Nombre = dr["Nombre"].ToString(),
                            Direccion = dr["Direccion"].ToString(),
                            GerenteResponsable = dr["GerenteResponsable"].ToString(),
                            NCertificadoOMA = dr["NCertificadoOMA"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Telefono = dr["Telefono"].ToString()
                        });
                    }
                    dr.Close();

                    return oListOrganizaciones;

                }
                catch (Exception ex)
                {
                    oListOrganizaciones = null;
                    return oListOrganizaciones;
                }
            }
        }

        /// <summary>
        /// Metodo obtiene Organización
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tbOrganizacion ObtenerOrganizacion(int id)
        {
            tbOrganizacion oOrganizacion = new tbOrganizacion();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerOrganizacionPorId", oConexion);
                cmd.Parameters.AddWithValue("@OrganizacionID", id);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        oOrganizacion.OrganizacionID = Convert.ToInt32(dr["OrganizacionID"].ToString());
                        oOrganizacion.Nombre = dr["Nombre"].ToString();
                        oOrganizacion.Direccion = dr["Direccion"].ToString();
                        oOrganizacion.GerenteResponsable = dr["GerenteResponsable"].ToString();
                        oOrganizacion.NCertificadoOMA = dr["NCertificadoOMA"].ToString();
                        oOrganizacion.Correo = dr["Correo"].ToString();
                        oOrganizacion.Telefono = dr["Telefono"].ToString();
                    }
                    dr.Close();
                    return oOrganizacion;
                }
                catch (Exception ex)
                {
                    oOrganizacion = null;
                    return oOrganizacion;
                }
            }
        }

        /// <summary>
        /// Elimina una organización si no tiene dependencias. Devuelve:
        /// 1 = Éxito, 2 = Dependencias encontradas, 0 = Error
        /// </summary>
        public int EliminarOrganizacion(int organizacionId)
        {
            int resultado = 0;

            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_EliminarOrganizacion", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrganizacionID", organizacionId);

                SqlParameter outResultado = new SqlParameter("@Resultado", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outResultado);

                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToInt32(outResultado.Value);
                }
                catch (Exception ex)
                {
                    resultado = 0; // Error general
                }
            }

            return resultado;
        }

        public int RegistrarOrganizacion(tbOrganizacion oOrganizacion)
        {
            int resultado = 0;

            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_RegistrarOrganizacion", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombre", oOrganizacion.Nombre);
                cmd.Parameters.AddWithValue("@Direccion", oOrganizacion.Direccion);
                cmd.Parameters.AddWithValue("@GerenteResponsable", oOrganizacion.GerenteResponsable);
                cmd.Parameters.AddWithValue("@NCertificadoOMA", oOrganizacion.NCertificadoOMA);
                cmd.Parameters.AddWithValue("@Correo", oOrganizacion.Correo);
                cmd.Parameters.AddWithValue("@Telefono", (object)oOrganizacion.Telefono ?? DBNull.Value);

                SqlParameter pResultado = new SqlParameter("@Resultado", SqlDbType.Bit);
                pResultado.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pResultado);

                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToInt32(pResultado.Value);
                }
                catch
                {
                    resultado = 0;
                }
            }

            return resultado;
        }

        public int ModificarOrganizacion(tbOrganizacion oOrganizacion)
        {
            int resultado = 0;

            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ModificarOrganizacion", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrganizacionID", oOrganizacion.OrganizacionID);
                cmd.Parameters.AddWithValue("@Nombre", oOrganizacion.Nombre);
                cmd.Parameters.AddWithValue("@Direccion", oOrganizacion.Direccion);
                cmd.Parameters.AddWithValue("@GerenteResponsable", oOrganizacion.GerenteResponsable);
                cmd.Parameters.AddWithValue("@NCertificadoOMA", oOrganizacion.NCertificadoOMA);
                cmd.Parameters.AddWithValue("@Correo", oOrganizacion.Correo);
                cmd.Parameters.AddWithValue("@Telefono", (object)oOrganizacion.Telefono ?? DBNull.Value);

                SqlParameter pResultado = new SqlParameter("@Resultado", SqlDbType.Bit);
                pResultado.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pResultado);

                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToInt32(pResultado.Value);
                }
                catch
                {
                    resultado = 0;
                }
            }

            return resultado;
        }



    }
}
