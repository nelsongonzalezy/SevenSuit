namespace DbService.Entity
{
    public class SeveClie : Base
    {
        public string Cedula { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Genero { get; set; }
        public DateTime Fecha_nac { get; set; } 
        public Guid Id_EstadoCivil { get; set; }
        public SeveEstadoCivil EstadoCivil { get; set; }

    }
}
