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

namespace sistema_gestion_heladeria.Views
{
    /// <summary>
    /// Lógica de interacción para PantallaLogin.xaml
    /// </summary>
    public partial class PantallaLogin : Window
    {
        public PantallaLogin()
        {
            InitializeComponent();
        }

        private void BtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Password;

            if (string.IsNullOrWhiteSpace(usuario) ||
                string.IsNullOrWhiteSpace(contrasena))
            {
                txtMensaje.Text = "Completá usuario y contraseña.";
                return;
            }

            // Login temporal
            if (usuario == "user" && contrasena == "1234")
            {
                // Guardamos la sesión
                SesionUsuario.GuardarSesion(1, usuario);

                // Abrimos la pantalla principal
                PantallaPrincipal principal = new PantallaPrincipal();
                principal.Show();

                // Cerramos el login
                Close();

                return;
            }

            txtMensaje.Text = "Usuario o contraseña incorrectos.";
        }
        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
