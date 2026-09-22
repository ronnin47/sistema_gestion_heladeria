using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using sistema_gestion_heladeria.Config;

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

        public Task<ResumenAdmin> ObtenerResumen() => GetAsync<ResumenAdmin>("/admin/resumen");
        public Task<List<ClienteAdmin>> ObtenerClientes() => GetAsync<List<ClienteAdmin>>("/admin/clientes");
        public Task<object> ModificarCliente(ClienteAdmin c) => EnviarAsync<object>(HttpMethod.Put, $"/admin/clientes/{c.IdVenta}", c);

        public Task<List<ProveedorAdmin>> ObtenerProveedores() => GetAsync<List<ProveedorAdmin>>("/admin/proveedores");
        public Task<ProveedorAdmin> AgregarProveedor(ProveedorAdmin p) => EnviarAsync<ProveedorAdmin>(HttpMethod.Post, "/admin/proveedores", p);
        public Task<ProveedorAdmin> ModificarProveedor(ProveedorAdmin p) => EnviarAsync<ProveedorAdmin>(HttpMethod.Put, $"/admin/proveedores/{p.IdProveedor}", p);
        public Task<object> EliminarProveedor(int id) => EnviarAsync<object>(HttpMethod.Delete, $"/admin/proveedores/{id}", null);

        public Task<List<UsuarioAdmin>> ObtenerUsuarios() => GetAsync<List<UsuarioAdmin>>("/admin/usuarios");
        public Task<UsuarioAdmin> AgregarUsuario(UsuarioAdmin u) => EnviarAsync<UsuarioAdmin>(HttpMethod.Post, "/admin/usuarios", u);
        public Task<UsuarioAdmin> ModificarUsuario(UsuarioAdmin u) => EnviarAsync<UsuarioAdmin>(HttpMethod.Put, $"/admin/usuarios/{u.Id}", u);
        public Task<object> EliminarUsuario(int id) => EnviarAsync<object>(HttpMethod.Delete, $"/admin/usuarios/{id}", null);

        public Task<List<ProductoAdmin>> ObtenerProductos() => GetAsync<List<ProductoAdmin>>("/admin/productos");
        public Task<ProductoAdmin> AgregarProducto(ProductoAdmin p) => EnviarAsync<ProductoAdmin>(HttpMethod.Post, "/admin/productos", p);
        public Task<ProductoAdmin> ModificarProducto(ProductoAdmin p) => EnviarAsync<ProductoAdmin>(HttpMethod.Put, $"/admin/productos/{p.IdProducto}", p);
        public Task<object> EliminarProducto(int id) => EnviarAsync<object>(HttpMethod.Delete, $"/admin/productos/{id}", null);
    }

    public class ClienteAdmin
    {
        [JsonProperty("id_venta")] public int IdVenta { get; set; }
        [JsonProperty("fecha")] public DateTime Fecha { get; set; }
        [JsonProperty("nombre")] public string Nombre { get; set; }
        [JsonProperty("telefono")] public string Telefono { get; set; }
        [JsonProperty("direccion")] public string Direccion { get; set; }
    }

    public class ProveedorAdmin
    {
        [JsonProperty("id_proveedor")] public int IdProveedor { get; set; }
        [JsonProperty("nombre")] public string Nombre { get; set; }
        [JsonProperty("telefono")] public string Telefono { get; set; }
        [JsonProperty("direccion")] public string Direccion { get; set; }
    }

    public class UsuarioAdmin
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("nombre")] public string Nombre { get; set; }
        [JsonProperty("apellido")] public string Apellido { get; set; }
        [JsonProperty("email")] public string Email { get; set; }
        [JsonProperty("pass")] public string Pass { get; set; }
        [JsonProperty("status")] public string Status { get; set; }
        [JsonProperty("imagen")] public string Imagen { get; set; }
    }

    public class ProductoAdmin
    {
        [JsonProperty("id_producto")] public int IdProducto { get; set; }
        [JsonProperty("nombre")] public string Nombre { get; set; }
        [JsonProperty("tipo")] public string Tipo { get; set; }
        [JsonProperty("precio")] public decimal Precio { get; set; }
        [JsonProperty("descripcion")] public string Descripcion { get; set; }
        [JsonProperty("stock")] public int Stock { get; set; }
        [JsonProperty("categoria")] public string Categoria { get; set; }
        [JsonProperty("activo")] public bool Activo { get; set; }
    }

    public class ResumenAdmin
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

    internal class ErrorApi { [JsonProperty("error")] public string Error { get; set; } }
}
