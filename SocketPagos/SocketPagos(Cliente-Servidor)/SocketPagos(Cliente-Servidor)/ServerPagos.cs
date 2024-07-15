using Datos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
namespace SocketPagos_Cliente_Servidor_
{
    public class ServerPagos
    {
        public Socket escuchar;
        public IPEndPoint connect;
        public int port;
        public string host;

        public ServerPagos (string host, int port)
        {
            this.host = host;
            this.port = port; 
            
            escuchar = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connect = new IPEndPoint(IPAddress.Parse(host), port);
        }



        public void Iniciar()
        {
            
            escuchar.Bind(connect);
            escuchar.Listen(10); 

            Console.WriteLine("Servidor escuchando en {0}:{1}", connect.Address, connect.Port);

            
            while (true)
            {
                Socket conexion = escuchar.Accept();
                Console.WriteLine("Cliente conectado.");

                
                ManejarCliente(conexion);
            }
        }

        private void ManejarCliente(Socket clienteSocket)
        {
            string response = string.Empty;
            try
            {
                
                byte[] buffer = new byte[1024];
                int bytesRead = clienteSocket.Receive(buffer);
                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Recibido: {0}", request);
                if (string.IsNullOrEmpty(request))
                {
                    response = "No se permiten datos vacíos.";
                }
                else
                {
                    DatosSim JsonCliente = new DatosSim(request, null, null);
                    JsonCliente.TramaJson = request;
                    
                    DatosSim clienteData = JsonConvert.DeserializeObject<DatosSim>(request);

                    if (clienteData != null)
                    {
                        string Identificacion = clienteData.identificacion;
                        string servicio = clienteData.servicio;
                        string llave = clienteData.llave;

                   
                        ConexionSQL conexionSQL = new ConexionSQL();
                        List<string> resultados = conexionSQL.ConsultarClientesPorIdentificacion(Identificacion);
                        if (resultados != null && resultados.Count > 0) 
                        {
                            ClientPagos Cliente = new ClientPagos();
                            Cliente.ClientePagos(Identificacion, servicio, llave);
                            
                            string fileName = $"Respuesta_Servicios.txt";
                            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                            if (File.Exists(filePath))
                            {
                                string fileContent = File.ReadAllText(filePath);

                             
                                var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContent);

                                if (jsonResponse != null && jsonResponse.ContainsKey("result"))
                                {
                                    var result = jsonResponse["result"] as JObject;
                                    if (result != null)
                                    {
                                        string numeroRecibo = result["numero_recibo"]?.ToString() ?? "N/A";
                                        string fechaVencimiento = result["fechavencimiento"]?.ToString() ?? "N/A";
                                        string monto = result["monto"]?.ToString() ?? "N/A";

                                        response = $"Número de Recibo: {numeroRecibo}, Fecha de Vencimiento: {fechaVencimiento}, Monto: {monto}";
                                        
                                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                                        clienteSocket.Send(responseBytes);
                                        Console.WriteLine("Respuesta enviada.");
                                    }
                                    else if (jsonResponse != null && jsonResponse.ContainsKey("status") && jsonResponse.ContainsKey("message"))
                                    {
                                        string status = jsonResponse["status"]?.ToString();
                                        string message = jsonResponse["message"]?.ToString();

                                        if (status == "ERROR")
                                        {
                                            response = $"Estado: {status}, Mensaje: {message}";
                                           
                                            byte[] responseBytes2 = Encoding.UTF8.GetBytes(response);
                                            clienteSocket.Send(responseBytes2);
                                            //Console.WriteLine("Respuesta enviada.");
                                        }
                                        else
                                        {
                                            response = "Formato del archivo JSON incorrecto.";
                                        }
                                    }
                                }
                                else if (jsonResponse != null && jsonResponse.ContainsKey("status") && jsonResponse.ContainsKey("message"))
                                {
                                    string status = jsonResponse["status"]?.ToString();
                                    string message = jsonResponse["message"]?.ToString();

                                    if (status == "ERROR")
                                    {
                                        response = $"Estado: {status}, Mensaje: {message}";
                                        
                                        byte[] responseBytes2 = Encoding.UTF8.GetBytes(response);
                                        clienteSocket.Send(responseBytes2);
                                        //Console.WriteLine("Respuesta enviada.");
                                    }
                                    else
                                    {
                                        response = "Formato del archivo JSON incorrecto.";
                                    }
                                }
                            }
                            else
                            {
                                response = "No se pudo encontrar el archivo de respuesta.";
                            }
                        }
                        else
                        {
                            Console.WriteLine("Cliente no registrado.");
                         

                        }
                  
                    }

                    
                    else
                    {
                        Console.WriteLine("Datos recibidos en un formato incorrecto.");
                        response = "Datos recibidos en un formato incorrecto.";
                    }
                }
               
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
            finally
            {
            
                clienteSocket.Shutdown(SocketShutdown.Both);
                clienteSocket.Close();
            }
        }

        static void Main(string[] args)
        {
            ServerPagos conectarServerPagos = new ServerPagos("127.0.0.1", 8080);
            conectarServerPagos.Iniciar();
        }
    }
}