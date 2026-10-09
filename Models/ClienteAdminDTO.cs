using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using sistema_gestion_heladeria.Config;

namespace sistema_gestion_heladeria.Models
{
    // DTO utilizado por Administrador para mostrar y modificar los datos del cliente asociados a una venta.
    // JsonProperty mantiene la correspondencia entre los nombres del JSON de la API y las propiedades de C#.
    public class ClienteAdminDTO
    {
        [JsonProperty("id_venta")] public int IdVenta { get; set; }
        [JsonProperty("fecha")] public DateTime Fecha { get; set; }
        [JsonProperty("nombre")] public string Nombre { get; set; }
        [JsonProperty("telefono")] public string Telefono { get; set; }
        [JsonProperty("direccion")] public string Direccion { get; set; }
    }
}
