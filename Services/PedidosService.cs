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


                //Mensaje de salida para verlo nosotros
                MessageBox.Show(
     "ID Usuario recibido: " + pedido.id);
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






    }
}