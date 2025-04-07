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
   public class CD_Permisos
    {
        public static CD_Permisos _instancia = null;

        private CD_Permisos()
        {

        }

        public static CD_Permisos Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_Permisos();
                }
                return _instancia;
            }
        }
        public List<tbPermisos> ObtenerPermisos(int IdRol)
        {
            List<tbPermisos> rptListaPermisos = new List<tbPermisos>();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerPermisos", oConexion);
                cmd.Parameters.AddWithValue("@IdRol", IdRol);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaPermisos.Add(new tbPermisos()
                        {
                            IdPermisos = Convert.ToInt32(dr["IdPermisos"].ToString()),
                            Menu = dr["Menu"].ToString(),
                            SubMenu = dr["SubMenu"].ToString(),
                            Activo = Convert.ToBoolean(dr["Activo"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaPermisos;

                }
                catch (Exception ex)
                {
                    rptListaPermisos = null;
                    return rptListaPermisos;
                }
            }
        }

        public bool ActualizarPermisos(string Detalle)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_ActualizarPermisos", oConexion);
                    cmd.Parameters.Add("Detalle", SqlDbType.Xml).Value = Detalle;
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
