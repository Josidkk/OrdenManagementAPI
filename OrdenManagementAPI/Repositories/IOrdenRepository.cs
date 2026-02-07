using OrderManagementAPI.DTOs;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Repositories
{
    public interface IOrdenRepository
    {
        Task<(OrdenResponseDto? orden, string mensaje, int? errorCode)> CrearOrdenAsync(OrdenRequestDto ordenRequest);
        Task<IEnumerable<Orden>> ObtenerOrdenesEncabezadoAsync();
        Task<IEnumerable<DetalleOrden>> ObtenerOrdenesDetalleAsync(long ordenId);
    }
}
