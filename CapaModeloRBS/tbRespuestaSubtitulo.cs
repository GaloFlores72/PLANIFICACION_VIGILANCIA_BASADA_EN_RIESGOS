using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
   public class tbRespuestaSubtitulo
    {
        public int RespuestaID { get; set; }
        public int ListaID { get; set; }
        public int SubtituloID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
       public List<tbDetalleRespuesta> oDetalleRespuesta { get; set; } = new List<tbDetalleRespuesta>();
    }
}
