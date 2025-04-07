using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CapaModeloRBS
{
   public class tbOrganizacion
    {
        [Key]
        public int OrganizacionID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(500)]
        public string Direccion { get; set; }

        [Required]
        [MaxLength(255)]
        public string GerenteResponsable { get; set; }
               

        [Required]
        [MaxLength(255)]
        public string Correo { get; set; }

        [MaxLength(50)]
        public string Telefono { get; set; }

        List<tbArea> oArea { get; set; } = new List<tbArea>();
    }
}
