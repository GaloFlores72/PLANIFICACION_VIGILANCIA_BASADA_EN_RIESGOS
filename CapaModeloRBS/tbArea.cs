using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
    public class tbArea
    {
        public int AreaID { get; set; }
        public int OrganizacionID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime? FechaCrea { get; set; }
        public string UsuarioModifica { get; set; }
        public DateTime? FechaModifica  { get; set; }
    }
}
