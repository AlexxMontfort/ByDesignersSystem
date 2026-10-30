namespace ByDesigners.App
{
    public class DetalleCotizacion
    {
        public int IdDetalle { get; set; }
        public int IdCot { get; set; }
        public ItemCotizable Item { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal => Item != null ? Item.CalcularPrecioTotal(Cantidad) : PrecioUnitario * Cantidad;
    }
}