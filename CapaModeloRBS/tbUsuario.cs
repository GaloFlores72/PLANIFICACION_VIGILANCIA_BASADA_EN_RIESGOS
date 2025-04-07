using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
    public class tbUsuario
    {
        public int IdUsuario { get; set; }
        public string CodigoUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public int? IdRol { get; set; }
        public bool? Activo { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaVigencia { get; set; }

        // Relación con Rol
        public tbRol oRol { get; set; }
        public List<tbMenu> oListaMenu { get; set; }
    }
}
