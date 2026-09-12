using System;
using System.Windows;
using sistema_gestion_heladeria.Services;
using sistema_gestion_heladeria.Models;
using System.Windows.Controls;

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

        private async void BtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            string email = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Password;

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(contrasena))
            {
                txtMensaje.Text = "Completá email y contraseña.";
                return;
            }

            LoginService loginService = new LoginService();

            SessionDataUser usuario = await loginService.Login(email, contrasena);
            //persistencia en memoria local
            if (usuario != null)
            {
                SesionUsuario.GuardarSesion(
                    usuario.IdUsuario,
                    
                    usuario.Nombre,
                    usuario.Apellido,
                    usuario.Email,
                    usuario.Status,
                    usuario.Imagen
                );

                PantallaPrincipal principal = new PantallaPrincipal(usuario);
                principal.Show();

                Close();
                return;
            }

            txtMensaje.Text = "Email o contraseña incorrectos.";
        }


        private void cmbUsuarioPrueba_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = cmbUsuarioPrueba.SelectedItem as ComboBoxItem;

            if (item == null)
                return;

            txtUsuario.Text = item.Tag.ToString();
            txtContrasena.Password = "1234";
        }
        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}