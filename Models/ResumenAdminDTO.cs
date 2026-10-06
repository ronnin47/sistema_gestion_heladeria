using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using sistema_gestion_heladeria.Config;

namespace sistema_gestion_heladeria.Models
{
    public class ResumenAdminDTO
    {
        [JsonProperty("pedidos_hoy")] public int PedidosHoy { get; set; }
        [JsonProperty("ventas_dia")] public decimal VentasDia { get; set; }
        [JsonProperty("pedidos_curso")] public int PedidosCurso { get; set; }
        [JsonProperty("ticket_promedio")] public decimal TicketPromedio { get; set; }
        [JsonProperty("pedido_tomado")] public int PedidoTomado { get; set; }
        [JsonProperty("preparacion")] public int Preparacion { get; set; }
        [JsonProperty("listos")] public int Listos { get; set; }
        [JsonProperty("camino")] public int Camino { get; set; }
        [JsonProperty("completados")] public int Completados { get; set; }
    }
}
