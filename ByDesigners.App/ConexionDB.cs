using MySql.Data.MySqlClient;

namespace ByDesigners.App
{
    public class ConexionBD
    {
        // Reemplaza "TuContraseña" por la contraseña que configuraste en MySQL
        private static string cadenaConexion = "Server=localhost;Database=bydesigners_db;Uid=root;Pwd=120610;";

        public ConexionBD()
        {
        }

        public static MySqlConnection ObtenerConexion()
        {
            MySqlConnection conexion = new MySqlConnection(cadenaConexion);
            return conexion;
        }
    }
}