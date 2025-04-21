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
    public class CD_Evidencia
    {
        public static CD_Evidencia _instancia = null;

        private CD_Evidencia()
        { }
        public static CD_Evidencia Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_Evidencia();
                }
                return _instancia;
            }
        }

        public bool RegistrarEvidencia(tbEvidencia oEvidencia)
        {
            bool respuesta = true;
            int evidenciaID = 0;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarEvidencia", oConexion);
                    cmd.Parameters.AddWithValue("ConstatacionID", oEvidencia.ConstatacionID);
                    cmd.Parameters.AddWithValue("Descripcion", oEvidencia.Descripcion);
                    cmd.Parameters.AddWithValue("Path", oEvidencia.Path);                    
                    cmd.Parameters.AddWithValue("UsuarioCreaID", oEvidencia.UsuarioCreaID);
                    cmd.Parameters.Add("EvidenciaID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    respuesta = Convert.ToBoolean(cmd.ExecuteNonQuery());
                    evidenciaID = Convert.ToInt32(cmd.Parameters["EvidenciaID"].Value);

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
