using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
   public class tbOrientacion
    {
        public int OrientacionID { get; set; }
        public int PreguntaID { get; set; }
        public string CodigoPeligro { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        
        // Relación con OrientacionesEstados
        public List<tbOrientacionEstado> oOrientacionesEstados { get; set; } = new List<tbOrientacionEstado>();
        public List<tbConstatacion> oContataciones { get; set; } = new List<tbConstatacion>();
    }
}
