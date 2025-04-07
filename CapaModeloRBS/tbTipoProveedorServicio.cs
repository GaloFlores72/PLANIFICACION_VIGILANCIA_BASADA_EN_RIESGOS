using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
    public class tbTipoProveedorServicio
    {
        public int IdTipoProveedor { get; set; }

        public string DescripcionTipoProveedor { get; set; }
        public bool Estado { get; set; } = true;

        public string UsuarioCrea { get; set; }

        public DateTime FechaCrea { get; set; } 

        public string UsuarioModifica { get; set; }

        public DateTime? FechaModifica { get; set; }

       public List<tbListaDeVerificacion> oListaDeVerificacion { get; set; }

    }
}
