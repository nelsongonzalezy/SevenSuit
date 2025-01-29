using System.ComponentModel.DataAnnotations;

namespace DbService.Entity
{
    public class Base
    {
        [Key]
        public Guid Id { get; set; }
        public bool Eliminado { get; set; }
        public string ModificadoPor { get; set; } = string.Empty;
        public DateTime FechaComidificado { get; set; }
        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaCreado { get; set; }
    }
}
