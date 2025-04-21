using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
    public class tbConstatacion
    {
        public int ConstatacionID { get; set; }
        public int RespuestaOrientacionID { get; set; }
        public int AreaID { get; set; }
        public string FechaConstatacion { get; set; }
        public bool? PresuntaInfraccion { get; set; }
        public string DescripcionConstatacion { get; set; }
        public bool? AfectaSO { get; set; }
        public string NotaAfectaSO { get; set; }
        public string EstadoConstatacion { get; set; }
        public DateTime FechaEnvio { get; set; }
        public int UsuarioCreaID { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioModificaID { get; set; }
        public DateTime FechaModifica { get; set; }
        public int UsuarioEnvioId { get; set; }
        public string ObservacionConstatacion { get; set; }
        public tbArea oArea { get; set; } = new tbArea();
        public List<tbEvidencia> oEvidencias { get; set; }
        
    }
}
