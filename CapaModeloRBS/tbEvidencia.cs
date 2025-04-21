using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
   public class tbEvidencia
    {
        public int EvidenciaID { get; set; }
        public int ConstatacionID { get; set; }
        public string Descripcion { get; set; }
        public string Path { get; set; }
        public int UsuarioCreaID { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioModificaID { get; set; }
        public DateTime FechaModifica { get; set; }


    }
}
