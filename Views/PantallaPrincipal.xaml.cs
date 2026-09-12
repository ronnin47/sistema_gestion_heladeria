using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using sistema_gestion_heladeria.Models;
using sistema_gestion_heladeria.Controls;
using sistema_gestion_heladeria.Services;


namespace sistema_gestion_heladeria.Views
{

    public partial class PantallaPrincipal : Window
    {

        private SessionDataUser usuario;


        public PantallaPrincipal(SessionDataUser _user)
        {

            usuario = _user;

            InitializeComponent();


        }


        private async void Window_Loaded (object sender, RoutedEventArgs e)
        {
            CargarDatosUsuario();

           
            //Segun el rol del usuario que userControl le renderiza
            CargarPanelSegunEstado();
            
             await ConsumirTodosProductos();
        }

        private async Task ConsumirTodosProductos()
{
    ProductosService productosService = new ProductosService();

    List<Producto> productos = await productosService.ConsumirTodosProductos();

    string mensaje = "";

    foreach (Producto producto in productos)
    {
        mensaje +=
            $"ID: {producto.IdProducto}\n" +
            $"Nombre: {producto.Nombre}\n" +
            $"Descripción: {producto.Descripcion}\n" +
            $"Precio: {producto.Precio}\n" +
            $"Stock: {producto.Stock}\n" +
            $"Categoría: {producto.Categoria}\n" +
            $"Activo: {producto.Activo}\n" +
            $"-------------------------\n";
    }

    if (productos.Count == 0)
    {
        MessageBox.Show("No se encontraron productos.");
        return;
    }

    MessageBox.Show(mensaje, "Productos recibidos");
}


        private void CargarPanelSegunEstado()
        {
            switch (usuario.Status.ToLower())
            {
                // Acá van los 4 casos

                case "cajero":
                    PanelContenido.Content = new cajeroControl();
                    break;

                case "produccion":
                    PanelContenido.Content = new produccionControl();
                    break;
                case "repartidor":
                    PanelContenido.Content = new repartidorControl();
                    break;

                case "administrador":
                    PanelContenido.Content = new administradorControl();
                    break;

            }
        }
        private void CargarDatosUsuario()
        {
            TxtNombre.Text = $"{usuario.Nombre} {usuario.Apellido}";
            TxtEstado.Text = $"Estado de cuenta: {usuario.Status}";
        }

































        private void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {

            //MessageBox.Show("funca el boton cerrar sesion");
            SesionUsuario.CerrarSesion();
            this.Close();

        }



    }
}
