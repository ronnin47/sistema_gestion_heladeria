using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace sistema_gestion_heladeria.Controls
{
    public partial class administradorControl : UserControl
    {
        private readonly ObservableCollection<ClienteAdmin> clientes = new ObservableCollection<ClienteAdmin>();
        private readonly ObservableCollection<ProveedorAdmin> proveedores = new ObservableCollection<ProveedorAdmin>();
        private readonly ObservableCollection<EmpleadoAdmin> empleados = new ObservableCollection<EmpleadoAdmin>();
        private readonly ObservableCollection<ProductoAdmin> productos = new ObservableCollection<ProductoAdmin>();

        public administradorControl()
        {
            InitializeComponent();
            CargarDatosEjemplo();
            DgClientes.ItemsSource = clientes;
            DgProveedores.ItemsSource = proveedores;
            DgEmpleados.ItemsSource = empleados;
            DgProductos.ItemsSource = productos;
            CmbEmpleadoRol.SelectedIndex = 0;
            MostrarPanel("resumen");
            CargarResumen();
        }

        private void CargarDatosEjemplo()
        {
            clientes.Add(new ClienteAdmin { Nombre = "Marta Gómez", Telefono = "11-4455-2211", Direccion = "Av. Maipú 1450" });
            clientes.Add(new ClienteAdmin { Nombre = "Julián Ferreyra", Telefono = "11-3322-8890", Direccion = "Chacabuco 220" });
            clientes.Add(new ClienteAdmin { Nombre = "Rocío Beltrán", Telefono = "11-6677-1123", Direccion = "—" });

            proveedores.Add(new ProveedorAdmin { Nombre = "Lácteos del Norte SRL", Rubro = "Materia prima láctea", Telefono = "11-2233-4455" });
            proveedores.Add(new ProveedorAdmin { Nombre = "Envases Vicente López", Rubro = "Packaging", Telefono = "11-5544-3322" });
            proveedores.Add(new ProveedorAdmin { Nombre = "Distribuidora Dulzura", Rubro = "Insumos y toppings", Telefono = "11-7788-9900" });

            empleados.Add(new EmpleadoAdmin { Nombre = "Carla Núñez", Rol = "Cajero / Atención" });
            empleados.Add(new EmpleadoAdmin { Nombre = "Bruno Ibáñez", Rol = "Producción" });
            empleados.Add(new EmpleadoAdmin { Nombre = "Nico Salcedo", Rol = "Repartidor" });
            empleados.Add(new EmpleadoAdmin { Nombre = "Estela Duarte", Rol = "Administración" });

            productos.Add(new ProductoAdmin { Nombre = "Cucurucho simple", Precio = 1800 });
            productos.Add(new ProductoAdmin { Nombre = "Cucurucho doble", Precio = 2600 });
            productos.Add(new ProductoAdmin { Nombre = "1/4 kg", Precio = 4200 });
            productos.Add(new ProductoAdmin { Nombre = "1/2 kg", Precio = 7800 });
            productos.Add(new ProductoAdmin { Nombre = "1 kg", Precio = 14500 });
        }

        private void CargarResumen()
        {
            // Valores visuales equivalentes al prototipo HTML. Luego pueden reemplazarse por datos de la API.
            TxtPedidosHoy.Text = "5";
            TxtVentasDia.Text = "$ 39.500";
            TxtPedidosCurso.Text = "4";
            TxtTicketPromedio.Text = "$ 7.900";
            BarPendientes.Value = 20;
            BarPreparacion.Value = 20;
            BarListos.Value = 40;
            BarCamino.Value = 0;
            BarCompletados.Value = 20;
        }

        private void MostrarPanel(string panel)
        {
            PanelResumen.Visibility = panel == "resumen" ? Visibility.Visible : Visibility.Collapsed;
            PanelClientes.Visibility = panel == "clientes" ? Visibility.Visible : Visibility.Collapsed;
            PanelProveedores.Visibility = panel == "proveedores" ? Visibility.Visible : Visibility.Collapsed;
            PanelEmpleados.Visibility = panel == "empleados" ? Visibility.Visible : Visibility.Collapsed;
            PanelProductos.Visibility = panel == "productos" ? Visibility.Visible : Visibility.Collapsed;

            Button[] botones = { BtnResumen, BtnClientes, BtnProveedores, BtnEmpleados, BtnProductos };
            foreach (Button b in botones)
            {
                b.Background = Brushes.Transparent;
                b.Foreground = new SolidColorBrush(Color.FromRgb(107, 111, 106));
                b.BorderBrush = Brushes.Transparent;
            }

            Button activo = panel == "clientes" ? BtnClientes : panel == "proveedores" ? BtnProveedores :
                            panel == "empleados" ? BtnEmpleados : panel == "productos" ? BtnProductos : BtnResumen;
            activo.Background = Brushes.White;
            activo.Foreground = new SolidColorBrush(Color.FromRgb(29, 33, 30));
            activo.BorderBrush = new SolidColorBrush(Color.FromRgb(226, 224, 216));
        }

        private void BtnResumen_Click(object sender, RoutedEventArgs e) { MostrarPanel("resumen"); }
        private void BtnClientes_Click(object sender, RoutedEventArgs e) { MostrarPanel("clientes"); }
        private void BtnProveedores_Click(object sender, RoutedEventArgs e) { MostrarPanel("proveedores"); }
        private void BtnEmpleados_Click(object sender, RoutedEventArgs e) { MostrarPanel("empleados"); }
        private void BtnProductos_Click(object sender, RoutedEventArgs e) { MostrarPanel("productos"); }

        private void AgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtClienteNombre.Text)) { MessageBox.Show("Ingresá el nombre del cliente."); return; }
            clientes.Add(new ClienteAdmin { Nombre = TxtClienteNombre.Text.Trim(), Telefono = TxtClienteTelefono.Text.Trim(), Direccion = TxtClienteDireccion.Text.Trim() });
            TxtClienteNombre.Clear(); TxtClienteTelefono.Clear(); TxtClienteDireccion.Clear();
        }
        private void EliminarCliente_Click(object sender, RoutedEventArgs e) { if (DgClientes.SelectedItem is ClienteAdmin item) clientes.Remove(item); }

        private void AgregarProveedor_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtProveedorNombre.Text)) { MessageBox.Show("Ingresá el nombre del proveedor."); return; }
            proveedores.Add(new ProveedorAdmin { Nombre = TxtProveedorNombre.Text.Trim(), Rubro = TxtProveedorRubro.Text.Trim(), Telefono = TxtProveedorTelefono.Text.Trim() });
            TxtProveedorNombre.Clear(); TxtProveedorRubro.Clear(); TxtProveedorTelefono.Clear();
        }
        private void EliminarProveedor_Click(object sender, RoutedEventArgs e) { if (DgProveedores.SelectedItem is ProveedorAdmin item) proveedores.Remove(item); }

        private void AgregarEmpleado_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtEmpleadoNombre.Text)) { MessageBox.Show("Ingresá el nombre del empleado."); return; }
            var rol = (CmbEmpleadoRol.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Cajero / Atención";
            empleados.Add(new EmpleadoAdmin { Nombre = TxtEmpleadoNombre.Text.Trim(), Rol = rol });
            TxtEmpleadoNombre.Clear();
        }
        private void EliminarEmpleado_Click(object sender, RoutedEventArgs e) { if (DgEmpleados.SelectedItem is EmpleadoAdmin item) empleados.Remove(item); }

        private void AgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            decimal precio;
            if (string.IsNullOrWhiteSpace(TxtProductoNombre.Text) || !decimal.TryParse(TxtProductoPrecio.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out precio))
            { MessageBox.Show("Completá un nombre y un precio válido."); return; }
            productos.Add(new ProductoAdmin { Nombre = TxtProductoNombre.Text.Trim(), Precio = precio });
            TxtProductoNombre.Clear(); TxtProductoPrecio.Clear();
        }
        private void EliminarProducto_Click(object sender, RoutedEventArgs e) { if (DgProductos.SelectedItem is ProductoAdmin item) productos.Remove(item); }

        private class ClienteAdmin { public string Nombre { get; set; } public string Telefono { get; set; } public string Direccion { get; set; } }
        private class ProveedorAdmin { public string Nombre { get; set; } public string Rubro { get; set; } public string Telefono { get; set; } }
        private class EmpleadoAdmin { public string Nombre { get; set; } public string Rol { get; set; } }
        private class ProductoAdmin { public string Nombre { get; set; } public decimal Precio { get; set; } }
    }
}
