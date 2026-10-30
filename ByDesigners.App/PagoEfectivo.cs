namespace ByDesigners.App
{
    public class PagoEfectivo : Pago
    {
        public string ReciboPor { get; set; } = string.Empty;

        public PagoEfectivo()
        {
            TipoPago = "Efectivo";
        }

        public override string GenerarComprobante()
        {
            return $"[PAGO EN EFECTIVO] Monto: ${MontoPago:F2} | Recibido por: {ReciboPor} | Fecha: {FechaPago:dd/MM/yyyy}";
        }
    }
}