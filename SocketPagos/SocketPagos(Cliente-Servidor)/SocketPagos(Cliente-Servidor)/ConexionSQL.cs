using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ConexionSQL
    {

        //String Conexion Emanuel: "Data Source=EMANUELLAPTOP;Initial Catalog=SocketPagosBD;User=EmanuelArce;Password=1234"

        public string connString = "Data Source=EMANUELLAPTOP;Initial Catalog = SocketPagosBD; Integrated Security = TRUE";
        public SqlConnection connection;

        public ConexionSQL()
        {
            connection = new SqlConnection(connString);
        }

        public void AbrirConexion()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                   // Console.WriteLine("Conexión exitosa :)");
                }
                else
                {
                    Console.WriteLine("La conexión ya está abierta.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al abrir la base de datos: " + ex.Message);
            }
        }

        public void CerrarConexion()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                   // Console.WriteLine("Conexión cerrada :)");
                }
                else
                {
                    Console.WriteLine("La conexión ya está cerrada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cerrar la base de datos: " + ex.Message);
            }
        }

        public List<string> ConsultarClientesPorIdentificacion(string identificacion)
        {
            List<string> resultados = new List<string>();
            AbrirConexion();
            try
            {
                
                string query = "SELECT * FROM Cliente WHERE Identificacion = @Identificacion";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Identificacion", identificacion);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    
                    string registro = $"{reader["Identificacion"]}, {reader["CorreoElectronico"]}, {reader["Nombre"]}, {reader["PrimerApellido"]}, {reader["SegundoApellido"]}, {reader["Estado"]} ";
                    resultados.Add(registro);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar la base de datos: " + ex.Message);
            }
            finally
            {
                CerrarConexion();
            }
            return resultados;
        }

        public void CrearBitacora(DateTime FechaActual, string JSON, string IdentificacionCliente, string RespuestaSocket)
        {
            AbrirConexion();

            try
            {
                // Consulta SQL modificada para coincidir con la estructura de la tabla
                string query = "INSERT INTO Bitacora (FechaConsulta, TramaJson, IdentificacionCliente , RespuestaSocket) VALUES (@FechaActual, @JSON, @IdentificacionCliente, @RespuestaSocket)";
                SqlCommand instruccionSQL = new SqlCommand(query, connection);
                instruccionSQL.Parameters.AddWithValue("@FechaActual", FechaActual);
                instruccionSQL.Parameters.AddWithValue("@JSON", JSON);
                instruccionSQL.Parameters.AddWithValue("@IdentificacionCliente", IdentificacionCliente);
                instruccionSQL.Parameters.AddWithValue("@RespuestaSocket", RespuestaSocket);
                instruccionSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar la base de datos: " + ex.Message);
            }
            finally
            {
                CerrarConexion();
            }
        }
    }

}
