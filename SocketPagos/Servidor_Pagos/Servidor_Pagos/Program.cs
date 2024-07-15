using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Servidor_Pagos;

IPAddress localAddr = IPAddress.Parse("127.0.0.1");
TcpListener server = new TcpListener(localAddr, 13000);

        
string serviciosServerAddress = "127.0.0.1";
int serviciosServerPort = 14000;
string bancoServerAddress = "127.0.0.1";
int bancoServerPort = 5000;


server.Start();
Console.WriteLine("Servidor de Pagos iniciado. Esperando conexiones...");

while (true)
{

    TcpClient client = server.AcceptTcpClient();
    Console.WriteLine("Cliente conectado!");
    NetworkStream stream = client.GetStream();
    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
    StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
    string message = reader.ReadLine();
    Console.WriteLine($"Mensaje recibido del cliente: {message}");
    DatosSim clienteData = JsonConvert.DeserializeObject<DatosSim>(message);

    string identificacion = clienteData.identificacion.PadLeft(12, '0');
    string servicio = clienteData.servicio.PadLeft(10, '0').Substring(0, 10);
    string llave = clienteData.llave.PadLeft(20, '0').Substring(0, 20);
    string reciboBuscado = clienteData.recibo;
    string codigo = clienteData.codigo;

    string solicitudServicios2 = $"{identificacion}{servicio}{reciboBuscado}";
    string solicitudServicios1 = $"{identificacion}{servicio}{llave}";

    try
    {
        using (TcpClient serviciosClient = new TcpClient(serviciosServerAddress, serviciosServerPort))
        using (NetworkStream serviciosStream = serviciosClient.GetStream())
        using (StreamWriter serviciosWriter = new StreamWriter(serviciosStream) /*{ AutoFlush = true }*/)
        using (StreamReader serviciosReader = new StreamReader(serviciosStream))
        {
            // Enviar la solicitud
            // Enviar la solicitud
            byte[] messageBytes1 = Encoding.UTF8.GetBytes(solicitudServicios1);
            serviciosStream.Write(messageBytes1, 0, messageBytes1.Length);
            Console.WriteLine($"Enviando solicitud al servidor de servicios: {solicitudServicios1}");
            // Leer la respuesta
            string respuestaServicios = serviciosReader.ReadLine();
            Console.WriteLine($"Respuesta del servidor de servicios: {respuestaServicios}");
            if (respuestaServicios.StartsWith("OK"))
            {
                string listaRecibos = respuestaServicios.Substring(2);
                string[] recibos = listaRecibos.Split(new[] { "][" }, StringSplitOptions.RemoveEmptyEntries);

                string monto = "";
                bool reciboEncontrado = false;

                foreach (string recibo in recibos)
                {
                    string cleanedRecibo = recibo.Trim('[', ']');
                    if (cleanedRecibo.Contains(reciboBuscado))
                    {
                        reciboEncontrado = true;

                        monto = cleanedRecibo.Substring(cleanedRecibo.Length - 8);
                        Console.WriteLine($"Monto a cancelar para el recibo {reciboBuscado}: {monto}");


                        string tipo = monto.Length > 8 ? "TJTA" : "CTA";


                        XDocument xmlDoc = new XDocument(
                            new XElement("pago",
                                new XElement("identificacion", identificacion),
                                new XElement("tipo", tipo),
                                new XElement("monto", monto),
                                new XElement("codigo", codigo)
                            )
                        );
                        string xmlString = xmlDoc.ToString();
                        Console.WriteLine("Trama XML creada: ");
                        Console.WriteLine(xmlString);

                        using (TcpClient bancoClient = new TcpClient(bancoServerAddress, bancoServerPort))
                        using (NetworkStream bancoStream = bancoClient.GetStream())
                        using (StreamWriter bancoWriter = new StreamWriter(bancoStream, Encoding.UTF8) { AutoFlush = true })
                        using (StreamReader bancoReader = new StreamReader(bancoStream, Encoding.UTF8))
                        {
                            bancoWriter.WriteLine(xmlString);
                            Console.WriteLine("Trama XML enviada al Servidor Banco");


                            string respuestaBanco = bancoReader.ReadLine();
                            Console.WriteLine($"Respuesta del Servidor Banco: {respuestaBanco}");

                            if (respuestaBanco == "<ok/>")
                            {

                                string tramaServicios = $"{identificacion.PadLeft(12, '0')}{servicio.PadLeft(10, '0')}{reciboBuscado.PadLeft(10, '0')}";
                                Console.WriteLine("Trama para el servidor de servicios: " + tramaServicios);


                                serviciosWriter.WriteLine(tramaServicios);
                                Console.WriteLine("Trama enviada al servidor de servicios");
                            }

                            writer.WriteLine(respuestaBanco);
                        }
                        break;
                    }
                }

                if (!reciboEncontrado)
                {
                    Console.WriteLine($"El recibo {reciboBuscado} no se encontró en la lista de recibos.");
                    writer.WriteLine("El recibo no se encontró en la lista de recibos.");
                }
            }
            else
            {
                Console.WriteLine("El servidor de servicios no pudo proporcionar la lista de recibos.");
                writer.WriteLine("Error: El servidor de servicios no pudo proporcionar la lista de recibos.");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al conectar con el servidor de Banco: {ex.Message}");
        writer.WriteLine($"Error: {ex.Message}");
    }


    client.Close();
}
