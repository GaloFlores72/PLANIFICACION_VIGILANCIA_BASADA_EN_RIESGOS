using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
   public class tbPregunta
    {
        public int PreguntaID { get; set; }
        public int SubtituloID { get; set; }
        public string Descripcion { get; set; }
        public string Referencia { get; set; }
        public string Estado { get; set; }
        public int? Estadisticas { get; set; }
        // Relación con Orientaciones
        public List<tbOrientacion> oListaOrientaciones { get; set; } = new List<tbOrientacion>();
    }
}
