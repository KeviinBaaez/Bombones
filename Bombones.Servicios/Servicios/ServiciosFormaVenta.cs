using Bombones.Datos.Interfaces;
using Bombones.Entidades.Entidades;
using System.Data.SqlClient;

namespace Bombones.Servicios.Servicios
{
    public class ServiciosFormaVenta : IServiciosFormaVenta
    {
        private readonly IRepositorioFormasVentas _repositorio;
        private readonly string? _cadena;

        public ServiciosFormaVenta(IRepositorioFormasVentas repositorio, string? cadena)
        {
            _repositorio = repositorio;
            _cadena = cadena;
        }

        public void Borrar(int formaDeVentaId)
        {
            if (_repositorio is null)
            {
                throw new ApplicationException("Dependencias no cargadas");
            }
            using (var conn = new SqlConnection(_cadena))
            {
                conn.Open();
                using(var tran = conn.BeginTransaction())
                {
                    try
                    {
                        _repositorio.Borrar(formaDeVentaId, conn, tran);
                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool EstaRelacionado(int formaDeVentaId)
        {
            if (_repositorio is null)
            {
                throw new ApplicationException("Dependencias no cargadas");
            }
            using (var conn = new SqlConnection(_cadena))
            {
                conn.Open();
                return _repositorio.EstaRelacionado(formaDeVentaId, conn);
            }
        }

        public bool Existe(FormaVenta forma)
        {
            if (_repositorio is null)
            {
                throw new ApplicationException("Dependencias no cargadas");
            }
            try
            {
                using (var conn = new SqlConnection(_cadena))
                {
                    conn.Open();
                    return _repositorio.Existe(forma, conn);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<FormaVenta>? GetLista()
        {
            if (_repositorio is null)
            {
                throw new ApplicationException("Dependencias no cargadas");
            }
            using (var conn = new SqlConnection(_cadena))
            {
                conn.Open();
                return _repositorio.GetLista(conn);
            }
        }

        public void Guardar(FormaVenta forma)
        {
            if (_repositorio is null)
            {
                throw new ApplicationException("Dependencias no cargadas");
            }
            using (var conn = new SqlConnection(_cadena))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        if (forma.FormaDeVentaId == 0)
                        {
                            _repositorio.Agregar(forma, conn, tran);
                        }
                        else
                        {
                            _repositorio.Editar(forma, conn, tran);
                        }
                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
