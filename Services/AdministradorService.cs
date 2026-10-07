using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using sistema_gestion_heladeria.Config;
using sistema_gestion_heladeria.Models;

namespace sistema_gestion_heladeria.Services
{
    public class AdministradorService
    {
        private async Task<T> GetAsync<T>(string ruta)
        {
            var response = await ApiConfig.Client.GetAsync($"{ApiConfig.ApiUrl}{ruta}");
            await ValidarRespuesta(response);
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        private async Task<T> EnviarAsync<T>(HttpMethod metodo, string ruta, object datos)
        {
            var request = new HttpRequestMessage(metodo, $"{ApiConfig.ApiUrl}{ruta}");
            if (datos != null)
                request.Content = new StringContent(JsonConvert.SerializeObject(datos), Encoding.UTF8, "application/json");

            var response = await ApiConfig.Client.SendAsync(request);
            await ValidarRespuesta(response);
            var contenido = await response.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(contenido) ? default(T) : JsonConvert.DeserializeObject<T>(contenido);
        }

        private async Task ValidarRespuesta(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return;
            string contenido = await response.Content.ReadAsStringAsync();
            try
            {
                var error = JsonConvert.DeserializeObject<ErrorApi>(contenido);
                throw new Exception(error != null && !string.IsNullOrWhiteSpace(error.Error) ? error.Error : "Error al comunicarse con la API");
            }
            catch (JsonException)
            {
                throw new Exception("Error al comunicarse con la API");
            }
        }

        public Task<ResumenAdminDTO> ObtenerResumen() => GetAsync<ResumenAdminDTO>("/admin/resumen");
        public Task<List<ClienteAdminDTO>> ObtenerClientes() => GetAsync<List<ClienteAdminDTO>>("/admin/clientes");
        public Task<object> ModificarCliente(ClienteAdminDTO c) => EnviarAsync<object>(HttpMethod.Put, $"/admin/clientes/{c.IdVenta}", c);

        public Task<List<ProveedorAdminDTO>> ObtenerProveedores() => GetAsync<List<ProveedorAdminDTO>>("/admin/proveedores");
        public Task<ProveedorAdminDTO> AgregarProveedor(ProveedorAdminDTO p) => EnviarAsync<ProveedorAdminDTO>(HttpMethod.Post, "/admin/proveedores", p);
        public Task<ProveedorAdminDTO> ModificarProveedor(ProveedorAdminDTO p) => EnviarAsync<ProveedorAdminDTO>(HttpMethod.Put, $"/admin/proveedores/{p.IdProveedor}", p);
        public Task<object> EliminarProveedor(int id) => EnviarAsync<object>(HttpMethod.Delete, $"/admin/proveedores/{id}", null);

        public Task<List<UsuarioAdminDTO>> ObtenerUsuarios() => GetAsync<List<UsuarioAdminDTO>>("/admin/usuarios");
        public Task<UsuarioAdminDTO> AgregarUsuario(UsuarioAdminDTO u) => EnviarAsync<UsuarioAdminDTO>(HttpMethod.Post, "/admin/usuarios", u);
        public Task<UsuarioAdminDTO> ModificarUsuario(UsuarioAdminDTO u) => EnviarAsync<UsuarioAdminDTO>(HttpMethod.Put, $"/admin/usuarios/{u.Id}", u);
        public Task<object> EliminarUsuario(int id) => EnviarAsync<object>(HttpMethod.Delete, $"/admin/usuarios/{id}", null);

        public Task<List<ProductoAdminDTO>> ObtenerProductos() => GetAsync<List<ProductoAdminDTO>>("/admin/productos");
        public Task<ProductoAdminDTO> AgregarProducto(ProductoAdminDTO p) => EnviarAsync<ProductoAdminDTO>(HttpMethod.Post, "/admin/productos", p);
        public Task<ProductoAdminDTO> ModificarProducto(ProductoAdminDTO p) => EnviarAsync<ProductoAdminDTO>(HttpMethod.Put, $"/admin/productos/{p.IdProducto}", p);
        public Task<object> EliminarProducto(int id) => EnviarAsync<object>(HttpMethod.Delete, $"/admin/productos/{id}", null);
    }

  

    



   

   

    internal class ErrorApi { [JsonProperty("error")] public string Error { get; set; } }
}
