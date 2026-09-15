using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistema_gestion_heladeria.Models
{
    public class PedidoActivoDTO
    {
        public int id_venta { get; set; }

        public DateTime fecha { get; set; }

        public decimal total { get; set; }

        public string tipo_entrega { get; set; }

        public string estado { get; set; }

        public string medio_pago { get; set; }

        public string cliente_nombre { get; set; }

        public string direccion { get; set; }

        public string estado_entrega { get; set; }

        public List<ProductoPedidoDTO> productos { get; set; }
    }
}
