
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using sistema_gestion_heladeria.Models;
using sistema_gestion_heladeria.Services;





namespace sistema_gestion_heladeria.Controls
{
    public partial class cajeroControl : UserControl
    {
        SessionDataUser usuario;

        List<Producto> productosDisponibles =
            new List<Producto>();

        Dictionary<int, int> cantidadesProductos =
            new Dictionary<int, int>();


       

        string medioPagoSeleccionado = "Efectivo";

        string tipoEntregaSeleccionado = "Local";

        PedidosService pedidosService = new PedidosService();


        private List<PedidoActivoDTO> pedidosActivos = new List<PedidoActivoDTO>();


        private SocketIOClient.SocketIO socket;




        /// <summary>
        ///
        /// </summary>
        /// <param name="_user"></param>
        /// <param name="socket"></param>

        public cajeroControl(SessionDataUser _user, SocketIOClient.SocketIO socket)
        {
            usuario = _user;

         // MessageBox.Show($"id:{usuario.IdUsuario} email:{ usuario.Email}");

            InitializeComponent();


            this.socket = socket;

            EscucharPedidos();

            Loaded += cajeroControl_Loaded;
        }

        private async void cajeroControl_Loaded(
      object sender,
      RoutedEventArgs e)
        {
            await ConsumirTodosProductos();

            pedidosActivos =
                await pedidosService.ObtenerPedidosActivos();


            /*
            MessageBox.Show(
                "Pedidos recibidos: " + pedidosActivos.Count
            );
            */

            await CargarPedidosCompletadosHoy();

            RenderizarPedidosActivos();
        }


