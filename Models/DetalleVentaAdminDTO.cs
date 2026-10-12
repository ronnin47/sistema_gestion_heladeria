
using Newtonsoft.Json;
using System.Collections.Generic;

namespace sistema_gestion_heladeria.Models
{
    // Representa un sabor seleccionado en un producto vendido.
    public class SaborVentaAdminDTO
    {
        [JsonProperty("id_sabor")]
        public int IdSabor { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }
    }

    // Representa un producto incluido en una venta.
    public class ProductoVentaAdminDTO
    {
        [JsonProperty("id_detalle")]
        public int IdDetalle { get; set; }

        [JsonProperty("id_producto")]
        public int? IdProducto { get; set; }

        [JsonProperty("producto_nombre")]
        public string ProductoNombre { get; set; }

        [JsonProperty("producto_tipo")]
        public string ProductoTipo { get; set; }

        [JsonProperty("vc_tipo")]
        public string VcTipo { get; set; }

        [JsonProperty("vc_descripcion")]
        public string VcDescripcion { get; set; }

        [JsonProperty("cantidad")]
        public int Cantidad { get; set; }

        [JsonProperty("precio_unitario")]
        public decimal PrecioUnitario { get; set; }

        [JsonProperty("subtotal")]
        public decimal Subtotal { get; set; }

        [JsonProperty("sabores")]
        public List<SaborVentaAdminDTO> Sabores { get; set; }
            = new List<SaborVentaAdminDTO>();
    }

    // Representa la respuesta completa del endpoint.
    public class DetalleVentaAdminDTO
    {
        [JsonProperty("id_venta")]
        public int IdVenta { get; set; }

        [JsonProperty("productos")]
        public List<ProductoVentaAdminDTO> Productos { get; set; }
            = new List<ProductoVentaAdminDTO>();
    }
}
