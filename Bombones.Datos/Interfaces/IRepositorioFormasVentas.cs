using Bombones.Entidades.Entidades;
using System.Data.SqlClient;

namespace Bombones.Datos.Interfaces
{
    public interface IRepositorioFormasVentas
    {
        void Agregar(FormaVenta forma, SqlConnection conn, SqlTransaction tran);
        void Borrar(int formaDeVentaId, SqlConnection conn, SqlTransaction tran);
        void Editar(FormaVenta forma, SqlConnection conn, SqlTransaction tran);
        bool EstaRelacionado(int formaDeVentaId, SqlConnection conn);
        bool Existe(FormaVenta forma, SqlConnection conn);
        List<FormaVenta>? GetLista(SqlConnection conn);
    }
}