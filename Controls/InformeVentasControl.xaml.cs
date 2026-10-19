using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using sistema_gestion_heladeria.Models;
using sistema_gestion_heladeria.Services;
using Microsoft.Win32;
using sistema_gestion_heladeria.Reports;

namespace sistema_gestion_heladeria.Controls.Administrador
{
    public partial class InformeVentasControl : UserControl
    {
        public InformeVentasControl()
        {
            InitializeComponent();

            DpDesde.SelectedDate = DateTime.Today;
            DpHasta.SelectedDate = DateTime.Today;

            PanelFechas.IsEnabled = false;
        }

        // Servicio encargado de consultar los informes a la API.
        private readonly InformeVentasService servicio =
            new InformeVentasService();

        // Conserva el último informe consultado para generar el PDF.
        private InformeVentasDTO informeActual;

        // Habilita las fechas cuando se elige un período personalizado.
        private void Periodo_Checked(object sender, RoutedEventArgs e)
        {
            if (PanelFechas == null || RbPersonalizado == null)
                return;

            PanelFechas.IsEnabled = RbPersonalizado.IsChecked == true;
        }

        // La consulta a la API se implementará en el siguiente paso.

        private async void BtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            // Determinar el período seleccionado.
            DateTime desde;
            DateTime hasta;

            if (RbHoy.IsChecked == true)
            {
                desde = DateTime.Today;
                hasta = DateTime.Today;
            }
            else
            {
                if (!DpDesde.SelectedDate.HasValue ||
                    !DpHasta.SelectedDate.HasValue)
                {
                    MessageBox.Show(
                        "Seleccioná ambas fechas.",
                        "Informes de ventas",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                desde = DpDesde.SelectedDate.Value.Date;
                hasta = DpHasta.SelectedDate.Value.Date;
            }

            if (desde > hasta)
            {
                MessageBox.Show(
                    "La fecha inicial no puede ser posterior a la final.",
                    "Informes de ventas",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // Evitar que se exporten datos de una consulta anterior.
            informeActual = null;
            BtnGuardarPdf.IsEnabled = false;
            DgVentasInforme.ItemsSource = null;
            TxtCantidadVentas.Text = "—";
            TxtTotalVentas.Text = "—";
            TxtTicketPromedio.Text = "—";

            try
            {
                BtnConsultar.IsEnabled = false;
                TxtEstadoInforme.Text = "Consultando ventas...";

                // Solicitar los datos a la API.
                informeActual = await servicio.ObtenerInforme(desde, hasta);

                // Mostrar los indicadores principales.
                TxtCantidadVentas.Text =
                    informeActual.Resumen.CantidadVentas.ToString();

                TxtTotalVentas.Text =
                    informeActual.Resumen.TotalRegistrado.ToString(
                        "C2", CultureInfo.CurrentCulture);

                TxtTicketPromedio.Text =
                    informeActual.Resumen.TicketPromedio.ToString(
                        "C2", CultureInfo.CurrentCulture);

                // Cargar las ventas en el DataGrid.
                DgVentasInforme.ItemsSource = informeActual.Ventas;

                if (informeActual.Resumen.CantidadVentas == 0)
                {
                    TxtEstadoInforme.Text =
                        "No se registraron ventas en el período seleccionado.";
                }
                else
                {
                    TxtEstadoInforme.Text =
                        "Informe consultado correctamente. Período: " +
                        desde.ToString("dd/MM/yyyy") + " al " +
                        hasta.ToString("dd/MM/yyyy") + ".";
                }

                DgVentasInforme.ItemsSource = informeActual.Ventas;

                // Permitir generar el PDF con el informe consultado.
                BtnGuardarPdf.IsEnabled = true;

            }
            catch (Exception ex)
            {
                informeActual = null;
                BtnGuardarPdf.IsEnabled = false;

                TxtEstadoInforme.Text =
                    "No se pudo obtener el informe de ventas.";

                MessageBox.Show(
                    ex.Message,
                    "Error al consultar el informe",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                // Volver a habilitar la consulta, incluso si ocurre un error.
                BtnConsultar.IsEnabled = true;
            }
        }


        // La generación del PDF se implementará después de conectar la API.

        private void BtnGuardarPdf_Click(object sender, RoutedEventArgs e)
        {
            // Verificar que exista un informe consultado.
            if (informeActual == null)
            {
                MessageBox.Show(
                    "Primero debés consultar un período de ventas.",
                    "Informe de ventas",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            // Proponer un nombre de archivo según el período.
            string nombreArchivo =
                "Informe_Ventas_" +
                informeActual.Periodo.Desde +
                "_a_" +
                informeActual.Periodo.Hasta +
                ".pdf";

            // Abrir la ventana de Windows para elegir dónde guardar.
            SaveFileDialog dialogo = new SaveFileDialog
            {
                Title = "Guardar informe de ventas",
                Filter = "Documento PDF (*.pdf)|*.pdf",
                DefaultExt = ".pdf",
                AddExtension = true,
                FileName = nombreArchivo,
                OverwritePrompt = true
            };

            if (dialogo.ShowDialog() != true)
                return;

            try
            {
                BtnGuardarPdf.IsEnabled = false;

                // Generar el PDF con los datos ya consultados.
                InformeVentasPdf.Generar(
                    informeActual,
                    dialogo.FileName);

                MessageBox.Show(
                    "El informe PDF se guardó correctamente.",
                    "Informe de ventas",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo generar el informe PDF.\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                // Restaurar el botón de exportación después del intento.
                BtnGuardarPdf.IsEnabled = informeActual != null;
            }
        }

    }
}
