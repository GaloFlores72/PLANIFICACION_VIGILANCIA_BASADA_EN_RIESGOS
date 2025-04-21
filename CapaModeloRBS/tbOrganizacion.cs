using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapaModeloRBS
{
   public class tbOrganizacion
    {
        public int OrganizacionID { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string GerenteResponsable { get; set; }
        public string NCertificadoOMA { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public List<tbArea> oArea { get; set; } = new List<tbArea>();
        public List<tbRespuestaLV> oRespuestaLV { get; set; } = new List<tbRespuestaLV>();
    }
}
