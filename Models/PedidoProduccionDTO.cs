using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace sistema_gestion_heladeria.Models
{
    public class PedidoProduccionDTO
    {
        [JsonProperty("id_venta")]
        public int IdVenta { get; set; }

        [JsonProperty("fecha")]
        public DateTime Fecha { get; set; }

        [JsonProperty("tipo_entrega")]
        public string TipoEntrega { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }

        [JsonProperty("cliente_nombre")]
        public string ClienteNombre { get; set; }

        [JsonProperty("id_detalle")]
        public int IdDetalle { get; set; }

        [JsonProperty("id_producto")]
        public int IdProducto { get; set; }

        [JsonProperty("producto_nombre")]
        public string ProductoNombre { get; set; }

        [JsonProperty("cantidad")]
        public int Cantidad { get; set; }

        [JsonProperty("sabores")]
        public List<string> Sabores { get; set; }
    }
}