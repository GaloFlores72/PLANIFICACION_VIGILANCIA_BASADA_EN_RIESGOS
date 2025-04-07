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

    }
}
