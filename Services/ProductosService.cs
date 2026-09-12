using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using sistema_gestion_heladeria.Models;

namespace sistema_gestion_heladeria.Services
{

    //CLASE PARA TRAER LA INFORMACION DE LOS PRODUCTOS
   public class ProductosService
    {
        private readonly HttpClient client = new HttpClient();

        private readonly string apiUrl = "http://localhost:3000";

        public async Task<List<Producto>> ConsumirTodosProductos()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(
                    $"{apiUrl}/productos"
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