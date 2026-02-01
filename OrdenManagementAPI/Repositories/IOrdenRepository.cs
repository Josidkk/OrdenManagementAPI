using OrderManagementAPI.DTOs;

namespace OrderManagementAPI.Repositories
{
    public interface IOrdenRepository
    {
        Task<(OrdenResponseDto? orden, string mensaje, int? errorCode)> CrearOrdenAsync(OrdenRequestDto ordenRequest);
    }
}
