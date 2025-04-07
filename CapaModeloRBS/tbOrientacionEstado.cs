using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
   public class tbOrientacionEstado
    {
        public int OrientacionEstadoID { get; set; }
        public int OrientacionID { get; set; }
        public int EstadoID { get; set; }
        public string Color { get; set; }

        //public string DescripcionOrientacion { get; set; }
        //public string DescripcionEstado { get; set; }
        //Relación con Orientacion
        //public tbOrientacion oOrientacion { get; set; } = new tbOrientacion();
        // Relación con EstadosDeImplementacion
        public tbEstadoDeImplementacion oEstadoDeImplementacion { get; set; } = new tbEstadoDeImplementacion();
    }
}
