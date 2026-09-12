using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace sistema_gestion_heladeria.Models
{
   public class Producto
    {


        [JsonProperty("id_producto")]
        public int IdProducto { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public string Categoria { get; set; }

        public bool Activo { get; set; }
    

    }
}
