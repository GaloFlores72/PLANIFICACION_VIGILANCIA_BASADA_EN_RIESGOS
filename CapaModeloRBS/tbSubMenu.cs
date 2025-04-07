using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
   public class tbSubMenu
    {
        public int IdSubMenu { get; set; }
        public int? IdMenu { get; set; }
        public string NombreSubMenu { get; set; }
        public string Controlador { get; set; }
        public string Vista { get; set; }
        public string Icono { get; set; }
        public bool? Activo { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaRegistro { get; set; }

        // Relación con Menu
        public tbMenu oMenu { get; set; }

        // Relación con Permisos
        public List<tbPermisos> oPermisos { get; set; } = new List<tbPermisos>();
    }
}
