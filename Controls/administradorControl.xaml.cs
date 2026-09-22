using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using sistema_gestion_heladeria.Services;

namespace sistema_gestion_heladeria.Controls
{
    public partial class administradorControl : UserControl
    {
        private readonly AdministradorService servicio = new AdministradorService();
        private readonly ObservableCollection<ClienteAdmin> clientes = new ObservableCollection<ClienteAdmin>();
        private readonly ObservableCollection<ProveedorAdmin> proveedores = new ObservableCollection<ProveedorAdmin>();
        private readonly ObservableCollection<UsuarioAdmin> empleados = new ObservableCollection<UsuarioAdmin>();
        private readonly ObservableCollection<ProductoAdmin> productos = new ObservableCollection<ProductoAdmin>();

        public administradorControl()
        {
            InitializeComponent();
            DgClientes.ItemsSource = clientes;
            DgProveedores.ItemsSource = proveedores;
            DgEmpleados.ItemsSource = empleados;
            DgProductos.ItemsSource = productos;
            CmbEmpleadoRol.SelectedIndex = 0;
            MostrarPanel("resumen");
            Loaded += AdministradorControl_Loaded;
        }

        private async void AdministradorControl_Loaded(object sender, RoutedEventArgs e)
        {
            await RecargarTodo();
        }

        private async System.Threading.Tasks.Task RecargarTodo()
        {
            try
            {
                var resumen = await servicio.ObtenerResumen();
                CargarResumen(resumen);
                Reemplazar(clientes, await servicio.ObtenerClientes());
                Reemplazar(proveedores, await servicio.ObtenerProveedores());
                Reemplazar(empleados, await servicio.ObtenerUsuarios());
                Reemplazar(productos, await servicio.ObtenerProductos());
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los datos de administración.\n" + ex.Message, "Administrador", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Reemplazar<T>(ObservableCollection<T> destino, System.Collections.Generic.IEnumerable<T> datos)
        {
            destino.Clear();
            if (datos == null) return;
            foreach (var item in datos) destino.Add(item);
        }

        private void CargarResumen(ResumenAdmin r)
        {
            if (r == null) return;
            TxtPedidosHoy.Text = r.PedidosHoy.ToString();
            TxtVentasDia.Text = r.VentasDia.ToString("C0", CultureInfo.CurrentCulture);
            TxtPedidosCurso.Text = r.PedidosCurso.ToString();
            TxtTicketPromedio.Text = r.TicketPromedio.ToString("C0", CultureInfo.CurrentCulture);

            int totalEstados = r.PedidoTomado + r.Preparacion + r.Listos + r.Camino + r.Completados;
            BarPendientes.Value = Porcentaje(r.PedidoTomado, totalEstados);
            BarPreparacion.Value = Porcentaje(r.Preparacion, totalEstados);
            BarListos.Value = Porcentaje(r.Listos, totalEstados);
            BarCamino.Value = Porcentaje(r.Camino, totalEstados);
            BarCompletados.Value = Porcentaje(r.Completados, totalEstados);
        }

        private double Porcentaje(int valor, int total) => total == 0 ? 0 : (valor * 100.0 / total);

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

        private void DgClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var c = DgClientes.SelectedItem as ClienteAdmin;
            if (c == null) return;
            TxtClienteNombre.Text = c.Nombre;
            TxtClienteTelefono.Text = c.Telefono;
            TxtClienteDireccion.Text = c.Direccion;
        }

        private async void ModificarCliente_Click(object sender, RoutedEventArgs e)
        {
            var c = DgClientes.SelectedItem as ClienteAdmin;
            if (c == null) { MessageBox.Show("Seleccioná una venta/cliente."); return; }
            if (string.IsNullOrWhiteSpace(TxtClienteNombre.Text)) { MessageBox.Show("El nombre es obligatorio."); return; }
            c.Nombre = TxtClienteNombre.Text.Trim(); c.Telefono = TxtClienteTelefono.Text.Trim(); c.Direccion = TxtClienteDireccion.Text.Trim();
            await Ejecutar(async () => { await servicio.ModificarCliente(c); Reemplazar(clientes, await servicio.ObtenerClientes()); }, "Datos del cliente modificados.");
        }

        private void DgProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var p = DgProveedores.SelectedItem as ProveedorAdmin;
            if (p == null) return;
            TxtProveedorNombre.Text = p.Nombre; TxtProveedorTelefono.Text = p.Telefono; TxtProveedorDireccion.Text = p.Direccion;
        }

