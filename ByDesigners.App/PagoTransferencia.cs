namespace ByDesigners.App
{
    public class PagoTransferencia : Pago
    {
        public string BancoOrigen { get; set; } = string.Empty;
        public string ClaveRastreoSpei { get; set; } = string.Empty;

        public PagoTransferencia()
        {
            TipoPago = "Transferencia SPEI";
        }

        public override string GenerarComprobante()
        {
            return $"[TRANSFERENCIA SPEI] Monto: ${MontoPago:F2} | Banco: {BancoOrigen} | Clave SPEI: {ClaveRastreoSpei}";
        }
    }
}