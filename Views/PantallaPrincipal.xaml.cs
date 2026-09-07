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

namespace sistema_gestion_heladeria.Views
{
    /// <summary>
    /// Lógica de interacción para PantallaPrincipal.xaml
    /// </summary>
    public partial class PantallaPrincipal : Window
    {

        private SessionDataUser usuario;


        public PantallaPrincipal(SessionDataUser _user)
        {

            usuario= _user;

            InitializeComponent();

            CargarDatosUsuario();
        }

        private void CargarDatosUsuario()
        {
            TxtNombre.Text = $"{usuario.Nombre} {usuario.Apellido}";
            TxtEstado.Text = $"Estado de cuenta: {usuario.Status}";
        }

        private void CerrarSesion_Click(object sender, RoutedEventArgs e) {

            //MessageBox.Show("funca el boton cerrar secion");

            //tenemos que llamar el metodo cerrar sesion de la clase session usuario, es un metodo estatico

            SesionUsuario.CerrarSesion();

            this.Close();

        }



    }
}
