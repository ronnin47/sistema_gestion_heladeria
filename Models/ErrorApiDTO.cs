using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace sistema_gestion_heladeria.Models
{
    // DTO interno utilizado por AdministradorService para leer el mensaje de error devuelto por la API.
    internal class ErrorApiDTO
    {
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}