namespace ByDesigners.App
{
    public class Proveedor
    {
        public int IdProv { get; set; }
        public string NomProv { get; set; } = string.Empty;
        public string TelProv { get; set; } = string.Empty;
        public string DirProv { get; set; } = string.Empty;
        public string EmailProv { get; set; } = string.Empty;
        public string EstatusProv { get; set; } = "Activo";
    }
}