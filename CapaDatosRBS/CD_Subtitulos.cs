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
    public class CD_Subtitulos
    {
        public static CD_Subtitulos _instancia = null;

        private CD_Subtitulos()
        {

        }

        public static CD_Subtitulos Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_Subtitulos();
                }
                return _instancia;
            }
        }

        public List<tbSubtitulo> ObtenerSubtitulo()
        {
            List<tbSubtitulo> rptSubtitulo = new List<tbSubtitulo>();
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerSubtituloTodos", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        rptSubtitulo.Add(new tbSubtitulo()
                        {
                            SubtituloID = reader.GetInt32(reader.GetOrdinal("SubtituloID")),
                            ListaID = reader.GetInt32(reader.GetOrdinal("ListaID")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                            Estado = reader.GetBoolean(reader.GetOrdinal("Estado")),
                            ListaNombre = reader.GetString(reader.GetOrdinal("ListaNombre"))
                        });
                    }
                    reader.Close();

                    return rptSubtitulo;
                }
                catch (Exception ex)
                {
                    rptSubtitulo = null;
                    return rptSubtitulo;
                }
            }
        }


        public tbSubtitulo ObtenerSubtituloPorOid(int subtituloID)
        {
            tbSubtitulo osubtitulo = new tbSubtitulo();
            using (var connection = new SqlConnection(ConexionSqlServer.CN))
            {
                var command = new SqlCommand("SELECT * FROM SUBTITULOS where SubtituloID = " + subtituloID, connection);
                command.Parameters.AddWithValue("@SubtituloID", subtituloID);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        osubtitulo = new tbSubtitulo
                        {
                            SubtituloID = reader.GetInt32(reader.GetOrdinal("SubtituloID")),
                            ListaID = reader.GetInt32(reader.GetOrdinal("ListaID")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                            Estado = reader.GetBoolean(reader.GetOrdinal("Estado")),
                        };
                    }
                }
            }

            return osubtitulo;
        }

        public List<tbSubtitulo> ObtenerSubtitulosPorListaId(int ListaID)
        {
            List<tbSubtitulo> subtitulos = new List<tbSubtitulo>();
            string query = @"SELECT  s.SubtituloID,  s.ListaID, s.Nombre, s.Descripcion, s.Estado, l.Nombre AS ListaNombre 
                            FROM Subtitulos s
                            INNER JOIN 
                                ListasDeVerificacion l ON s.ListaID = l.ListaID
                            WHERE 
                                s.ListaID = @ListaID";

            using (var connection = new SqlConnection(ConexionSqlServer.CN))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ListaID", ListaID);
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tbSubtitulo subtitulo = new tbSubtitulo
                        {
                            SubtituloID = reader.GetInt32(reader.GetOrdinal("SubtituloID")),
                            ListaID = reader.GetInt32(reader.GetOrdinal("ListaID")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")).Trim(),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")).Trim(),
                            Estado = reader.GetBoolean(reader.GetOrdinal("Estado")),
                            ListaNombre = reader.GetString(reader.GetOrdinal("ListaNombre"))
                        };

                        subtitulos.Add(subtitulo);
                    }
                }
            }

            return subtitulos;
        }

        public List<tbSubtitulo> ObtenerListasSubPorOid(int subtituloID)
        {
            var listasSub = new List<tbSubtitulo>();
            using (var connection = new SqlConnection(ConexionSqlServer.CN))
            {
                var command = new SqlCommand("SELECT * FROM SUBTITULOS", connection);
                command.Parameters.AddWithValue("@SubtituloID", subtituloID);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listasSub.Add(new tbSubtitulo
                        {
                            SubtituloID = reader.GetInt32(reader.GetOrdinal("SubtituloID")),
                            ListaID = reader.GetInt32(reader.GetOrdinal("ListaID")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Estado = reader.GetBoolean(reader.GetOrdinal("Estado")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion"))
                        });
                    }
                }
            }

            return listasSub;
        }


        public bool RegistrarSubtitulo(tbSubtitulo osubtitulo)
        {
            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarSubtitulo", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ListaID", osubtitulo.ListaID);
                    cmd.Parameters.AddWithValue("@Nombre", osubtitulo.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", osubtitulo.Descripcion);
                    cmd.Parameters.AddWithValue("@Activo", osubtitulo.Estado);
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



        public bool ModificarSubtitulo(tbSubtitulo objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_ModificarSubtitulo", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SubtituloID", objeto.SubtituloID);
                    cmd.Parameters.AddWithValue("@ListaID", objeto.ListaID);
                    cmd.Parameters.AddWithValue("@Nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", objeto.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", objeto.Estado);
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

        public bool EliminarSubtitulo(int subtituloID)
        {
            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(ConexionSqlServer.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_EliminarSubtitulo", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SubtituloID", subtituloID);
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

