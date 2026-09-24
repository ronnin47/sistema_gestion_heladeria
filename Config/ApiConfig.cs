using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace sistema_gestion_heladeria.Config
{
    public static class ApiConfig
    {

        public static readonly HttpClient Client = new HttpClient();


        /*
        // Local
        public static readonly string ApiUrl = "http://localhost:3000";

        // Socket
        public static readonly string SocketUrl = "http://localhost:3000";
        */

        // Render
        public static readonly string ApiUrl =
            "https://api-heladeria-groz.onrender.com";

        // Socket.IO
        public static readonly string SocketUrl =
            "https://api-heladeria-groz.onrender.com";

       
    }
}
