using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistema_gestion_heladeria.Models
{
    public class ProductoPedido
    {
        public int id_producto { get; set; }

        public int cantidad { get; set; }

        public decimal precio_unitario { get; set; }

        public decimal subtotal { get; set; }
    }
}
