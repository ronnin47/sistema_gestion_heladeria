using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using sistema_gestion_heladeria.Config;

namespace sistema_gestion_heladeria.Models
{
    public class ClienteAdminDTO
    {
        [JsonProperty("id_venta")] public int IdVenta { get; set; }
        [JsonProperty("fecha")] public DateTime Fecha { get; set; }
        [JsonProperty("nombre")] public string Nombre { get; set; }
        [JsonProperty("telefono")] public string Telefono { get; set; }
        [JsonProperty("direccion")] public string Direccion { get; set; }
    }
}
