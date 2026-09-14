using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistema_gestion_heladeria.Models
{
   public class Pedido
    {
        //LA CLASE PEDIDO REPRESENTA TANTO PEDIDONUEVO QUE SE CREA, COMO LA INFORMACION GENERAL DEL PEDIDO, COMO PEDIDOACTIVO

        // Datos del pedido activo
        public int id_venta { get; set; }

        public DateTime fecha { get; set; }

        public string estado { get; set; }

        public string resumen_productos { get; set; }


        // Datos generales del pedido
        public string cliente_nombre { get; set; }

        public string tipo_entrega { get; set; }

        public string medio_pago { get; set; }

        public decimal total { get; set; }

        public int id_usuario { get; set; }


        // Productos del pedido
        public List<ProductoPedido> productos { get; set; }
    }


  
}

