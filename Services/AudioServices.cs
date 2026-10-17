using System;
using System.IO;
using System.Windows.Media;

namespace sistema_gestion_heladeria.Services
{
    public static class AudioService
    {
        private static MediaPlayer reproductor = new MediaPlayer();

        public static void Reproducir(string nombreArchivo)
        {
            string ruta = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Sounds",
                nombreArchivo
            );

            if (!File.Exists(ruta))
                return;

            reproductor.Stop();
            reproductor.Open(new Uri(ruta, UriKind.Absolute));
            reproductor.Play();
        }

        public static void ReproducirNotificacion()
        {
            Reproducir("pedidoEntrante.mp3");
        }

        public static void Detener()
        {
            reproductor.Stop();
        }
    }
}

