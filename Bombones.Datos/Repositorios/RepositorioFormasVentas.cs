using Bombones.Datos.Interfaces;
using Bombones.Entidades.Entidades;
using Dapper;
using System.Data.SqlClient;

namespace Bombones.Datos.Repositorios
{
    public class RepositorioFormasVentas : IRepositorioFormasVentas
    {
        public RepositorioFormasVentas()
        {
            
        }

        public void Agregar(FormaVenta forma, SqlConnection conn, SqlTransaction tran)
        {
            string insertQuery = @"INSERT INTO FormasDeVentasK (Descripcion) 
                    VALUES(@Descripcion); SELECT CAST(SCOPE_IDENTITY() as int)";

            var primaryKey = conn.QuerySingle<int>(insertQuery, forma, tran);
            if (primaryKey > 0)
            {

                forma.FormaDeVentaId = primaryKey;
                return;
            }
            throw new Exception("No se pudo agregar");
        }

        public void Borrar(int formaDeVentaId, SqlConnection conn, SqlTransaction tran)
        {
            string deleteQuery = @"DELETE FROM FormasDeVentasK 
                    WHERE FormaDeVentaId=@FormaDeVentaId";
            int registrosAfectados = conn
                .Execute(deleteQuery, new { formaDeVentaId }, tran);
            if (registrosAfectados == 0)
            {
                throw new Exception("No se pudo borrar");
            }
        }

        public void Editar(FormaVenta forma, SqlConnection conn, SqlTransaction tran)
        {
            string updateQuery = @"UPDATE FormasDeVentasK SET Descripcion=@Descripcion 
                    WHERE FormaDeVentaId=@FormaDeVentaId";

            int registrosAfectados = conn.Execute(updateQuery, forma, tran);
            if (registrosAfectados == 0)
            {
                throw new Exception("No se pudo editar");
            }
        }

        public bool EstaRelacionado(int formaDeVentaId, SqlConnection conn)
        {
            string selectQuery = @"SELECT COUNT(*) 
                            FROM Bombones 
                                WHERE FormaDeVentaId=@FormaDeVentaId";
            return conn.QuerySingle<int>(selectQuery, new { formaDeVentaId }) > 0;
        }

        public bool Existe(FormaVenta forma, SqlConnection conn)
        {
            string selectQuery = @"SELECT COUNT(*) FROM FormasDeVentasK ";
            string finalQuery = string.Empty;
            string conditional = string.Empty;
            if (forma.FormaDeVentaId == 0)
            {
                conditional = "WHERE Descripcion = @Descripcion";
            }
            else
            {
                conditional = @"WHERE Descripcion = @Descripcion
                                AND FormaDeVentaId<>@FormaDeVentaId";
            }
            finalQuery = string.Concat(selectQuery, conditional);
            return conn.QuerySingle<int>(finalQuery, forma) > 0;
        }

        public List<FormaVenta>? GetLista(SqlConnection conn)
        {
            string selectQuery = @"SELECT FormaDeVentaId, Descripcion FROM FormasDeVentasK 
                        ORDER BY Descripcion";
            return conn.Query<FormaVenta>(selectQuery).ToList();
        }
    }
}
