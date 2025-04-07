using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
    public class tbEstadoDeImplementacion
    {
        public int EstadoID { get; set; }
        public string Descripcion { get; set; }

        // Relación con OrientacionesEstados
        //public List<tbOrientacionEstado> oOrientacionesEstados { get; set; } = new List<tbOrientacionEstado>();
    }
}
