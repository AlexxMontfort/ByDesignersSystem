namespace ByDesigners.App
{
    public class Trabajador
    {
        public int IdTrab { get; set; }
        public string NomTrab { get; set; } = string.Empty;
        public string EspTrab { get; set; } = string.Empty;
        public string TelTrab { get; set; } = string.Empty;
        public string EmailTrab { get; set; } = string.Empty;
        public DateTime FechaContr { get; set; } = DateTime.Now;
        public decimal SueldoBase { get; set; }
    }
}