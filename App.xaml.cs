using System.Windows;
using sistema_gestion_heladeria.Views;

using sistema_gestion_heladeria.Models;

namespace sistema_gestion_heladeria
{
    public partial class App : Application
    {




        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (SesionUsuario.EstaLogueado())
            {
                SessionDataUser usuario = SesionUsuario.ObtenerSesion();

                PantallaPrincipal principal = new PantallaPrincipal( usuario);
                principal.Show();
            }
            else
            {
                PantallaLogin login = new PantallaLogin();
                login.Show();
            }
        }
    }
}