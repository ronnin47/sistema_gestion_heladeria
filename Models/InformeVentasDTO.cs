
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace sistema_gestion_heladeria.Models
{
    public class InformeVentasDTO
    {
        [JsonProperty("periodo")]
        public PeriodoInformeDTO Periodo { get; set; }

        [JsonProperty("resumen")]
        public ResumenInformeDTO Resumen { get; set; }

        [JsonProperty("productosVendidos")]
        public List<ProductoVendidoInformeDTO> ProductosVendidos { get; set; }

        [JsonProperty("saboresVendidos")]
        public List<SaborVendidoInformeDTO> SaboresVendidos { get; set; }

        [JsonProperty("ventas")]
        public List<VentaInformeDTO> Ventas { get; set; }
    }

    public class PeriodoInformeDTO
    {
        public string Desde { get; set; }
        public string Hasta { get; set; }
    }

    public class ResumenInformeDTO
    {
        public int CantidadVentas { get; set; }
        public decimal TotalRegistrado { get; set; }
        public decimal TicketPromedio { get; set; }
        public int TotalSeleccionesSabores { get; set; }

        public Dictionary<string, AgrupacionVentasDTO> VentasPorEstado { get; set; }
        public Dictionary<string, AgrupacionVentasDTO> VentasPorMedioPago { get; set; }
        public Dictionary<string, AgrupacionVentasDTO> VentasPorDia { get; set; }
    }

    public class AgrupacionVentasDTO
    {
        public int Cantidad { get; set; }
        public decimal Importe { get; set; }
    }

    public class ProductoVendidoInformeDTO
    {
        [JsonProperty("id_producto")]
        public int IdProducto { get; set; }

        public string Producto { get; set; }

        [JsonProperty("cantidad_vendida")]
        public int CantidadVendida { get; set; }

        [JsonProperty("importe_total")]
        public decimal ImporteTotal { get; set; }
    }

    public class SaborVendidoInformeDTO
    {
        [JsonProperty("id_sabor")]
        public int IdSabor { get; set; }

        public string Sabor { get; set; }

        [JsonProperty("veces_elegido")]
        public int VecesElegido { get; set; }
    }

    public class VentaInformeDTO
    {
        [JsonProperty("id_venta")]
        public int IdVenta { get; set; }

        [JsonProperty("fecha_local")]
        public string FechaLocal { get; set; }

        public decimal Total { get; set; }
        public string Estado { get; set; }

        [JsonProperty("medio_pago")]
        public string MedioPago { get; set; }
    }
}
