using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketPagos_Cliente_Servidor_
{
    public class DatosSim
    {
        public string identificacion { get; set; }
        public string servicio { get; set; }
        public string llave { get; set; }

        public string TramaJson { get; set; }

        public DatosSim(string identificacion, string servicio, string llave) {
            
        
               this.identificacion = identificacion;  
               this.servicio = servicio;
               this.llave = llave;
        
        }
    }
}
