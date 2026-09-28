using sistema_gestion_heladeria.Models;
using sistema_gestion_heladeria.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace sistema_gestion_heladeria.Controls
{
    public partial class repartidorControl : UserControl
    {
        private PedidosService pedidosService = new PedidosService();

        private List<PedidoActivoDTO> pedidosActivos =
            new List<PedidoActivoDTO>();

        // Pedidos entregados durante esta sesión
        private List<PedidoActivoDTO> pedidosEntregados =
            new List<PedidoActivoDTO>();

        public repartidorControl()
        {
            InitializeComponent();

            Loaded += RepartidorControl_Loaded;
        }

        private async void RepartidorControl_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            await CargarPedidos();
        }

        // =========================================================
        // CARGAR PEDIDOS
        // =========================================================

        private async System.Threading.Tasks.Task CargarPedidos()
        {
            pedidosActivos =
                await pedidosService.ObtenerPedidosActivos();

            RenderizarPedidos();
        }

        // =========================================================
        // MOSTRAR PEDIDOS
        // =========================================================

        private void RenderizarPedidos()
        {
            PedidosItemsControl.ItemTemplate = null;
            PedidosEntregadosItemsControl.ItemTemplate = null;

            PedidosItemsControl.Items.Clear();
            PedidosEntregadosItemsControl.Items.Clear();

            // =====================================================
            // PEDIDOS ACTIVOS
            // SOLO DELIVERY
            // =====================================================

            foreach (PedidoActivoDTO pedido in pedidosActivos)
            {
                // SOLO MOSTRAR PEDIDOS DE DELIVERY
                if (pedido.tipo_entrega != "Delivery")
                    continue;

                // Si ya está entregado no va a la izquierda
                if (pedido.estado == "Entregado")
                    continue;

                Border tarjeta =
                    CrearTarjetaPedido(pedido);

                PedidosItemsControl.Items.Add(tarjeta);
            }

            // =====================================================
            // PEDIDOS ENTREGADOS
            // SOLO DELIVERY
            // =====================================================

            foreach (PedidoActivoDTO pedido in pedidosEntregados)
            {
                // SOLO MOSTRAR PEDIDOS DE DELIVERY
                if (pedido.tipo_entrega != "Delivery")
                    continue;

                Border tarjeta =
                    CrearTarjetaEntregado(pedido);

                PedidosEntregadosItemsControl.Items.Add(tarjeta);
            }
        }

        // =========================================================
        // CREAR TARJETA DEL PEDIDO
        // =========================================================

        private Border CrearTarjetaPedido(PedidoActivoDTO pedido)
        {
            Border tarjeta = new Border();

            tarjeta.Background = Brushes.White;
            tarjeta.CornerRadius = new CornerRadius(12);
            tarjeta.Margin = new Thickness(0, 0, 0, 15);
            tarjeta.Padding = new Thickness(20);

            Grid contenido = new Grid();

            ColumnDefinition columnaInfo =
                new ColumnDefinition();

            columnaInfo.Width =
                new GridLength(1, GridUnitType.Star);

            ColumnDefinition columnaBotones =
                new ColumnDefinition();

            columnaBotones.Width =
                GridLength.Auto;

            contenido.ColumnDefinitions.Add(columnaInfo);
            contenido.ColumnDefinitions.Add(columnaBotones);

            // =====================================================
            // INFORMACIÓN DEL PEDIDO
            // =====================================================

            StackPanel informacion = new StackPanel();

            // ID
            TextBlock numero = new TextBlock();

            numero.Text =
                "Pedido #" + pedido.id_venta;

            numero.FontSize = 20;
            numero.FontWeight = FontWeights.Bold;

            numero.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(31, 41, 55));

            informacion.Children.Add(numero);

            // HORA
            TextBlock hora = new TextBlock();

            hora.Text =
                "Hora: " +
                pedido.fecha.ToString("HH:mm");

            hora.FontSize = 14;

            hora.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(107, 114, 128));

            hora.Margin =
                new Thickness(0, 5, 0, 0);

            informacion.Children.Add(hora);

            // CLIENTE
            TextBlock cliente = new TextBlock();

            cliente.Text =
                "Cliente: " +
                pedido.cliente_nombre;

            cliente.FontSize = 15;

            cliente.Margin =
                new Thickness(0, 10, 0, 0);

            cliente.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(55, 65, 81));

            informacion.Children.Add(cliente);

            // TIPO DE ENTREGA
            TextBlock tipoEntrega =
                new TextBlock();

            tipoEntrega.Text =
                "Entrega: " +
                pedido.tipo_entrega;

            tipoEntrega.FontSize = 14;

            tipoEntrega.Margin =
                new Thickness(0, 5, 0, 0);

            tipoEntrega.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(107, 114, 128));

            informacion.Children.Add(tipoEntrega);

            // DIRECCIÓN
            if (!string.IsNullOrWhiteSpace(
                pedido.direccion))
            {
                TextBlock direccion =
                    new TextBlock();

                direccion.Text =
                    "Dirección: " +
                    pedido.direccion;

                direccion.FontSize = 14;

                direccion.Margin =
                    new Thickness(0, 5, 0, 0);

                direccion.Foreground =
                    new SolidColorBrush(
                        Color.FromRgb(107, 114, 128));

                direccion.TextWrapping =
                    TextWrapping.Wrap;

                informacion.Children.Add(direccion);
            }

            // MEDIO DE PAGO
            TextBlock medioPago =
                new TextBlock();

            medioPago.Text =
                "Medio de pago: " +
                pedido.medio_pago;

            medioPago.FontSize = 14;

            medioPago.Margin =
                new Thickness(0, 5, 0, 0);

            medioPago.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(107, 114, 128));

            informacion.Children.Add(medioPago);

            // =====================================================
            // DETALLE DEL PEDIDO
            // =====================================================

            Border detalle = new Border();

            detalle.Background =
                new SolidColorBrush(
                    Color.FromRgb(249, 250, 251));

            detalle.CornerRadius =
                new CornerRadius(8);

            detalle.Padding =
                new Thickness(12);

            detalle.Margin =
                new Thickness(0, 10, 0, 10);

            StackPanel detallePanel =
                new StackPanel();

            TextBlock tituloDetalle =
                new TextBlock();

            tituloDetalle.Text =
                "Detalle del pedido";

            tituloDetalle.FontWeight =
                FontWeights.Bold;

            tituloDetalle.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(55, 65, 81));

            detallePanel.Children.Add(
                tituloDetalle);

            if (pedido.productos != null)
            {
                foreach (
                    ProductoPedidoDTO producto
                    in pedido.productos)
                {
                    TextBlock productoTexto =
                        new TextBlock();

                    string texto =
                        producto.cantidad +
                        " x " +
                        producto.producto_nombre;

                    // Tipo de vaso / cucurucho
                    if (!string.IsNullOrWhiteSpace(
                        producto.vc_descripcion))
                    {
                        texto +=
                            " (" +
                            producto.vc_descripcion +
                            ")";
                    }

                    // Sabores
                    if (producto.sabores != null &&
                        producto.sabores.Count > 0)
                    {
                        string sabores =
                            string.Join(
                                ", ",
                                producto.sabores.Select(
                                    s => s.nombre));

                        texto +=
                            " - Sabores: " +
                            sabores;
                    }

                    productoTexto.Text =
                        texto;

                    productoTexto.TextWrapping =
                        TextWrapping.Wrap;

                    productoTexto.Margin =
                        new Thickness(0, 5, 0, 0);

                    productoTexto.Foreground =
                        new SolidColorBrush(
                            Color.FromRgb(
                                107, 114, 128));

                    detallePanel.Children.Add(
                        productoTexto);
                }
            }

            detalle.Child =
                detallePanel;

            informacion.Children.Add(
                detalle);

            // =====================================================
            // TOTAL
            // =====================================================

            TextBlock total =
                new TextBlock();

            total.Text =
                "Total: $" +
                pedido.total.ToString("N2");

            total.FontSize = 16;

            total.FontWeight =
                FontWeights.Bold;

            total.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(31, 41, 55));

            total.Margin =
                new Thickness(0, 0, 0, 10);

            informacion.Children.Add(
                total);

            // =====================================================
            // ESTADO
            // =====================================================

            StackPanel estadoPanel =
                new StackPanel();

            estadoPanel.Orientation =
                Orientation.Horizontal;

            TextBlock estadoTexto =
                new TextBlock();

            estadoTexto.Text =
                "Estado:";

            estadoTexto.FontWeight =
                FontWeights.Bold;

            estadoTexto.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(55, 65, 81));

            estadoPanel.Children.Add(
                estadoTexto);

            Border estado =
                new Border();

            estado.Background =
                new SolidColorBrush(
                    Color.FromRgb(254, 243, 199));

            estado.CornerRadius =
                new CornerRadius(15);

            estado.Padding =
                new Thickness(12, 5, 12, 5);

            estado.Margin =
                new Thickness(10, 0, 0, 0);

            TextBlock estadoValor =
                new TextBlock();

            estadoValor.Text =
                pedido.estado;

            estadoValor.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(146, 64, 14));

            estadoValor.FontWeight =
                FontWeights.Bold;

            estadoValor.FontSize = 12;

            estado.Child =
                estadoValor;

            estadoPanel.Children.Add(
                estado);

            informacion.Children.Add(
                estadoPanel);

            Grid.SetColumn(
                informacion,
                0);

            contenido.Children.Add(
                informacion);

            // =====================================================
            // BOTONES
            // =====================================================

            StackPanel botones =
                new StackPanel();

            botones.VerticalAlignment =
                VerticalAlignment.Bottom;

            botones.HorizontalAlignment =
                HorizontalAlignment.Right;

            botones.Margin =
                new Thickness(20, 0, 0, 0);

            // -----------------------------------------------------
            // EN CAMINO
            // -----------------------------------------------------

            if (pedido.estado != "En camino")
            {
                Button btnCamino =
                    new Button();

                btnCamino.Content =
                    "En camino hacia domicilio";

                btnCamino.Background =
                    new SolidColorBrush(
                        Color.FromRgb(245, 158, 11));

                btnCamino.Foreground =
                    Brushes.White;

                btnCamino.FontWeight =
                    FontWeights.SemiBold;

                btnCamino.FontSize = 14;

                btnCamino.Padding =
                    new Thickness(15, 8, 15, 8);

                btnCamino.Margin =
                    new Thickness(5, 0, 5, 5);

                btnCamino.BorderThickness =
                    new Thickness(0);

                btnCamino.Tag =
                    pedido;

                btnCamino.Click +=
                    BtnCamino_Click;

                botones.Children.Add(
                    btnCamino);
            }

            // -----------------------------------------------------
            // ENTREGADO
            // -----------------------------------------------------

            Button btnEntregado =
                new Button();

            btnEntregado.Content =
                "Entregado";

            btnEntregado.Background =
                new SolidColorBrush(
                    Color.FromRgb(16, 185, 129));

            btnEntregado.Foreground =
                Brushes.White;

            btnEntregado.FontWeight =
                FontWeights.SemiBold;

            btnEntregado.FontSize = 14;

            btnEntregado.Padding =
                new Thickness(15, 8, 15, 8);

            btnEntregado.Margin =
                new Thickness(5, 5, 5, 0);

            btnEntregado.BorderThickness =
                new Thickness(0);

            btnEntregado.Tag =
                pedido;

            btnEntregado.Click +=
                BtnEntregado_Click;

            botones.Children.Add(
                btnEntregado);

            Grid.SetColumn(
                botones,
                1);

            contenido.Children.Add(
                botones);

            tarjeta.Child =
                contenido;

            return tarjeta;
        }

        // =========================================================
        // TARJETA DE PEDIDO ENTREGADO
        // =========================================================

        private Border CrearTarjetaEntregado(
            PedidoActivoDTO pedido)
        {
            Border tarjeta =
                new Border();

            tarjeta.Background =
                Brushes.White;

            tarjeta.CornerRadius =
                new CornerRadius(10);

            tarjeta.Margin =
                new Thickness(0, 0, 0, 12);

            tarjeta.Padding =
                new Thickness(15);

            tarjeta.BorderBrush =
                new SolidColorBrush(
                    Color.FromRgb(167, 243, 208));

            tarjeta.BorderThickness =
                new Thickness(1);

            StackPanel contenido =
                new StackPanel();

            // PEDIDO
            TextBlock numero =
                new TextBlock();

            numero.Text =
                "Pedido #" +
                pedido.id_venta;

            numero.FontSize = 17;

            numero.FontWeight =
                FontWeights.Bold;

            numero.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(6, 95, 70));

            contenido.Children.Add(
                numero);

            // HORA
            TextBlock hora =
                new TextBlock();

            hora.Text =
                "Hora: " +
                pedido.fecha.ToString("HH:mm");

            hora.FontSize = 13;

            hora.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(107, 114, 128));

            hora.Margin =
                new Thickness(0, 5, 0, 0);

            contenido.Children.Add(
                hora);

            // CLIENTE
            TextBlock cliente =
                new TextBlock();

            cliente.Text =
                "Cliente: " +
                pedido.cliente_nombre;

            cliente.FontSize = 14;

            cliente.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(107, 114, 128));

            cliente.Margin =
                new Thickness(0, 5, 0, 0);

            contenido.Children.Add(
                cliente);

            // DIRECCIÓN
            if (!string.IsNullOrWhiteSpace(
                pedido.direccion))
            {
                TextBlock direccion =
                    new TextBlock();

                direccion.Text =
                    "Dirección: " +
                    pedido.direccion;

                direccion.FontSize = 13;

                direccion.Foreground =
                    new SolidColorBrush(
                        Color.FromRgb(
                            107, 114, 128));

                direccion.TextWrapping =
                    TextWrapping.Wrap;

                direccion.Margin =
                    new Thickness(0, 3, 0, 0);

                contenido.Children.Add(
                    direccion);
            }

            // TOTAL
            TextBlock total =
                new TextBlock();

            total.Text =
                "Total: $" +
                pedido.total.ToString("N2");

            total.FontSize = 14;

            total.FontWeight =
                FontWeights.Bold;

            total.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(55, 65, 81));

            total.Margin =
                new Thickness(0, 8, 0, 0);

            contenido.Children.Add(
                total);

            // ENTREGADO
            TextBlock entregado =
                new TextBlock();

            entregado.Text =
                "✓ Entregado";

            entregado.FontWeight =
                FontWeights.Bold;

            entregado.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(6, 95, 70));

            entregado.Margin =
                new Thickness(0, 8, 0, 0);

            contenido.Children.Add(
                entregado);

            tarjeta.Child =
                contenido;

            return tarjeta;
        }

        // =========================================================
        // BOTÓN EN CAMINO
        // =========================================================

        private async void BtnCamino_Click(
            object sender,
            RoutedEventArgs e)
        {
            Button boton =
                sender as Button;

            if (boton == null)
                return;

            PedidoActivoDTO pedido =
                boton.Tag as PedidoActivoDTO;

            if (pedido == null)
                return;

            bool cambiado =
                await pedidosService.CambiarEstado(
                    pedido.id_venta,
                    "En camino");

            if (cambiado)
            {
                pedido.estado =
                    "En camino";

                RenderizarPedidos();
            }
            else
            {
                MessageBox.Show(
                    "No se pudo cambiar el estado del pedido.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // =========================================================
        // BOTÓN ENTREGADO
        // =========================================================

        private async void BtnEntregado_Click(
            object sender,
            RoutedEventArgs e)
        {
            Button boton =
                sender as Button;

            if (boton == null)
                return;

            PedidoActivoDTO pedido =
                boton.Tag as PedidoActivoDTO;

            if (pedido == null)
                return;

            bool cambiado =
                await pedidosService.CambiarEstado(
                    pedido.id_venta,
                    "Entregado");

            if (cambiado)
            {
                pedido.estado =
                    "Entregado";

                pedidosActivos.RemoveAll(
                    p => p.id_venta ==
                         pedido.id_venta);

                if (!pedidosEntregados.Any(
                    p => p.id_venta ==
                         pedido.id_venta))
                {
                    pedidosEntregados.Add(
                        pedido);
                }

                RenderizarPedidos();
            }
            else
            {
                MessageBox.Show(
                    "No se pudo marcar el pedido como entregado.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // =========================================================
        // ACTUALIZAR
        // =========================================================

        private async void BtnActualizar_Click(
            object sender,
            RoutedEventArgs e)
        {
            await CargarPedidos();
        }
    }
}