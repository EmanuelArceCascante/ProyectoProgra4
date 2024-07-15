using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets; 
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;

namespace Simulador
{
    public partial class frmSimulador : Form
    {
        public frmSimulador()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void btnConsultarRecibo_Click(object sender, EventArgs e)
        {
            string cliente = txtUser.Text;
            string servicio = txtServicio.Text;
            string llave = txtLlaveServicio.Text;
            ClienteSim cliente1 = new ClienteSim(cliente, servicio, llave);
            EnviarDatos(cliente1);
           // RealizarPago();
        }
        public void EnviarDatos(ClienteSim cliente1)
        {
            
            string serverIp = "127.0.0.1"; 
            int port = 8080;

          
            TcpClient client = new TcpClient();
            try
            {

               
                client.Connect(serverIp, port);
                Console.WriteLine("Conectado al servidor.");

               
                NetworkStream stream = client.GetStream();

               
                string message = JsonConvert.SerializeObject(cliente1);
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                stream.Write(messageBytes, 0, messageBytes.Length);
                Console.WriteLine("Mensaje enviado: {0}", message);

                
                byte[] buffer = new byte[256];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Respuesta recibida: {0}", response);

                //MessageBox.Show(response, "Respuesta del Servidor", MessageBoxButtons.OK, MessageBoxIcon.Information);

               
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RealizarPago(ClientPago Pago1) {

            
            string serverIp = "127.0.0.1"; 
            int port = 13000;

            TcpClient client = new TcpClient();
            try
            {

                
                client.Connect(serverIp, port);
                Console.WriteLine("Conectado al servidor.");

               
                NetworkStream stream = client.GetStream();

              
                string message = JsonConvert.SerializeObject(Pago1);
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                stream.Write(messageBytes, 0, messageBytes.Length);
                Console.WriteLine("Mensaje enviado: {0}", message);
               
              
                byte[] buffer = new byte[256];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Respuesta recibida: {0}", response);

              //  MessageBox.Show(response, "Respuesta del Servidor", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
                stream.Close();
                   client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnPagar_Click(object sender, EventArgs e)
        {

            string Identificacion = txtIdentificacionPagos.Text;
            string Servicio = txtServicioPagos.Text;
            string Llave = txtLlavePagos.Text;
            string Recibo = txtNumServicioPago.Text;
            string Codigo = txtCodigoSeguridadPago.Text;
            ClientPago Pago1 = new ClientPago(Identificacion, Servicio, Llave, Recibo, Codigo);
            RealizarPago(Pago1);
        }
    }
}
