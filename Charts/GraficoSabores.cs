using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using sistema_gestion_heladeria.Models;

namespace sistema_gestion_heladeria.Charts
{
    public class GraficoSabores : Canvas
    {
        private List<SaborMasVendidoDTO> datos = new List<SaborMasVendidoDTO>();

        public GraficoSabores()
        {
            SizeChanged += (sender, args) => Dibujar();
        }

        // El ID define el color; nunca depende del orden del filtro.
        // La secuencia de tono usa todos los valores RGB disponibles de manera determinista.
        public static Brush ObtenerColorSabor(int idSabor)
        {
            // Para los primeros sabores se utilizan 11 colores contrastantes y únicos.
            Color[] paleta = {
                Colors.SteelBlue, Colors.Orange, Colors.MediumSeaGreen,
                Colors.MediumPurple, Colors.Crimson, Colors.Goldenrod,
                Colors.Teal, Colors.Coral, Colors.SlateBlue,
                Colors.OliveDrab, Colors.HotPink
            };
            if (idSabor >= 1 && idSabor <= paleta.Length)
                return new SolidColorBrush(paleta[idSabor - 1]);

            // Colores deterministas adicionales para IDs fuera de la paleta.
            // No hay garantía de distinción perceptual para una cantidad ilimitada.
            uint n = unchecked((uint)idSabor) * 2654435761u;
            return new SolidColorBrush(Color.FromRgb(
                (byte)(60 + ((n >> 16) % 170)),
                (byte)(60 + ((n >> 8) % 170)),
                (byte)(60 + (n % 170))));
        }

        public void ActualizarDatos(IEnumerable<SaborMasVendidoDTO> nuevos)
        {
            datos = (nuevos ?? Enumerable.Empty<SaborMasVendidoDTO>())
                .Where(x => x != null && x.Cantidad > 0)
                .OrderBy(x => x.IdSabor).ToList();
            Dibujar();
        }

        private void Dibujar()
        {
            Children.Clear();
            double total = datos.Sum(x => (double)x.Cantidad);
            double radio = Math.Min(ActualWidth, ActualHeight) / 2 - 5;
            if (total <= 0 || radio <= 0) return;
            double cx = ActualWidth / 2, cy = ActualHeight / 2;
            double angulo = -90;
            foreach (var sabor in datos)
            {
                double grados = 360.0 * sabor.Cantidad / total;
                var pincel = ObtenerColorSabor(sabor.IdSabor);
                Shape figura;
                if (grados >= 359.999999)
                {
                    figura = new Ellipse { Width = radio * 2, Height = radio * 2, Fill = pincel, Stroke = Brushes.White, StrokeThickness = 1 };
                    SetLeft(figura, cx - radio);
                    SetTop(figura, cy - radio);
                }
                else
                {
                    double fin = angulo + grados;
                    Point inicio = Punto(cx, cy, radio, angulo);
                    Point extremo = Punto(cx, cy, radio, fin);
                    var geometria = new PathGeometry();
                    var sector = new PathFigure { StartPoint = new Point(cx, cy), IsClosed = true };
                    sector.Segments.Add(new LineSegment(inicio, true));
                    sector.Segments.Add(new ArcSegment(extremo, new Size(radio, radio), 0, grados > 180, SweepDirection.Clockwise, true));
                    geometria.Figures.Add(sector);
                    figura = new Path { Data = geometria, Fill = pincel, Stroke = Brushes.White, StrokeThickness = 1 };
                }
                double porcentaje = sabor.Cantidad * 100.0 / total;
                figura.Cursor = Cursors.Hand;
                ToolTipService.SetInitialShowDelay(figura, 100);
                ToolTipService.SetShowDuration(figura, 15000);
                figura.ToolTip = new ToolTip { Content = sabor.Nombre + "\n" + sabor.Cantidad + " selecciones · " + porcentaje.ToString("N1") + "%" };
                Children.Add(figura);
                angulo += grados;
            }
        }

        private static Point Punto(double cx, double cy, double radio, double grados)
        {
            double radianes = grados * Math.PI / 180.0;
            return new Point(cx + radio * Math.Cos(radianes), cy + radio * Math.Sin(radianes));
        }
    }
}
