using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapaModeloRBS
{
    public class tbListaDeVerificacion
    {
        public int ListaID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool? Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime? FechaModifica { get; set; }
        public string UsuarioModifica { get; set; }
        public int IdTipoProveedorServicio { get; set; }
        public string DescripcionTipoProveedor { get; set; }
        public tbTipoProveedorServicio oTipoProveedorServicio { get; set; } = new tbTipoProveedorServicio();
        // Relación con Subtitulos
        public List<tbSubtitulo> oSubtitulos { get; set; } = new List<tbSubtitulo>();

    }
}
