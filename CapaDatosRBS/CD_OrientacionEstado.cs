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
    public class CD_OrientacionEstado
    {
        public static CD_OrientacionEstado _instancia = null;

        private CD_OrientacionEstado()
        {

        }

        public static CD_OrientacionEstado Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_OrientacionEstado();
                }
                return _instancia;
            }
        }

        public tbOrientacionEstado ObtieneOrientacionEstado(int orientacionEstadoID)
        {
            tbOrientacionEstado oOrientacionEstado = new tbOrientacionEstado();
            string query = "SELECT oe.OrientacionEstadoID, oe.OrientacionID, em.EstadoID, em.Descripcion,  ISNULL(oe.Color, '#FFFFFF') as Color FROM EstadosDeImplementacion em"
                + " INNER JOIN OrientacionesEstados oe ON(EM.EstadoID = OE.EstadoID)"
                + " WHERE oe.OrientacionEstadoID = " + orientacionEstadoID;
            using (var connection = new SqlConnection(ConexionSqlServer.CN))
            {
                var command = new SqlCommand(query, connection);
                //command.Parameters.AddWithValue("@OrientacionID", orientacionEstadoID);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        oOrientacionEstado.OrientacionEstadoID = reader.GetInt32(reader.GetOrdinal("OrientacionEstadoID"));

                        oOrientacionEstado.OrientacionID = reader.GetInt32(reader.GetOrdinal("OrientacionID"));
                        oOrientacionEstado.EstadoID = reader.GetInt32(reader.GetOrdinal("EstadoID"));
                        oOrientacionEstado.Color = reader.GetString(reader.GetOrdinal("Color"));
                        oOrientacionEstado.oEstadoDeImplementacion = CD_EstadosDeImplementacion.Instancia.ObtenerEstadoDeImplementacionPorEstadoId(reader.GetInt32(reader.GetOrdinal("EstadoID")));
                    }
                }
            }
            return oOrientacionEstado;
        }

        public List<tbOrientacionEstado> ObtenerOrientacionEstado(int OrientacionID)
        {
            var listas = new List<tbOrientacionEstado>();

            using (var connection = new SqlConnection(ConexionSqlServer.CN))
            {
                var command = new SqlCommand("SELECT * FROM OrientacionesEstados where OrientacionID = @OrientacionID", connection);
                command.Parameters.AddWithValue("@OrientacionID", OrientacionID);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listas.Add(new tbOrientacionEstado
                        {
                            OrientacionEstadoID = reader.GetInt32(reader.GetOrdinal("OrientacionEstadoID")),
                            OrientacionID = reader.GetInt32(reader.GetOrdinal("OrientacionID")),
                            EstadoID = reader.GetInt32(reader.GetOrdinal("EstadoID")),
                            Color = reader.GetString(reader.GetOrdinal("Color")),
                            oEstadoDeImplementacion = CD_EstadosDeImplementacion.Instancia.ObtenerEstadoDeImplementacionPorEstadoId(reader.GetInt32(reader.GetOrdinal("EstadoID")))
                        });
                    }
                }
            }

            return listas;
        }

        public List<tbOrientacionEstado> ObtenerOrientacionEstadosTodos()
        {
            List<tbOrientacionEstado> rptOrientacionEstado = new List<tbOrientacionEstado>();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerOrientacionEstadosTodos", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        rptOrientacionEstado.Add(new tbOrientacionEstado()
                        {
                            OrientacionEstadoID = reader.GetInt32(reader.GetOrdinal("OrientacionEstadoID"))
                           
                        });
                    }
                    reader.Close();

                    return rptOrientacionEstado;
                }
                catch (Exception ex)
                {
                    rptOrientacionEstado = null;
                    return rptOrientacionEstado;
                }
            }
        }

        public List<tbOrientacionEstado> ObtenerOrientacionEstadoPorOrientacionID(int OrientacionID)
        {
            List<tbOrientacionEstado> orientacionesEstados = new List<tbOrientacionEstado>();

            string query = @"SELECT e.OrientacionEstadoID, o.Descripcion as DescripcionOrientacion, estado.Descripcion as DescripcionEstado 
                            FROM ORIENTACIONESESTADOS e
                            JOIN ORIENTACIONES o ON o.OrientacionID = e.OrientacionID
                            JOIN ESTADOSDEIMPLEMENTACION estado ON estado.EstadoID = e.EstadoID
                            WHERE 
                                o.OrientacionID = @OrientacionID";

            using (var connection = new SqlConnection(ConexionSqlServer.CN))

            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@OrientacionID", OrientacionID);
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tbOrientacionEstado orientacionEstado = new tbOrientacionEstado
                        {
                            OrientacionEstadoID = reader.GetInt32(reader.GetOrdinal("OrientacionEstadoID")),
                            //OrientacionID = reader.GetInt32(reader.GetOrdinal("OrientacionID")),
                            //DescripcionOrientacion = reader.GetString(reader.GetOrdinal("DescripcionOrientacion")),
                            //DescripcionEstado = reader.GetString(reader.GetOrdinal("DescripcionEstado"))
                        };

                        orientacionesEstados.Add(orientacionEstado);
                    }
                }
            }

            return orientacionesEstados;
        }

        public bool RegistrarOrientacionEstado(tbOrientacionEstado orientacion)
        {
            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarEstadosDeOrientacion", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.AddWithValue("@OrientacionEstadoID", orientacion.OrientacionEstadoID);
                    cmd.Parameters.AddWithValue("@OrientacionID", orientacion.OrientacionID);
                    cmd.Parameters.AddWithValue("@EstadoID", orientacion.EstadoID);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                }
                catch (Exception ex)
                {
                    respuesta = false;
                    Console.WriteLine(ex.Message);
                }
            }
            return respuesta;
        }

        public bool ModificarOrientacionEstado(tbOrientacionEstado objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_ModificarOrientacionEstado", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@OrientacionEstadoID", objeto.OrientacionEstadoID);
                    cmd.Parameters.AddWithValue("@OrientacionID", objeto.OrientacionID);
                    cmd.Parameters.AddWithValue("@EstadoID", objeto.EstadoID);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool EliminarOrientacion(int orientacionID)
        {
            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_EliminarOrientacion", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@OrientacionID", orientacionID);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Console.WriteLine("Resultado de la eliminación: " + respuesta);
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
