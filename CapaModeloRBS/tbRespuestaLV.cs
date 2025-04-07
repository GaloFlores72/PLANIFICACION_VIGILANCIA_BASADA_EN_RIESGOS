using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
    public class tbRespuestaLV
    {
        public int RespuestaID { get; set; }
        public int IdTipoProveedorServicio { get; set; }
        public int ListaID { get; set; }
        public int OrganizacionID { get; set; }
        public int UsuarioID { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public string UsuarioCrea { get; set; }

        public DateTime FechaCrea { get; set; }

        public string UsuarioModifica { get; set; }

        public DateTime FechaModifica { get; set; }
        public int InspectorResponsableID { get; set; }
        public string InspectoresResponsables { get; set; }
        public string Certificado { get; set; }

        public string NombreLista { get; set; }
        public string DescripcionLista { get; set; }

        public tbUsuario oUsuario { get; set; } = new tbUsuario();
        public tbOrganizacion oOrganizacion { get; set; } = new tbOrganizacion();

        // Relación con Lista de Verificación        
        public tbListaDeVerificacion oListaDeVerificacion { get; set; }

        public List<tbUsuarioRespuestaLV> oUsuarioRespuestaLV { get; set; }
        public List<tbUsuario> oInpectores { get; set; } = new List<tbUsuario>();


    }
}
