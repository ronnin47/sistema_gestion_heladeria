using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistema_gestion_heladeria.Models
{
    public class PedidoCaja
    {

        public string cliente_nombre { get; set; }

        public string tipo_entrega { get; set; }

        public string direccion { get; set; }

        public string telefono { get; set; }

        public string medio_pago { get; set; }

        public decimal total { get; set; }

        public int id { get; set; }

        public List<ProductoPedido> productos { get; set; }
    }


  

    public class ProductoPedido
    {
        public int id_producto { get; set; }

        public int? id_vc { get; set; }

        public int cantidad { get; set; }

        public decimal precio_unitario { get; set; }

        public decimal subtotal { get; set; }

        public List<int> sabores { get; set; }
    }
}
