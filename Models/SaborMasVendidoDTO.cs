using Newtonsoft.Json;

namespace sistema_gestion_heladeria.Models
{
    public class SaborMasVendidoDTO
    {
        [JsonProperty("id_sabor")]
        public int IdSabor { get; set; }
        [JsonProperty("nombre")]
        public string Nombre { get; set; }
        [JsonProperty("cantidad")]
        public int Cantidad { get; set; }
    }
}
