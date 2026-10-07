using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using sistema_gestion_heladeria.Config;

namespace sistema_gestion_heladeria.Models
{
    // DTO utilizado por Administrador para gestionar los usuarios/empleados del sistema.
    public class UsuarioAdminDTO
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("nombre")] public string Nombre { get; set; }
        [JsonProperty("apellido")] public string Apellido { get; set; }
        [JsonProperty("email")] public string Email { get; set; }
        [JsonProperty("pass")] public string Pass { get; set; }
        [JsonProperty("status")] public string Status { get; set; }
        [JsonProperty("imagen")] public string Imagen { get; set; }
    }
}
