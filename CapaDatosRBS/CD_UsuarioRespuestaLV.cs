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
   public class CD_UsuarioRespuestaLV
    {
        public static CD_UsuarioRespuestaLV _instancia = null;

        private CD_UsuarioRespuestaLV()
        {

        }

        public static CD_UsuarioRespuestaLV Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_UsuarioRespuestaLV();
                }
                return _instancia;
            }
        }

        public bool RegistrarUsuarioRespuesta(tbUsuarioRespuestaLV oUsuarioRespuesta)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarUsuarioRespuesta", oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario", oUsuarioRespuesta.IdUsuario);
                    cmd.Parameters.AddWithValue("IdRespuestaLV", oUsuarioRespuesta.IdRespuestaLV);
                    cmd.Parameters.AddWithValue("UsuarioCrea", oUsuarioRespuesta.UsuarioCrea);                    
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    respuesta = Convert.ToBoolean(cmd.ExecuteNonQuery());
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
