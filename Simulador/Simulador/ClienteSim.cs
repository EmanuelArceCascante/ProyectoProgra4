using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text;
using System.Runtime.CompilerServices;

namespace Simulador
{
    public class ClienteSim
    {

        public string identificacion { get; set; }
        public string servicio { get; set; }
        public string llave { get; set; }
        public ClienteSim(string identificacion, string servicio, string llave)
        {
            this.identificacion = identificacion;
            this.servicio = servicio;
            this.llave = llave;
        }
        /*public string Serializar()
        {
            return $"{this.Cliente}|{this.Servicio}|{this.Llave}";
        }
        */

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {

            

         
          
            /*--------------------------------------------------*/
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmSimulador());
        }
    }
}
