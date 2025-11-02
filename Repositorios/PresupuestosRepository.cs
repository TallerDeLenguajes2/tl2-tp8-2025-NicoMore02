using Microsoft.Data.Sqlite;
using SQLitePCL;
using tl2_tp8_2025_NicoMore02.Models;

public class PresupuestosRepository
{
    private string connectionString = "Data Source=DB/Tienda_final.db";

    public void CrearPresupuesto(Presupuestos presupuesto)
    {
        var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@NombreDestinatario, @FechaCreacion)";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@NombreDestinatario", presupuesto.NombreDestinatario));
        comando.Parameters.Add(new SqliteParameter("@FechaCreacion", presupuesto.FechaCreacion));

        comando.ExecuteNonQuery();
    }

    public List<Presupuestos> ListarPresupuestos()
    {
        var Presupuestos = new List<Presupuestos>();
        var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sqlPresu = "SELECT * FROM Presupuestos";
        using var comandopresu = new SqliteCommand(sqlPresu, conexion);
        using (var lectorPresu = comandopresu.ExecuteReader())
        {
            while (lectorPresu.Read())
            {
                var presu = new Presupuestos
                {
                    idPresupuesto = Convert.ToInt32(lectorPresu["idPresupuesto"]),
                    NombreDestinatario = lectorPresu["NombreDestinatario"].ToString(),
                    FechaCreacion = Convert.ToDateTime(lectorPresu["FechaCreacion"]),
                    detalle = new List<PresupuestosDetalle>()
                };
                Presupuestos.Add(presu);
            }
        }
        

        foreach (var presupu in Presupuestos)
        {
            string sqlDetalle = @"SELECT pd.*, p.Descripcion, p.Precio
            From PresupuestosDetalle pd
            INNER JOIN Productos p ON pd.idProducto = p.idProducto
            WHERE pd.idPresupuesto = @idPresupuesto";

            using var comandoDetalle = new SqliteCommand(sqlDetalle, conexion);
            comandoDetalle.Parameters.Add(new SqliteParameter("@idPresupuesto", presupu.idPresupuesto));
            using (var lectorDetalle = comandoDetalle.ExecuteReader())
            {
                while (lectorDetalle.Read())
                {
                    var detalle = new PresupuestosDetalle
                    {
                        producto = new Productos
                        {
                            idProducto = Convert.ToInt32(lectorDetalle["idProducto"]),
                            descripcion = lectorDetalle["Descripcion"].ToString(),
                            precio = Convert.ToInt32(lectorDetalle["Precio"])
                        },
                        cantidad = Convert.ToInt32(lectorDetalle["Cantidad"])
                    };
                    presupu.detalle.Add(detalle);
                }
            }
        }

            return Presupuestos;
    }

    public Presupuestos GetPresupuesto(int id)
    {
        var presupuesto = new Presupuestos();
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "SELECT * FROM Presupuestos WHERE idPresupuesto = @id";
        using var PresuComando = new SqliteCommand(sql, conexion);
        PresuComando.Parameters.Add(new SqliteParameter("@id", id));

        using var lectorPresu = PresuComando.ExecuteReader();
        if (lectorPresu.Read())
        {
            presupuesto.idPresupuesto = Convert.ToInt32(lectorPresu["idPresupuesto"]);
            presupuesto.NombreDestinatario = lectorPresu["NombreDestinatario"].ToString();
            presupuesto.FechaCreacion = Convert.ToDateTime(lectorPresu["FechaCreacion"]);
            presupuesto.detalle = new List<PresupuestosDetalle>();
        }
        else
        {
            return null;
        }


        using (var DetalleComando = conexion.CreateCommand())
        {
            DetalleComando.CommandText = @"SELECT pd.*, p.Descripcion, p.Precio
            FROM PresupuestosDetalle pd
            INNER JOIN Productos p ON pd.idProducto = p.idProducto
            WHERE pd.idPresupuesto = @id";

            DetalleComando.Parameters.Add(new SqliteParameter("@id", id));
            using (var lectorDetalle = DetalleComando.ExecuteReader())
            {
                while (lectorDetalle.Read())
                {
                    presupuesto.detalle.Add(new PresupuestosDetalle
                    {
                        producto = new Productos
                        {
                            idProducto = Convert.ToInt32(lectorDetalle["idProducto"]),
                            descripcion = lectorDetalle["Descripcion"].ToString(),
                            precio = Convert.ToInt32(lectorDetalle["Precio"])
                        },
                        cantidad = Convert.ToInt32(lectorDetalle["Cantidad"])
                    });
                }
            }
        }

        return presupuesto;
    }

    public void AgregarProductos(int idPresupuesto, int idProducto, int cantidad)
{
    using var conexion = new SqliteConnection(connectionString);
    conexion.Open();

    // Primero verificar si el producto ya existe
    string sqlVerificar = @"SELECT Cantidad FROM PresupuestosDetalle 
                           WHERE idPresupuesto = @idPresupuesto AND idProducto = @idProducto";
    
    using var comandoVerificar = new SqliteCommand(sqlVerificar, conexion);
    comandoVerificar.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
    comandoVerificar.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
    
    var resultado = comandoVerificar.ExecuteScalar();
    
    if (resultado != null)
    {
        int cantidadActual = Convert.ToInt32(resultado);
        string sqlActualizar = @"UPDATE PresupuestosDetalle 
                               SET Cantidad = @nuevaCantidad 
                               WHERE idPresupuesto = @idPresupuesto AND idProducto = @idProducto";
        
        using var comandoActualizar = new SqliteCommand(sqlActualizar, conexion);
        comandoActualizar.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
        comandoActualizar.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
        comandoActualizar.Parameters.Add(new SqliteParameter("@nuevaCantidad", cantidadActual + cantidad));
        comandoActualizar.ExecuteNonQuery();
    }
    else
    {
        string sqlInsertar = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) 
                              VALUES (@idPresupuesto, @idProducto, @cantidad)";
        
        using var comandoInsertar = new SqliteCommand(sqlInsertar, conexion);
        comandoInsertar.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
        comandoInsertar.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
        comandoInsertar.Parameters.Add(new SqliteParameter("@cantidad", cantidad));
        comandoInsertar.ExecuteNonQuery();
    }
}





    //public void AgregarProductos(int idPresupuesto, int idProducto, int cantidad)
    //{
    //    using (var conexion = new SqliteConnection(connectionString))
    //    {
    //        conexion.Open();
    //        string slq = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, cantidad) VALUES (@idPresupuesto, @idProducto, @cantidad)";

    //        using var comando = new SqliteCommand(slq, conexion);
    //        comando.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
    //        comando.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
    //        comando.Parameters.Add(new SqliteParameter("@cantidad", cantidad));

    //        comando.ExecuteNonQuery();
    //    }
    //}

    public void EliminarPresupuesto(int id)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sqlPresuDetalle = "DELETE FROM PresupuestoDetalle WHERE idPresupuesto = @id";
        using var comando = new SqliteCommand(sqlPresuDetalle, conexion);
        comando.Parameters.Add(new SqliteParameter("@id", id));
        comando.ExecuteNonQuery();

        string sqlPresu = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";
        using var comandopresu = new SqliteCommand(sqlPresu, conexion);
        comandopresu.Parameters.Add(new SqliteParameter("@id", id));
        comandopresu.ExecuteNonQuery();
    }

    public void ActualizarPresupuesto(Presupuestos presupuesto)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = @"UPDATE Presupuestos 
                  SET NombreDestinatario = @NombreDestinatario, 
                      FechaCreacion = @FechaCreacion 
                  WHERE idPresupuesto = @idPresupuesto";

        using var comando = new SqliteCommand(sql, conexion);
    
        comando.Parameters.Add(new SqliteParameter("@NombreDestinatario", presupuesto.NombreDestinatario ?? ""));
        comando.Parameters.Add(new SqliteParameter("@FechaCreacion", presupuesto.FechaCreacion));
        comando.Parameters.Add(new SqliteParameter("@idPresupuesto", presupuesto.idPresupuesto));

        int filasAfectadas = comando.ExecuteNonQuery();
    
        if (filasAfectadas == 0)
        {
            throw new Exception($"No se encontró el presupuesto con ID {presupuesto.idPresupuesto}");
        }
    }
}