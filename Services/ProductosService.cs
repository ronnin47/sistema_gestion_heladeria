using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using sistema_gestion_heladeria.Models;
using sistema_gestion_heladeria.Config;

namespace sistema_gestion_heladeria.Services
{

    //CLASE PARA TRAER LA INFORMACION DE LOS PRODUCTOS
   public class ProductosService
    {
    

        public async Task<List<Producto>> ConsumirTodosProductos(int idUsuario)
        {
            try
            {
                HttpResponseMessage response = await ApiConfig.Client.GetAsync(
                    $"{ApiConfig.ApiUrl}/productos/{idUsuario}"
                );

                if (!response.IsSuccessStatusCode)
                    return new List<Producto>();

                string respuesta = await response.Content.ReadAsStringAsync();

                var productos = JsonConvert.DeserializeObject<List<Producto>>(
                    respuesta
                );

                return productos ?? new List<Producto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error al obtener productos: {ex.Message}"
                );

                return new List<Producto>();
            }
        }
    }
}