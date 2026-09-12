using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace sistema_gestion_heladeria.Services
{
    class ApiClient
    {
 
            public static HttpClient Client { get; } = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:3000/")
            };



        //ESTO NO LO ESTA USANDO PERO LO DEJO POR SI LAS DUDAS Y SI SE NECESITA MAS ADELANTE

            public static JsonSerializerOptions JsonOptions { get; } =
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
            }
        
}
