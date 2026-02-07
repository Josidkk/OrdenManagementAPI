using OrderManagementAPI.DTOs;

namespace OrderManagementAPI.Services
{
    public interface IOrdenService
    {
        Task<ApiResponse<OrdenResponseDto>> CrearOrdenAsync(OrdenRequestDto ordenRequest);
        Task<ApiResponse<IEnumerable<OrdenResponseDto>>> ObtenerOrdenesAsync();
        Task<ApiResponse<OrdenResponseDto>> ObtenerOrdenPorIdAsync(long id);
    }
}
