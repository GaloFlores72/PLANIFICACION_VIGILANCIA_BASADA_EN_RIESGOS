using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
   public class tbSubtitulo
    {
        public int SubtituloID { get; set; }
        public int ListaID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ListaNombre { get; set; }
        public bool? Estado { get; set; }

        public tbListaDeVerificacion oListaVerificacion { get; set; }
        public List<tbPregunta> oListaPreguntas { get; set; } = new List<tbPregunta>();
    }
}
