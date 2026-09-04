using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistema_gestion_heladeria.Models
{
    public class SessionDataUser
    {
        public int IdUsuario { get; set; }
      
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public string Email { get; set; }
        public string Status { get; set; }

        public string Imagen { get; set; }
    }



}
