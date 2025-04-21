using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVigilanciaBasadaEnRiesgos.Utilidades
{
    public class ArchivoPDF
    {
        public string EvidenciaNombre { get; set; }
        public string Nombre { get; set; }
        public byte[] Contenido { get; set; }
        public string ContentType { get; set; } = "application/pdf";
    }
}