        private ProveedorAdmin LeerProveedor()
        {
            if (string.IsNullOrWhiteSpace(TxtProveedorNombre.Text)) throw new Exception("El nombre del proveedor es obligatorio.");
            return new ProveedorAdmin { Nombre = TxtProveedorNombre.Text.Trim(), Telefono = TxtProveedorTelefono.Text.Trim(), Direccion = TxtProveedorDireccion.Text.Trim() };
        }

        private async void AgregarProveedor_Click(object sender, RoutedEventArgs e)
        {
            try { var p = LeerProveedor(); await Ejecutar(async () => { await servicio.AgregarProveedor(p); Reemplazar(proveedores, await servicio.ObtenerProveedores()); LimpiarProveedor(); }, "Proveedor agregado."); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void ModificarProveedor_Click(object sender, RoutedEventArgs e)
        {
            var seleccionado = DgProveedores.SelectedItem as ProveedorAdmin;
            if (seleccionado == null) { MessageBox.Show("Seleccioná un proveedor."); return; }
            try { var p = LeerProveedor(); p.IdProveedor = seleccionado.IdProveedor; await Ejecutar(async () => { await servicio.ModificarProveedor(p); Reemplazar(proveedores, await servicio.ObtenerProveedores()); LimpiarProveedor(); }, "Proveedor modificado."); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void EliminarProveedor_Click(object sender, RoutedEventArgs e)
        {
            var p = DgProveedores.SelectedItem as ProveedorAdmin;
            if (p == null) { MessageBox.Show("Seleccioná un proveedor."); return; }
            if (!Confirmar("¿Eliminar el proveedor seleccionado?")) return;
            await Ejecutar(async () => { await servicio.EliminarProveedor(p.IdProveedor); Reemplazar(proveedores, await servicio.ObtenerProveedores()); LimpiarProveedor(); }, "Proveedor eliminado.");
        }

        private void LimpiarProveedor() { TxtProveedorNombre.Clear(); TxtProveedorTelefono.Clear(); TxtProveedorDireccion.Clear(); DgProveedores.SelectedItem = null; }

        private void DgEmpleados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var u = DgEmpleados.SelectedItem as UsuarioAdmin;
            if (u == null) return;
            TxtEmpleadoNombre.Text = u.Nombre; TxtEmpleadoApellido.Text = u.Apellido; TxtEmpleadoEmail.Text = u.Email; TxtEmpleadoPass.Password = u.Pass ?? ""; TxtEmpleadoImagen.Text = u.Imagen;
            foreach (ComboBoxItem item in CmbEmpleadoRol.Items)
                if (string.Equals(item.Tag as string, u.Status, StringComparison.OrdinalIgnoreCase)) { CmbEmpleadoRol.SelectedItem = item; break; }
        }

        private UsuarioAdmin LeerUsuario()
        {
            var rol = CmbEmpleadoRol.SelectedItem as ComboBoxItem;
            var status = rol == null ? null : rol.Tag as string;
            if (string.IsNullOrWhiteSpace(TxtEmpleadoNombre.Text) || string.IsNullOrWhiteSpace(TxtEmpleadoApellido.Text) || string.IsNullOrWhiteSpace(TxtEmpleadoEmail.Text) || string.IsNullOrWhiteSpace(TxtEmpleadoPass.Password) || string.IsNullOrWhiteSpace(status))
                throw new Exception("Nombre, apellido, email, contraseña y estado/rol son obligatorios.");
            return new UsuarioAdmin { Nombre = TxtEmpleadoNombre.Text.Trim(), Apellido = TxtEmpleadoApellido.Text.Trim(), Email = TxtEmpleadoEmail.Text.Trim(), Pass = TxtEmpleadoPass.Password, Status = status, Imagen = TxtEmpleadoImagen.Text.Trim() };
        }

        private async void AgregarEmpleado_Click(object sender, RoutedEventArgs e)
        {
            try { var u = LeerUsuario(); await Ejecutar(async () => { await servicio.AgregarUsuario(u); Reemplazar(empleados, await servicio.ObtenerUsuarios()); LimpiarEmpleado(); }, "Usuario agregado."); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void ModificarEmpleado_Click(object sender, RoutedEventArgs e)
        {
            var seleccionado = DgEmpleados.SelectedItem as UsuarioAdmin;
            if (seleccionado == null) { MessageBox.Show("Seleccioná un usuario."); return; }
            try { var u = LeerUsuario(); u.Id = seleccionado.Id; await Ejecutar(async () => { await servicio.ModificarUsuario(u); Reemplazar(empleados, await servicio.ObtenerUsuarios()); LimpiarEmpleado(); }, "Usuario modificado."); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void EliminarEmpleado_Click(object sender, RoutedEventArgs e)
        {
            var u = DgEmpleados.SelectedItem as UsuarioAdmin;
            if (u == null) { MessageBox.Show("Seleccioná un usuario."); return; }
            if (!Confirmar("¿Eliminar el usuario seleccionado?")) return;
            await Ejecutar(async () => { await servicio.EliminarUsuario(u.Id); Reemplazar(empleados, await servicio.ObtenerUsuarios()); LimpiarEmpleado(); }, "Usuario eliminado.");
        }

        private void LimpiarEmpleado() { TxtEmpleadoNombre.Clear(); TxtEmpleadoApellido.Clear(); TxtEmpleadoEmail.Clear(); TxtEmpleadoPass.Clear(); TxtEmpleadoImagen.Clear(); CmbEmpleadoRol.SelectedIndex = 0; DgEmpleados.SelectedItem = null; }

        private void DgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var p = DgProductos.SelectedItem as ProductoAdmin;
            if (p == null) return;
            TxtProductoNombre.Text = p.Nombre; TxtProductoTipo.Text = p.Tipo; TxtProductoPrecio.Text = p.Precio.ToString(CultureInfo.CurrentCulture); TxtProductoDescripcion.Text = p.Descripcion; TxtProductoStock.Text = p.Stock.ToString(); TxtProductoCategoria.Text = p.Categoria; ChkProductoActivo.IsChecked = p.Activo;
        }

        private ProductoAdmin LeerProducto()
        {
            decimal precio; int stock;
            if (string.IsNullOrWhiteSpace(TxtProductoNombre.Text) || !decimal.TryParse(TxtProductoPrecio.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out precio) || !int.TryParse(TxtProductoStock.Text, out stock))
                throw new Exception("Completá nombre, precio y stock con valores válidos.");
            return new ProductoAdmin { Nombre = TxtProductoNombre.Text.Trim(), Tipo = TxtProductoTipo.Text.Trim(), Precio = precio, Descripcion = TxtProductoDescripcion.Text.Trim(), Stock = stock, Categoria = TxtProductoCategoria.Text.Trim(), Activo = ChkProductoActivo.IsChecked == true };
        }

        private async void AgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            try { var p = LeerProducto(); await Ejecutar(async () => { await servicio.AgregarProducto(p); Reemplazar(productos, await servicio.ObtenerProductos()); LimpiarProducto(); }, "Producto agregado."); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void ModificarProducto_Click(object sender, RoutedEventArgs e)
        {
            var seleccionado = DgProductos.SelectedItem as ProductoAdmin;
            if (seleccionado == null) { MessageBox.Show("Seleccioná un producto."); return; }
            try { var p = LeerProducto(); p.IdProducto = seleccionado.IdProducto; await Ejecutar(async () => { await servicio.ModificarProducto(p); Reemplazar(productos, await servicio.ObtenerProductos()); LimpiarProducto(); }, "Producto modificado."); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void EliminarProducto_Click(object sender, RoutedEventArgs e)
        {
            var p = DgProductos.SelectedItem as ProductoAdmin;
            if (p == null) { MessageBox.Show("Seleccioná un producto."); return; }
            if (!Confirmar("¿Eliminar el producto seleccionado?")) return;
            await Ejecutar(async () => { await servicio.EliminarProducto(p.IdProducto); Reemplazar(productos, await servicio.ObtenerProductos()); LimpiarProducto(); }, "Producto eliminado.");
        }

        private void LimpiarProducto() { TxtProductoNombre.Clear(); TxtProductoTipo.Clear(); TxtProductoPrecio.Clear(); TxtProductoDescripcion.Clear(); TxtProductoStock.Clear(); TxtProductoCategoria.Clear(); ChkProductoActivo.IsChecked = true; DgProductos.SelectedItem = null; }

        private bool Confirmar(string mensaje) => MessageBox.Show(mensaje, "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;

        private async System.Threading.Tasks.Task Ejecutar(Func<System.Threading.Tasks.Task> accion, string mensajeExito)
        {
            try { await accion(); MessageBox.Show(mensajeExito, "Administrador", MessageBoxButton.OK, MessageBoxImage.Information); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Administrador", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
