using Microsoft.Data.Sqlite;
using tl2_tp8_2025_NicoMore02.Models;
public class ProductosRepository
{
    private string connectionString = "Data Source=DB/Tienda_final.db";

    public void CrearProducto(Productos producto)
    {
        var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "INSERT INTO Productos (descripcion, precio) VALUES (@descripcion, @precio)";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@descripcion", producto.descripcion));
        comando.Parameters.Add(new SqliteParameter("@precio", producto.precio));

        comando.ExecuteNonQuery();
    }

    public void ActualizarProducto(int id, Productos producto)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "UPDATE Productos SET descripcion = @descripcion, precio = @precio WHERE idProducto = @id";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@descripcion", producto.descripcion));
        comando.Parameters.Add(new SqliteParameter("@precio", producto.precio));
        comando.Parameters.Add(new SqliteParameter("@id", producto.idProducto));

        comando.ExecuteNonQuery();
    }

    public List<Productos> ListarTodos()
    {
        var productos = new List<Productos>();
        using (var conexion = new SqliteConnection(connectionString))
        {
            conexion.Open();
            var comando = conexion.CreateCommand();
            comando.CommandText = "SELECT * FROM Productos";
            using (var reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    productos.Add(new Productos
                    {
                        idProducto = reader.GetInt32(0),
                        descripcion = reader.GetString(1),
                        precio = reader.GetInt32(2)
                    });
                }
            }
        }
        return productos;
    }


    public Productos BuscarPorId(int id)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "SELECT * FROM Productos WHERE idProducto = @id";

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@id", id));

        using var lector = comando.ExecuteReader();
        if (lector.Read())
        {
            var produ = new Productos
            {
                idProducto = Convert.ToInt32(lector["idProducto"]),
                descripcion = lector["Descripcion"].ToString(),
                precio = Convert.ToInt32(lector["Precio"])
            };

            return produ;
        }
        return null;
    }

    public void EliminarProducto(int id)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sqlEliminarDetalles = "DELETE FROM PresupuestosDetalle WHERE idProducto = @id";
        using var cmdDetalles = new SqliteCommand(sqlEliminarDetalles, conexion);
        cmdDetalles.Parameters.Add(new SqliteParameter("@id", id));
        cmdDetalles.ExecuteNonQuery();


        string sql = "DELETE FROM Productos WHERE idProducto = @id";
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@id", id));
        comando.ExecuteNonQuery();
    }

}