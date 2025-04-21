using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
    public class tbDetalleRespuesta
    {
        public int DetalleRespuestaID { get; set; }
        public int RespuestaID { get; set; }
        public int PreguntaID { get; set; }
        public string Estado { get; set; }
        public string Color { get; set; }
        public string Comentario { get; set; }
        public int SubtituloID { get; set; }
        public string NombreSubtitulo { get; set; }
        public string DescripcionPregunta { get; set; }
        public string ReferenciaPregunta { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModifica { get; set; }
        public DateTime FechaModifica { get; set; }
        public string CodigoPregunta { get; set; }

        public List<tbRespuestaOrientacion> oRespuestaOrientacion { get; set; } = new List<tbRespuestaOrientacion>();
    }
}
