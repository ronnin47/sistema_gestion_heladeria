using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using sistema_gestion_heladeria.Models;
using sistema_gestion_heladeria.Services;

namespace sistema_gestion_heladeria.Controls
{
    public partial class produccionControl : UserControl
    {
        SessionDataUser usuario;

        public produccionControl(SessionDataUser _user)
        {
            usuario = _user;
            InitializeComponent();
            Loaded += produccionControl_Loaded;
        }

        private async void produccionControl_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarPedidos();
        }

        private async Task CargarPedidos()
        {
            try
            {
                //crea una variable de servicion de la clase pedidosService y trae todo de la propia clase, caracteristicas y metodos para usarlos abajo
                //y luego abajo de eso crea una lista de pedidoProduccionDTO llamada pedidos para traer toda la informacion de pedidos usando uno de los
                //metodos que trajo el pedidosService
                PedidosService servicio = new PedidosService();
                List<PedidoProduccionDTO> pedidos = await servicio.ObtenerPedidosProduccion();




                // 1. Agrupar por Venta/Cliente para consolidar productos y sabores, llena una nueva variable con lo que le llego a pedidos arriba
                var pedidosAgrupados = pedidos
                    .GroupBy(p => new { p.IdVenta, p.ClienteNombre, Estado = p.Estado?.Trim() })
                    .Select(grupo => new TarjetaPedidoViewModel(grupo.Key.IdVenta, grupo.Key.ClienteNombre, grupo.Key.Estado, grupo.ToList()))
                    .ToList();



                // 2. Filtrar por estado exacto (o valores equivalentes), con la variable de pedidos agrupados le aplica metodos para listar los que cumplen con cada
                // seccion de pedidos, separandolos en 3 variables, pendientes, enPreparacion y listos
                var pendientes = pedidosAgrupados
                    .Where(t => string.Equals(t.Estado, "Pedido tomado", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                var enPreparacion = pedidosAgrupados
                    .Where(t => string.Equals(t.Estado, "En preparación", StringComparison.OrdinalIgnoreCase)
                             || string.Equals(t.Estado, "En preparacion", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                var listos = pedidosAgrupados
                    .Where(t => string.Equals(t.Estado, "Listo", StringComparison.OrdinalIgnoreCase))
                    .ToList();
                


                // 3. Asignar los orígenes de datos
                icPendientes.ItemsSource = pendientes;
                icEnPreparacion.ItemsSource = enPreparacion;
                icListos.ItemsSource = listos;

                // 4. Actualizar badges superiores
                lblCantPendientes.Text = pendientes.Count.ToString();
                lblCantPreparacion.Text = enPreparacion.Count.ToString();
                lblCantListos.Text = listos.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los pedidos:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ViewModel adaptado para agrupar múltiples ítems dentro de una misma tarjeta
        public class TarjetaPedidoViewModel
        {
            public int IdVenta { get; }
            public string ClienteNombre { get; }
            public string Estado { get; }
            public List<PedidoProduccionDTO> Items { get; }

            public TarjetaPedidoViewModel(int idVenta, string clienteNombre, string estado, List<PedidoProduccionDTO> items)
            {
                IdVenta = idVenta;
                ClienteNombre = clienteNombre;
                Estado = estado;
                Items = items;
            }

            public string HeaderText => $"Pedido #{IdVenta}";

            public string DetalleFormateado
            {
                get
                {
                    List<string> lineas = new List<string>();

                    foreach (var item in Items)
                    {
                        string linea = $"{item.ProductoNombre} x{item.Cantidad}";
                        if (item.Sabores != null && item.Sabores.Count > 0)
                        {
                            linea += $" ({string.Join(", ", item.Sabores)})";
                        }
                        lineas.Add(linea);
                    }

                    return string.Join("\n", lineas);
                }
            }

            // Retorna el primer objeto DTO para identificar la venta en los botones
            public PedidoProduccionDTO PedidoOriginal => Items.FirstOrDefault();
        }

        private async void BtnEmpezar_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var pedido = btn?.Tag as PedidoProduccionDTO;

            if (pedido != null)
            {
                PedidosService servicio = new PedidosService();

                await servicio.CambiarEstado(pedido.IdVenta,"En preparación");

                // TODO: Llamar a tu servicio para cambiar el estado a "En preparación"
                // await servicio.CambiarEstado(pedido.IdVenta, "En preparación");
                await CargarPedidos();
            }
        }

        private async void BtnListo_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var pedido = btn?.Tag as PedidoProduccionDTO;

            if (pedido != null)
            {
                PedidosService servicio = new PedidosService();

                await servicio.CambiarEstado(pedido.IdVenta,"Listo");

                // TODO: Llamar a tu servicio para cambiar el estado a "Listo"
                // await servicio.CambiarEstado(pedido.IdVenta, "Listo");
                await CargarPedidos();
            }
        }
    }

    // Clase auxiliar para formatear tus datos a la vista sin modificar PedidoProduccionDTO
    public class TarjetaPedidoViewModel
    {
        public PedidoProduccionDTO PedidoOriginal { get; }

        public TarjetaPedidoViewModel(PedidoProduccionDTO dto)
        {
            PedidoOriginal = dto;
        }

        public string HeaderText => $"Pedido #{PedidoOriginal.IdVenta}";
        public string ClienteNombre => PedidoOriginal.ClienteNombre;

        public string DetalleFormateado
        {
            get
            {
                string texto = $"{PedidoOriginal.ProductoNombre} x{PedidoOriginal.Cantidad}";

                if (PedidoOriginal.Sabores != null && PedidoOriginal.Sabores.Count > 0)
                {
                    texto += "\nSabores: " + string.Join(", ", PedidoOriginal.Sabores);
                }

                return texto;
            }
        }
    }
}