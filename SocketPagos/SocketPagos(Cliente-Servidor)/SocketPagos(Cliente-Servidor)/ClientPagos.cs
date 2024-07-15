using Datos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketPagos_Cliente_Servidor_
{

    //Este es el cliente que envia y recibe la solicitud a y de servicios
    //La información que envia la recibe de el ServerClient que recibe y envia info del simulador y al simulador
    public class ClientPagos
    {
        string serverIp = "127.0.0.1"; 
        int port = 8888;
        
        
        TcpClient client = new TcpClient();
        public void ClientePagos (string Id, string servicio, string llave)
        {
            DatosSim ClientDatos = new DatosSim(Id, servicio, llave);
            string Identificacion = ClientDatos.identificacion;
            string Servicio = ClientDatos.servicio;
            string Llave = ClientDatos.llave;

            try
            {

              
                client.Connect(serverIp, port);
                Console.WriteLine("Conectado al servidor servicios.");

                
                NetworkStream stream = client.GetStream();

         

                string IdentificacionNueva = Identificacion.PadLeft(12, '0');
                string ServicioNueva = Servicio.PadLeft(10, '0'); 
                string LlaveNueva = Llave.PadLeft(20, '0'); 

                string Trama = IdentificacionNueva + ServicioNueva + LlaveNueva;
                
                byte[] messageBytes = Encoding.UTF8.GetBytes(Trama);
                stream.Write(messageBytes, 0, messageBytes.Length);
                Console.WriteLine("Mensaje enviado a servicios: {0}", Trama);

               
                byte[] buffer = new byte[256];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);


                //Console.WriteLine("Respuesta recibida de servicios: {0}", response);

                
                string  jsonResponse =  GuardarRespuestaEnArchivo(response);
                MostrarContenidoArchivoJson();
                
                DateTime FechaCnsulta = DateTime.Now;
                ConexionSQL InsertarBitacora1 = new ConexionSQL();
                InsertarBitacora1.CrearBitacora(FechaCnsulta, jsonResponse, Identificacion, response);

                

                
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.ToString());
            }


        }

        private string GuardarRespuestaEnArchivo(string response)
        {
            try
            {
                string jsonResponse;

                if (response.StartsWith("OK"))
                {
                    var recibos = new List<Dictionary<string, object>>();
                    string[] partes = response.Substring(2).Split(new[] { "][" }, StringSplitOptions.None);

                    foreach (var parte in partes)
                    {
                        string cleanedParte = parte.Trim('[', ']');
                        string numeroRecibo = cleanedParte.Substring(0, 10);
                        string fechaVencimiento = cleanedParte.Substring(10, 10);
                        string monto = cleanedParte.Substring(20, 19);
                        monto = monto.Insert(monto.Length - 2, ".");

                        recibos.Add(new Dictionary<string, object>
                        {
                            { "numero", numeroRecibo },
                            { "vencimiento", fechaVencimiento },
                            { "monto", monto }
                        });
                    }

                    var resultado = new
                    {
                        resultado = 0,
                        recibos = recibos
                    };

                     jsonResponse = JsonConvert.SerializeObject(resultado, Formatting.Indented);
                }
                else
                {
                    var errorResponse = new
                    {
                        resultado = -1,
                        descripcion = response.Substring(6) // Assuming the error message is after "ERROR: "
                    };

                     jsonResponse = JsonConvert.SerializeObject(errorResponse, Formatting.Indented);
                }

                string fileName = $"Respuesta_Servicios.json";
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                File.WriteAllText(filePath, jsonResponse);

                Console.WriteLine("Respuesta guardada en archivo: " + filePath);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar la respuesta en el archivo: " + ex.Message);
                return null;
            }

        }
        private void MostrarContenidoArchivoJson()
        {
            try
            {
                string fileName = $"Respuesta_Servicios.json";
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                if (File.Exists(filePath))
                {
                    string jsonContent = File.ReadAllText(filePath);

                    try
                    {
                        var jsonObject = JsonConvert.DeserializeObject(jsonContent);
                        Console.WriteLine("Contenido del archivo JSON:");
                        Console.WriteLine(JsonConvert.SerializeObject(jsonObject, Formatting.Indented));
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine("El contenido del archivo JSON no es válido: " + jsonEx.Message);
                        Console.WriteLine("Contenido sin procesar: ");
                        Console.WriteLine(jsonContent);
                    }
                }
                else
                {
                    Console.WriteLine("El archivo JSON no existe.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer el archivo JSON: " + ex.Message);
            }
        }

    }
}
