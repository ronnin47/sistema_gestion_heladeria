using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace sistema_gestion_heladeria.Models
{
    public class HeladoSabor
    {


        [JsonProperty("id_sabor")]
        public int IdSabor { get; set; }

        public string Nombre { get; set; }

        public decimal Stock { get; set; }

        [JsonProperty("stock_minimo")]
        public decimal StockMinimo { get; set; }

        [JsonProperty("id_proveedor")]
        public int IdProveedor { get; set; }



    }
}

