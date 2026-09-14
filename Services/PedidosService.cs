using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using sistema_gestion_heladeria.Models;

namespace sistema_gestion_heladeria.Services
{
    class PedidosService
    {
        private readonly HttpClient client = new HttpClient();

        private readonly string apiUrl = "http://localhost:3000";


        // Obtener pedidos activos
        public async Task<List<Pedido>> GetPedidosActivosAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(
                    $"{apiUrl}/pedidos/activos"
                );

                if (!response.IsSuccessStatusCode)
                    return new List<Pedido>();

                string respuesta = await response.Content.ReadAsStringAsync();

                var pedidos =
                    JsonConvert.DeserializeObject<List<Pedido>>(
                        respuesta
                    );

                return pedidos ?? new List<Pedido>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error al obtener pedidos activos: {ex.Message}"
                );

                return new List<Pedido>();
            }
        }


        // Obtener cantidad de pedidos completados hoy
        public async Task<int> GetCompletadosHoyAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(
                    $"{apiUrl}/pedidos/completados-hoy"
                );

                if (!response.IsSuccessStatusCode)
                    return 0;

                string respuesta = await response.Content.ReadAsStringAsync();

                var metricas =
                    JsonConvert.DeserializeObject<MetricasHoy>(
                        respuesta
                    );

                return metricas?.completados ?? 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error al obtener pedidos completados: {ex.Message}"
                );

                return 0;
            }
        }


        // Crear pedido
        public async Task<bool> CrearPedidoAsync(Pedido pedido)
        {
            try
            {
                string json = JsonConvert.SerializeObject(pedido);

                var contenido = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response = await client.PostAsync(
                    $"{apiUrl}/pedidos",
                    contenido
                );

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error al crear pedido: {ex.Message}"
                );

                return false;
            }
        }


        // Cambiar estado del pedido
        public async Task<bool> MarcarPedidoComoAsync(
            int idVenta,
            string nuevoEstado
        )
        {
            try
            {
                var datos = new
                {
                    estado = nuevoEstado
                };

                string json = JsonConvert.SerializeObject(datos);

                var contenido = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                var request = new HttpRequestMessage(
                    new HttpMethod("PATCH"),
                    $"{apiUrl}/pedidos/{idVenta}/estado"
                );

                request.Content = contenido;

                HttpResponseMessage response =
                    await client.SendAsync(request);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error al cambiar estado: {ex.Message}"
                );

                return false;
            }
        }
    }
}