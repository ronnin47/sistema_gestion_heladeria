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
using SocketIOClient;
using sistema_gestion_heladeria.Config;


namespace sistema_gestion_heladeria.Views
{

    public partial class PantallaPrincipal : Window
    {

        private SessionDataUser usuario;
        private SocketIOClient.SocketIO socket;


        public PantallaPrincipal(SessionDataUser _user)
        {

            usuario = _user;

            InitializeComponent();


        }


        private async void Window_Loaded (object sender, RoutedEventArgs e)
        {
            CargarDatosUsuario();

           
            //Segun el rol del usuario que userControl le renderiza
           
            await ConectarSocket();

            CargarPanelSegunEstado();

        }




        //*********SOCKET IO*****************
        private async Task ConectarSocket()
        {
            try
            {
                socket = new SocketIOClient.SocketIO( new Uri( ApiConfig.SocketUrl ));

                socket.OnConnected +=
                    (s, args) =>
                    {
                        System.Diagnostics.Debug.WriteLine(
                            "CONECTADO AL SOCKET.IO"
                        );
                    };

                socket.OnError +=
                    (send, en) =>
                    {
                        System.Diagnostics.Debug.WriteLine(
                            "ERROR SOCKET: " + en
                        );
                    };

                await socket.ConnectAsync();

                System.Diagnostics.Debug.WriteLine(
                    "CONNECTASYNC EJECUTADO"
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "CONNECT ERROR: " +
                    ex.Message
                );
            }
        }





        private void CargarPanelSegunEstado()
        {
            switch (usuario.Status.ToLower())
            {
                // Acá van los 4 casos

                case "cajero":
                    PanelContenido.Content = new cajeroControl(usuario, socket);
                    break;

                case "produccion":
                    PanelContenido.Content = new produccionControl(usuario, socket);
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
