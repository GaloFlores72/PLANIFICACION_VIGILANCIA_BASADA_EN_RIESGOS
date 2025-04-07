using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CapaModeloRBS
{
    public class tbDetalleRespuestaLV
    {
        [Key]
        public int DetalleRespuestaID { get; set; }

        [ForeignKey("RespuestaLV")]
        public int RespuestaID { get; set; }

        [ForeignKey("Pregunta")]
        public int PreguntaID { get; set; }

        [ForeignKey("Orientacion")]
        public int? OrientacionID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Estado { get; set; }

        [MaxLength(1000)]
        public string Comentario { get; set; }

        [ForeignKey("Subtitulo")]
        public int? SubtituloID { get; set; }
        public string NombreSubtitulo { get; set; }
        public string DescripcionPregunta { get; set; }
        public string ReferenciaPregunta { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModifica { get; set; }
        public DateTime FechaModifica { get; set; }

        //public RespuestaLV RespuestaLV { get; set; }
        public tbPregunta Pregunta { get; set; }
       public tbOrientacion Orientacion { get; set; }
        //public Subtitulo Subtitulo { get; set; }
    }
}
