using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistema_gestion_heladeria.Models
{
    public class ProductoPedidoDTO
    {
        public int id_detalle { get; set; }

        public int? id_producto { get; set; }

        public int? id_vc { get; set; }

        public int cantidad { get; set; }

        public decimal precio_unitario { get; set; }

        public decimal subtotal { get; set; }

        public string producto_nombre { get; set; }

        public string producto_tipo { get; set; }

        public string vc_tipo { get; set; }

        public string vc_descripcion { get; set; }

        public List<SaborPedidoDTO> sabores { get; set; }
    }
}
