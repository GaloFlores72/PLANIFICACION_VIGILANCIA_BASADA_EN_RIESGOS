using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModeloRBS
{
    public class tbMenu
    {
        public int IdMenu { get; set; }
        public string NombreMenu { get; set; }
        public string Icono { get; set; }
        public bool? Activo { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaRegistro { get; set; }

        // Relación con SubMenu
        public List<tbSubMenu> oSubMenu { get; set; } = new List<tbSubMenu>();
    }
}
