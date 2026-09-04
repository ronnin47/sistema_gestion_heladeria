using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using sistema_gestion_heladeria.Models;

namespace sistema_gestion_heladeria.Services
{
    class LoginService
    {
        private readonly HttpClient client = new HttpClient();

        private readonly string apiUrl = "http://localhost:3000";

        public async Task<SessionDataUser> Login(string email, string pass)
        {
            try
            {
                var datos = new
                {
                    email = email,
                    pass = pass
                };

                string json = JsonConvert.SerializeObject(datos);

                var contenido = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response = await client.PostAsync(
                    $"{apiUrl}/login",
                    contenido
                );

                if (!response.IsSuccessStatusCode)
                    return null;

                string respuesta = await response.Content.ReadAsStringAsync();

                var resultado = JsonConvert.DeserializeObject<LoginResponse>(respuesta);

                if (resultado == null || resultado.usuario == null)
                    return null;

                return resultado.usuario;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Login: {ex.Message}");
                return null;
            }
        }

        private class LoginResponse
        {
            public bool login { get; set; }
            public SessionDataUser usuario { get; set; }
        }
    }
}




