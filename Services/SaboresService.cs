using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using sistema_gestion_heladeria.Models;
using sistema_gestion_heladeria.Config;
using System.Windows;

namespace sistema_gestion_heladeria.Services
{

    //CLASE PARA TRAER LA INFORMACION DE LOS PRODUCTOS
    public class SaboresService
    {


        public async Task<List<HeladoSabor>> ConsumirTodosSabores(int idUsuario)
        {
            try
            {
                HttpResponseMessage response = await ApiConfig.Client.GetAsync(
                    $"{ApiConfig.ApiUrl}/sabores/{idUsuario}"
                );

                if (!response.IsSuccessStatusCode)
                    return new List<HeladoSabor>();

                string respuesta = await response.Content.ReadAsStringAsync();





                var sabores = JsonConvert.DeserializeObject<List<HeladoSabor>>(respuesta);



                return sabores ?? new List<HeladoSabor>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
       ex.ToString(),
       "Error en SaboresService",
       MessageBoxButton.OK,
       MessageBoxImage.Error
   );


                return new List<HeladoSabor>();
            }
        }
    }
}
