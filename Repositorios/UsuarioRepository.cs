using MVC.Models;
using MVC.Interfaces;
using Microsoft.Data.Sqlite;

public class UsuarioRepository : IUserRepository
{
    private string connectionString = "Data Source=DB/Tienda_final.db";

    public Usuario GetUser(string username, string password)
    {
        Usuario user = null;
        const string sql = @"SELECT Id, Nombre, User, Pass, Rol
                             FROM Usuarios
                             WHERE User = @username AND Pass = @password";
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@username", username);
        comando.Parameters.AddWithValue("@password", password);

        using var reader = comando.ExecuteReader();
        if (reader.Read())
        {
            user = new Usuario
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                User = reader.GetString(2),
                Pass = reader.GetString(3),
                Rol = reader.GetString(4)
            };
        }
        return user;
    }
}