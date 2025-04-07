using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatosRBS
{
   public class ConexionSqlServer
    {
        public static string CN = ConfigurationManager.ConnectionStrings["ConnectionStringSqlServer"].ConnectionString;
    }
}
