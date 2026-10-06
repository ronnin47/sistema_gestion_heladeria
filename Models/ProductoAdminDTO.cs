using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using sistema_gestion_heladeria.Config;

namespace sistema_gestion_heladeria.Models
{
    public class ProductoAdminDTO
    {
        [JsonProperty("id_producto")] public int IdProducto { get; set; }
        [JsonProperty("nombre")] public string Nombre { get; set; }
        [JsonProperty("tipo")] public string Tipo { get; set; }
        [JsonProperty("precio")] public decimal Precio { get; set; }
        [JsonProperty("descripcion")] public string Descripcion { get; set; }
        [JsonProperty("stock")] public int Stock { get; set; }
        [JsonProperty("categoria")] public string Categoria { get; set; }
        [JsonProperty("activo")] public bool Activo { get; set; }
    }
}
