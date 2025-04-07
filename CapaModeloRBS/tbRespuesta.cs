using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
   public class tbRespuesta
    {
       public int RespuestaID { get; set; }
        public int ListaID { get; set; }
        public int OrganizacionID { get; set; }
        public int UsuarioID { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int InspectorResponsableID { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime FechaCrea { get; set; }
        public string UsuarioModifica { get; set; }
        public DateTime FechaModifica { get; set; }
        public string NombreLista { get; set; }
        public string Certificado { get; set; }
        public string DescripcionLista { get; set; }
        public string NombreInpectores { get; set; }
        public tbUsuario oInspector { get; set; } = new tbUsuario();
        public tbOrganizacion oOrganizacion { get; set; } = new tbOrganizacion();        
        public List<tbUsuario> oInpectores { get; set; } = new List<tbUsuario>();
        public List<tbRespuestaSubtitulo> oRespuestaSubtitulo { get; set; } = new List<tbRespuestaSubtitulo>();
        public tbTipoProveedorServicio oTipoProveedorServicio { get; set; } = new tbTipoProveedorServicio();

    }
}
