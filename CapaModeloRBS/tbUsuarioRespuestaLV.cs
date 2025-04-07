using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
    public class tbUsuarioRespuestaLV
    {
        public int IdUsuarioRespuesta { get; set; }
        public int IdUsuario { get; set; }

        public int IdRespuestaLV { get; set; }

        public string UsuarioCrea { get; set; }

        public DateTime? FechaCrea { get; set; }
        public DateTime? UsuarioModifica { get; set; }
        public DateTime? FechaModifica { get; set; }
    }
}
