using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using sistema_gestion_heladeria.Models;
using sistema_gestion_heladeria.Config;
using System.Windows;

namespace sistema_gestion_heladeria.Services
{
   public class PedidosService
    {


        public async Task<bool> InsertarPedido(PedidoCaja pedido)
        {
            try
            {


             
           
                using (HttpClient client = new HttpClient())
                {
                    string json =
                        JsonConvert.SerializeObject(pedido);

                    StringContent contenido =
                        new StringContent(
                            json,
                            Encoding.UTF8,
                            "application/json");

                    HttpResponseMessage respuesta =
                        await client.PostAsync(
                            ApiConfig.ApiUrl + "/insertPedido",
                            contenido);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        return true;
                    }

                    string error =
                        await respuesta.Content.ReadAsStringAsync();

                    Console.WriteLine(
                        "Error al insertar pedido: " +
                        error);

                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Error al insertar pedido: " +
                    ex.Message);

                return false;
            }
        }




        public async Task<List<PedidoActivoDTO>> ObtenerPedidosActivos()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage respuesta =
                        await client.GetAsync(
                            ApiConfig.ApiUrl + "/pedidosActivos");

                    if (respuesta.IsSuccessStatusCode)
                    {
                        string json =
                            await respuesta.Content.ReadAsStringAsync();

                        List<PedidoActivoDTO> pedidos =
                            JsonConvert.DeserializeObject<List<PedidoActivoDTO>>(json);

                        return pedidos;
                    }

                    string error =
                        await respuesta.Content.ReadAsStringAsync();

                    Console.WriteLine(
                        "Error al obtener pedidos activos: " +
                        error);

                    return new List<PedidoActivoDTO>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Error al obtener pedidos activos: " +
                    ex.Message);

                return new List<PedidoActivoDTO>();
            }
        }





    }
}