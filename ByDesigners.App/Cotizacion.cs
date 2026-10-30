namespace ByDesigners.App
{
    public class Cotizacion
    {
        public int IdCot { get; set; }
        public DateTime FechaCot { get; set; } = DateTime.Now;
        public string EstadoCot { get; set; } = "Pendiente";
        public int IdCli { get; set; }
        public int IdTrab { get; set; }
        public string ObsCot { get; set; } = string.Empty;

        public List<DetalleCotizacion> Detalles { get; set; } = new List<DetalleCotizacion>();

        public decimal TotalCot => Detalles.Sum(d => d.Subtotal);
    }
}