
using System;
using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using sistema_gestion_heladeria.Config;
using sistema_gestion_heladeria.Models;

namespace sistema_gestion_heladeria.Services
{
    // Consulta los informes de ventas mediante la API.
    // No modifica registros de la base de datos.
    public class InformeVentasService
    {
        public async Task<InformeVentasDTO> ObtenerInforme(
            DateTime desde, DateTime hasta)
        {
            if (desde.Date > hasta.Date)
                throw new ArgumentException(
                    "La fecha inicial no puede ser posterior a la final.");

            string fechaDesde = desde.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            string fechaHasta = hasta.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            string ruta = "/admin/reportes/ventas"
                + "?desde=" + fechaDesde
                + "&hasta=" + fechaHasta;

            using (var respuesta = await ApiConfig.Client.GetAsync(ApiConfig.ApiUrl + ruta))
            {
                string contenido =
                    await respuesta.Content.ReadAsStringAsync();

                if (!respuesta.IsSuccessStatusCode)
                {
                    throw new Exception(
                        "No se pudo obtener el informe de ventas. "
                        + contenido);
                }

                var informe =
                    JsonConvert.DeserializeObject<InformeVentasDTO>(
                        contenido);

                if (informe == null)
                    throw new Exception(
                        "La API devolvió un informe vacío.");

                return informe;
            }
        }
    }
}
