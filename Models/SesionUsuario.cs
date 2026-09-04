using System.IO;
using System.Text.Json;
using sistema_gestion_heladeria.Models;

namespace sistema_gestion_heladeria
{
    public static class SesionUsuario
    {
        private static readonly string RutaSesion =
            Path.Combine(
                System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.LocalApplicationData),
                "sistema_gestion_heladeria",
                "sesion.json"
            );

        public static bool EstaLogueado()
        {
            return File.Exists(RutaSesion);
        }

        public static void GuardarSesion(
            int idUsuario,
           
            string nombre,
            string apellido,
            string email,
            string status,
            string imagen)
        {
            string carpeta = Path.GetDirectoryName(RutaSesion);

            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            var sesion = new SessionDataUser
            {
                IdUsuario = idUsuario,
               
                Nombre = nombre,
                Apellido = apellido,
                Email = email,
                Status = status,
                Imagen=imagen
            };

            string json = JsonSerializer.Serialize(sesion);

            File.WriteAllText(RutaSesion, json);
        }

        public static SessionDataUser ObtenerSesion()
        {
            if (!File.Exists(RutaSesion))
                return null;

            string json = File.ReadAllText(RutaSesion);

            return JsonSerializer.Deserialize<SessionDataUser>(json);
        }

        public static void CerrarSesion()
        {
            if (File.Exists(RutaSesion))
                File.Delete(RutaSesion);
        }
    }
}