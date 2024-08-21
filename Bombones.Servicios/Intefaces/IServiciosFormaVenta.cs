using Bombones.Entidades.Entidades;

namespace Bombones.Servicios.Servicios
{
    public interface IServiciosFormaVenta
    {
        void Borrar(int formaDeVentaId);
        bool EstaRelacionado(int formaDeVentaId);
        bool Existe(FormaVenta forma);
        List<FormaVenta>? GetLista();
        void Guardar(FormaVenta forma);
    }
}