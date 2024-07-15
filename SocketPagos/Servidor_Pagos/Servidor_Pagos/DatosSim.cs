using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servidor_Pagos
{
    public class DatosSim
    {
        public string identificacion { get; set; }
        public string servicio { get; set; }
        public string llave { get; set; }
        public string recibo { get; set; }
        public string codigo { get; set; }
        public DatosSim(string identificacion, string servicio, string llave, string recibo, string codigo)
        {
            this.identificacion = identificacion;
            this.servicio = servicio;
            this.llave = llave;
            this.recibo = recibo;
            this.codigo = codigo;
        }
    }
}