        private void EscucharPedidos()
        {
            socket.On(
                "pedido_creado",
                response =>
                {
                    try
                    {
                        var datos =
                            response.GetValue<System.Text.Json.JsonElement>();

                        int idVenta =
                            datos.GetProperty("id_venta").GetInt32();

                        Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show(
                                "Se creó un nuevo pedido.\n\nID venta: " + idVenta,
                                "Pedido creado",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information
                            );
                        });
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            "Error pedido_creado: " + ex.Message
                        );
                    }
                }
            );
        }


        private async Task CargarPedidosCompletadosHoy()
        {
            List<PedidoActivoDTO> pedidosCompletadosHoy =
                await pedidosService.ObtenerPedidosCompletadosHoy();

            txtCompletadosHoy.Text =
                pedidosCompletadosHoy.Count.ToString();
        }
        private void RenderizarPedidosActivos()
        {
            panelPedidosActivos.Children.Clear();

            foreach (PedidoActivoDTO pedido in pedidosActivos)
            {
                Border tarjeta = new Border
                {
                    Margin = new Thickness(4),
                    Padding = new Thickness(8),
                    Background = Brushes.White,
                    BorderBrush = new SolidColorBrush(
                        Color.FromRgb(220, 224, 228)),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(5)
                };

                StackPanel contenido = new StackPanel();


                // CABECERA
                Grid cabecera = new Grid();

                cabecera.ColumnDefinitions.Add(
                    new ColumnDefinition());

                cabecera.ColumnDefinitions.Add(
                    new ColumnDefinition
                    {
                        Width = GridLength.Auto
                    });

                TextBlock numero = new TextBlock
                {
                    Text = "Pedido #" + pedido.id_venta,
                    FontSize = 13,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new SolidColorBrush(
                        Color.FromRgb(45, 50, 55))
                };

                TextBlock hora = new TextBlock
                {
                    Text = pedido.fecha.ToString("HH:mm"),
                    FontSize = 10,
                    Foreground = new SolidColorBrush(
                        Color.FromRgb(120, 125, 130)),
                    VerticalAlignment = VerticalAlignment.Center
                };

                Grid.SetColumn(hora, 1);

                cabecera.Children.Add(numero);
                cabecera.Children.Add(hora);

                contenido.Children.Add(cabecera);


                // CLIENTE
                TextBlock cliente = new TextBlock
                {
                    Text = pedido.cliente_nombre,
                    FontSize = 12,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new SolidColorBrush(
                        Color.FromRgb(45, 50, 55)),
                    Margin = new Thickness(0, 4, 0, 3)
                };

                contenido.Children.Add(cliente);


                // INFORMACIÓN
                Grid informacion = new Grid();

                informacion.ColumnDefinitions.Add(
                    new ColumnDefinition());

                informacion.ColumnDefinitions.Add(
                    new ColumnDefinition());

                TextBlock entrega = new TextBlock
                {
                    Text = pedido.tipo_entrega,
                    FontSize = 10,
                    Foreground = new SolidColorBrush(
                        Color.FromRgb(90, 95, 100))
                };

                TextBlock pago = new TextBlock
                {
                    Text = pedido.medio_pago,
                    FontSize = 10,
                    Foreground = new SolidColorBrush(
                        Color.FromRgb(90, 95, 100)),
                    HorizontalAlignment = HorizontalAlignment.Right
                };

                Grid.SetColumn(pago, 1);

                informacion.Children.Add(entrega);
                informacion.Children.Add(pago);

                contenido.Children.Add(informacion);


                // DIRECCIÓN
                if (!string.IsNullOrEmpty(pedido.direccion))
                {
                    TextBlock direccion = new TextBlock
                    {
                        Text = pedido.direccion,
                        FontSize = 10,
                        Foreground = new SolidColorBrush(
                            Color.FromRgb(110, 115, 120)),
                        Margin = new Thickness(0, 3, 0, 0),
                        TextTrimming = TextTrimming.CharacterEllipsis
                    };

                    contenido.Children.Add(direccion);
                }


                // PIE
                Grid pie = new Grid
                {
                    Margin = new Thickness(0, 5, 0, 0)
                };

                // Estado ocupa solamente lo que necesita
                pie.ColumnDefinitions.Add(
                    new ColumnDefinition
                    {
                        Width = GridLength.Auto
                    });

                // Espacio central
                pie.ColumnDefinitions.Add(
                    new ColumnDefinition
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    });

                // Total
                pie.ColumnDefinitions.Add(
                    new ColumnDefinition
                    {
                        Width = GridLength.Auto
                    });


                // ESTADO
                Border bordeEstado = new Border
                {
                    Background = new SolidColorBrush(
                        Color.FromRgb(245, 247, 249)),
                    BorderBrush = new SolidColorBrush(
                        Color.FromRgb(80, 130, 75)),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(4),
                    Padding = new Thickness(5, 2, 5, 2),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };

                TextBlock estado = new TextBlock
                {
                    Text = pedido.estado,
                    FontSize = 10,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new SolidColorBrush(
                        Color.FromRgb(80, 130, 75))
                };

                bordeEstado.Child = estado;

                Grid.SetColumn(bordeEstado, 0);


                // TOTAL
                TextBlock total = new TextBlock
                {
                    Text = "$ " + pedido.total.ToString("N2"),
                    FontSize = 13,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(
                        Color.FromRgb(45, 50, 55)),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Grid.SetColumn(total, 2);


                pie.Children.Add(bordeEstado);


                // BOTÓN MARCAR ENTREGADO
                if (pedido.estado == "Listo")
                {
                    Button botonEntregado = new Button
                    {
                        Content = "Marcar entregado",
                        FontSize = 10,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = Brushes.White,
                        Background = new SolidColorBrush(
                            Color.FromRgb(190, 65, 65)),
                        BorderBrush = new SolidColorBrush(
                            Color.FromRgb(165, 50, 50)),
                        BorderThickness = new Thickness(1),
                        Padding = new Thickness(7, 3, 7, 3),
                        Margin = new Thickness(8, 0, 8, 0),
                        Cursor = System.Windows.Input.Cursors.Hand,
                        Tag = pedido
                    };

                    botonEntregado.Click += MarcarEntregado_Click;

                    Grid.SetColumn(botonEntregado, 1);

                    pie.Children.Add(botonEntregado);
                }


                pie.Children.Add(total);

                contenido.Children.Add(pie);


                tarjeta.Child = contenido;

                panelPedidosActivos.Children.Add(tarjeta);
            }
        }
        private async Task ConsumirTodosProductos()
        {
            ProductosService productosService =
                new ProductosService();

            productosDisponibles =
                await productosService.ConsumirTodosProductos();

            if (productosDisponibles == null ||
                productosDisponibles.Count == 0)
            {
                MessageBox.Show("No se encontraron productos.");
                return;
            }

            RenderizarProductos();
        }
        private void RenderizarProductos()
        {
            panelProductos.Children.Clear();

            foreach (Producto producto in productosDisponibles)
            {
                Button botonProducto = new Button();
                botonProducto.Width = 125;
                botonProducto.Height = 65;
                botonProducto.Margin = new Thickness(4);
                botonProducto.Padding = new Thickness(6, 4, 6, 4);
                botonProducto.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                botonProducto.Foreground = new SolidColorBrush(Color.FromRgb(45, 50, 55));
                botonProducto.BorderBrush = new SolidColorBrush(Color.FromRgb(220, 224, 228));
                botonProducto.BorderThickness = new Thickness(1);
                botonProducto.FontSize = 12;
                botonProducto.FontWeight = FontWeights.SemiBold;
                botonProducto.Cursor = System.Windows.Input.Cursors.Hand;
                botonProducto.Tag = producto;
                botonProducto.Click += Producto_Click;

                StackPanel contenido = new StackPanel();
                contenido.HorizontalAlignment = HorizontalAlignment.Center;
                contenido.VerticalAlignment = VerticalAlignment.Center;

                TextBlock nombre = new TextBlock();
                nombre.Text = producto.Nombre;
                nombre.FontSize = 12;
                nombre.FontWeight = FontWeights.SemiBold;
                nombre.TextAlignment = TextAlignment.Center;
                nombre.HorizontalAlignment = HorizontalAlignment.Center;

                TextBlock precio = new TextBlock();
                precio.Text = "$ " + producto.Precio.ToString("N2");
                precio.FontSize = 11;
                precio.FontWeight = FontWeights.Normal;
                precio.Foreground = new SolidColorBrush(Color.FromRgb(80, 130, 75));
                precio.Margin = new Thickness(0, 4, 0, 0);
                precio.TextAlignment = TextAlignment.Center;
                precio.HorizontalAlignment = HorizontalAlignment.Center;

                contenido.Children.Add(nombre);
                contenido.Children.Add(precio);

                botonProducto.Content = contenido;
                panelProductos.Children.Add(botonProducto);
            }
        }




        private void Producto_Click(
            object sender,
            RoutedEventArgs e)
        {
            Button boton =
                sender as Button;

            Producto producto =
                boton.Tag as Producto;

            if (producto == null)
                return;

            AgregarProductoAlPedido(producto);
        }

        private void AgregarProductoAlPedido(
            Producto producto)
        {
            if (cantidadesProductos.ContainsKey(
                producto.IdProducto))
            {
                cantidadesProductos[
                    producto.IdProducto]++;
            }
            else
            {
                cantidadesProductos.Add(
                    producto.IdProducto,
                    1);
            }

            RenderizarPedidoActual();
        }

        private void RenderizarPedidoActual()
        {
            panelPedidoActual.Children.Clear();

            decimal total = 0;

            foreach (Producto producto in productosDisponibles)
            {
                if (!cantidadesProductos.ContainsKey(
                    producto.IdProducto))
                {
                    continue;
                }

                int cantidad =
                    cantidadesProductos[
                        producto.IdProducto];

                total += producto.Precio * cantidad;

                CrearFilaPedido(
                    producto,
                    cantidad);
            }

            txtTotalPedido.Text =
                "$ " + total.ToString("N2");
        }

        private void CrearFilaPedido(
            Producto producto,
            int cantidad)
        {
            Border contenedor =
                new Border();

            contenedor.Background =
                new SolidColorBrush(
                    Color.FromRgb(250, 250, 250));

            contenedor.BorderBrush =
                new SolidColorBrush(
                    Color.FromRgb(225, 228, 232));

            contenedor.BorderThickness =
                new Thickness(0, 0, 0, 1);

            contenedor.Padding =
                new Thickness(6, 5, 6, 5);


            Grid fila =
                new Grid();

            fila.Height = 38;


            fila.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });

            fila.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width = GridLength.Auto
                });


            TextBlock nombre =
                new TextBlock();

            nombre.Text =
                producto.Nombre;

            nombre.FontSize = 13;

            nombre.FontWeight =
                FontWeights.SemiBold;

            nombre.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(55, 60, 65));

            nombre.VerticalAlignment =
                VerticalAlignment.Center;

            nombre.Margin =
                new Thickness(4, 0, 10, 0);


            Grid.SetColumn(nombre, 0);

            fila.Children.Add(nombre);


            StackPanel controles =
                new StackPanel();

            controles.Orientation =
                Orientation.Horizontal;

            controles.VerticalAlignment =
                VerticalAlignment.Center;


            Button botonMenos =
                CrearBotonControl("-");

            botonMenos.Tag =
                producto;

            botonMenos.Click +=
                BotonMenos_Click;


            TextBlock textoCantidad =
                new TextBlock();

            textoCantidad.Text =
                cantidad.ToString();

            textoCantidad.Width = 28;

            textoCantidad.TextAlignment =
                TextAlignment.Center;

            textoCantidad.VerticalAlignment =
                VerticalAlignment.Center;

            textoCantidad.FontSize = 13;

            textoCantidad.FontWeight =
                FontWeights.SemiBold;

            textoCantidad.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(50, 55, 60));


            Button botonMas =
                CrearBotonControl("+");

            botonMas.Tag =
                producto;

            botonMas.Click +=
                BotonMas_Click;


            Button botonEliminar =
                CrearBotonControl("×");

            botonEliminar.Width = 28;

            botonEliminar.Margin =
                new Thickness(8, 0, 0, 0);

            botonEliminar.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(190, 65, 65));

            botonEliminar.Tag =
                producto;

            botonEliminar.Click +=
                BotonEliminar_Click;


            controles.Children.Add(botonMenos);
            controles.Children.Add(textoCantidad);
            controles.Children.Add(botonMas);
            controles.Children.Add(botonEliminar);


            Grid.SetColumn(controles, 1);

            fila.Children.Add(controles);


            contenedor.Child = fila;

            panelPedidoActual.Children.Add(
                contenedor);
        }

        private Button CrearBotonControl(
            string texto)
        {
            Button boton =
                new Button();

            boton.Content = texto;

            boton.Width = 28;
            boton.Height = 28;

            boton.Margin =
                new Thickness(2, 0, 2, 0);

            boton.Padding =
                new Thickness(0);

            boton.FontSize = 14;

            boton.FontWeight =
                FontWeights.SemiBold;

            boton.Background =
                new SolidColorBrush(
                    Color.FromRgb(245, 246, 247));

            boton.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(55, 60, 65));

            boton.BorderBrush =
                new SolidColorBrush(
                    Color.FromRgb(215, 219, 223));

            boton.BorderThickness =
                new Thickness(1);

            boton.Cursor =
                System.Windows.Input.Cursors.Hand;

            return boton;
        }

        private void BotonMenos_Click(
            object sender,
            RoutedEventArgs e)
        {
            Button boton =
                sender as Button;

            Producto producto =
                boton.Tag as Producto;

            if (producto == null)
                return;

            if (!cantidadesProductos.ContainsKey(
                producto.IdProducto))
                return;

            cantidadesProductos[
                producto.IdProducto]--;

            if (cantidadesProductos[
                producto.IdProducto] <= 0)
            {
                cantidadesProductos.Remove(
                    producto.IdProducto);
            }

            RenderizarPedidoActual();
        }

        private void BotonMas_Click(
            object sender,
            RoutedEventArgs e)
        {
            Button boton =
                sender as Button;

            Producto producto =
                boton.Tag as Producto;

            if (producto == null)
                return;

            if (!cantidadesProductos.ContainsKey(
                producto.IdProducto))
                return;

            cantidadesProductos[
                producto.IdProducto]++;

            RenderizarPedidoActual();
        }

        private void BotonEliminar_Click(
            object sender,
            RoutedEventArgs e)
        {
            Button boton =
                sender as Button;

            Producto producto =
                boton.Tag as Producto;

            if (producto == null)
                return;

            if (cantidadesProductos.ContainsKey(
                producto.IdProducto))
            {
                cantidadesProductos.Remove(
                    producto.IdProducto);
            }

            RenderizarPedidoActual();
        }



 



        private void MedioPago_Click(
    object sender,
    RoutedEventArgs e)
        {
            Button botonSeleccionado =
                sender as Button;

            if (botonSeleccionado == null)
                return;

            medioPagoSeleccionado =
                botonSeleccionado.Content.ToString();

            Button[] botones =
            {
        btnEfectivo,
        btnTarjeta,
        btnTransferencia,
        btnMercadoPago
    };

            foreach (Button boton in botones)
            {
                boton.Background =
                    new SolidColorBrush(
                        Color.FromRgb(245, 246, 247));

                boton.Foreground =
                    new SolidColorBrush(
                        Color.FromRgb(69, 75, 82));

                boton.BorderBrush =
                    new SolidColorBrush(
                        Color.FromRgb(217, 221, 227));

                boton.FontWeight =
                    FontWeights.Normal;
            }

            botonSeleccionado.Background =
                new SolidColorBrush(
                    Color.FromRgb(63, 125, 74));

            botonSeleccionado.Foreground =
                new SolidColorBrush(
                    Colors.White);

            botonSeleccionado.BorderBrush =
                new SolidColorBrush(
                    Color.FromRgb(53, 107, 63));

            botonSeleccionado.FontWeight =
                FontWeights.SemiBold;
        }




        private PedidoCaja CrearPedidoCaja()
        {
            PedidoCaja pedido =
                new PedidoCaja();

            string nombreCliente =
                txtNombreEntrega.Text.Trim();

            if (string.IsNullOrEmpty(nombreCliente))
            {
                nombreCliente = "Cliente mostrador";
            }

            pedido.cliente_nombre =
                nombreCliente;

            pedido.tipo_entrega =
                tipoEntregaSeleccionado;

            if (tipoEntregaSeleccionado == "Delivery")
            {
                pedido.telefono =
                    txtTelefonoEntrega.Text.Trim();

                pedido.direccion =
                    txtDireccionEntrega.Text.Trim();
            }
            else
            {
                pedido.telefono =
                    null;

                pedido.direccion =
                    null;
            }

            pedido.medio_pago =
                medioPagoSeleccionado;

            pedido.total =
                ObtenerTotalPedido();

            pedido.id =
                usuario.IdUsuario;

            pedido.productos =
                CrearProductosPedido();

            return pedido;
        }

        private decimal ObtenerTotalPedido()
        {
            decimal total = 0;

            foreach (Producto producto in productosDisponibles)
            {
                if (!cantidadesProductos.ContainsKey(
                    producto.IdProducto))
                {
                    continue;
                }

                int cantidad =
                    cantidadesProductos[
                        producto.IdProducto];

                total +=
                    producto.Precio * cantidad;
            }

            return total;
        }


        private List<ProductoPedido> CrearProductosPedido()
        {
            List<ProductoPedido> productos =
                new List<ProductoPedido>();

            foreach (Producto producto in productosDisponibles)
            {
                if (!cantidadesProductos.ContainsKey(
                    producto.IdProducto))
                {
                    continue;
                }

                int cantidad =
                    cantidadesProductos[
                        producto.IdProducto];

                decimal subtotal =
                    producto.Precio * cantidad;

                ProductoPedido productoPedido =
                    new ProductoPedido();

                productoPedido.id_producto =
                    producto.IdProducto;

                productoPedido.id_vc =
                    null;

                productoPedido.cantidad =
                    cantidad;

                productoPedido.precio_unitario =
                    producto.Precio;

                productoPedido.subtotal =
                    subtotal;

                productoPedido.sabores =
                    new List<int>();

                productos.Add(
                    productoPedido);
            }

            return productos;
        }






        private async void MostrarMensajeExito(string mensaje)
        {
            textoMensajeExito.Text = mensaje;

            mensajeExito.Visibility =
                Visibility.Visible;

            await Task.Delay(2500);

            mensajeExito.Visibility =
                Visibility.Collapsed;
        }




        //************************ACA**********************
        private async void CobrarYEnviar_Click(
    object sender,
    RoutedEventArgs e)
        {
            if (cantidadesProductos.Count == 0)
            {
                MessageBox.Show(
                    "No hay productos en el pedido.");

                return;
            }

            PedidoCaja pedido = CrearPedidoCaja();

           

            bool guardado = await pedidosService.InsertarPedido(pedido);

            if (!guardado)
            {
                MessageBox.Show(
                    "No se pudo registrar el pedido.");

                return;
            }
            // PERO SI SALIO BIEN EMIT


            // RECARGAR PEDIDOS ACTIVOS
            pedidosActivos =
                await pedidosService.ObtenerPedidosActivos();

            RenderizarPedidosActivos();




            //esto va ser un mensaje que no interrumpe
            //tira un mensaje y desapare
            MostrarMensajeExito(
      "Pedido enviado a cocina correctamente.");

            cantidadesProductos.Clear();

            RenderizarPedidoActual();

            medioPagoSeleccionado =
                "Efectivo";

            btnEfectivo.RaiseEvent(
                new RoutedEventArgs(
                    Button.ClickEvent));
        }






        private async void MarcarEntregado_Click(
        object sender,
        RoutedEventArgs e)
        {
            Button boton = sender as Button;

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

            if (!cambiado)
            {
                MessageBox.Show(
                    "No se pudo marcar el pedido como entregado.");

                return;
            }

            pedidosActivos =
                await pedidosService.ObtenerPedidosActivos();


            // Actualizar cantidad de pedidos completados hoy
            await CargarPedidosCompletadosHoy();

            RenderizarPedidosActivos();
        }



        private void TipoEntrega_Click(object sender, RoutedEventArgs e)
        {
            Button boton = sender as Button;

            if (boton == null)
                return;

            if (boton == btnEntregaLocal)
            {
                tipoEntregaSeleccionado = "Local";

                btnEntregaLocal.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#3F7D4A"));

                btnEntregaLocal.Foreground = Brushes.White;

                btnDelivery.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#F5F6F7"));

                btnDelivery.Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#454B52"));

                txtNombreEntrega.Visibility = Visibility.Visible;

                lblTelefonoEntrega.Visibility = Visibility.Collapsed;
                txtTelefonoEntrega.Visibility = Visibility.Collapsed;

                lblDireccionEntrega.Visibility = Visibility.Collapsed;
                txtDireccionEntrega.Visibility = Visibility.Collapsed;
            }
            else if (boton == btnDelivery)
            {
                tipoEntregaSeleccionado = "Delivery";

                btnDelivery.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#3F7D4A"));

                btnDelivery.Foreground = Brushes.White;

                btnEntregaLocal.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#F5F6F7"));

                btnEntregaLocal.Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#454B52"));

                txtNombreEntrega.Visibility = Visibility.Visible;

                lblTelefonoEntrega.Visibility = Visibility.Visible;
                txtTelefonoEntrega.Visibility = Visibility.Visible;

                lblDireccionEntrega.Visibility = Visibility.Visible;
                txtDireccionEntrega.Visibility = Visibility.Visible;
            }
        }

    }
}

