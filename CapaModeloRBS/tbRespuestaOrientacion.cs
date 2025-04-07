using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
    public class tbRespuestaOrientacion
    {
        public int RespuestaOrientacionID { get; set; }
        public int DetalleRespuestaID { get; set; }
        public int OrientacionID { get; set; }
        public int EstadoImplementacionID { get; set; }
        public string Color { get; set; }
        public string Comentario { get; set; }
        public int UsuarioCreaId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioModificaId { get; set; }
        public DateTime FechaModifica { get; set; }
        public string CodigoPeligro { get; set; }
        public string DescripcionOrientacion { get; set; }
        public List<tbOrientacionEstado> oOrientacionEstado { get; set; } = new List<tbOrientacionEstado>();
    }
}
