using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
   public class tbRol
    {
        public int IdRol { get; set; }
        public string DescripcionRol { get; set; }
        public bool? Activo { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaRegistro { get; set; }

        // Relación con Usuario
        public List<tbUsuario> oUsuarios { get; set; } = new List<tbUsuario>();

        // Relación con Permisos
        public List<tbPermisos> oPermisos { get; set; } = new List<tbPermisos>();
    }
}
