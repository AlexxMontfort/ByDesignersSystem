namespace ByDesigners.App
{
    public abstract class Pago
    {
        public int IdPago { get; set; }
        public decimal MontoPago { get; set; }
        public DateTime FechaPago { get; set; } = DateTime.Now;
        public string TipoPago { get; protected set; } = string.Empty;
        public int IdCot { get; set; }
        public string EstatusPago { get; set; } = "Registrado";

        // Método polimórfico para obtener el recibo/comprobante del pago
        public abstract string GenerarComprobante();
    }
}