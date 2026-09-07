using System.Windows;
using sistema_gestion_heladeria.Views;

namespace sistema_gestion_heladeria
{
    public partial class App : Application
    {




        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (SesionUsuario.EstaLogueado())
            {
                PantallaPrincipal principal = new PantallaPrincipal();
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