namespace ByDesigners.App
{
    public abstract class ItemCotizable
    {
        public int IdItem { get; set; }
        public string NombreItem { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioBase { get; set; }
        public string TipoItem { get; protected set; } = string.Empty;
        public int Stock { get; set; }
        public DateTime FechaAlta { get; set; } = DateTime.Now;

        // Método abstracto que resuelve la llamada en DetalleCotizacion
        public abstract decimal CalcularPrecioTotal(int cantidad);
    }
}