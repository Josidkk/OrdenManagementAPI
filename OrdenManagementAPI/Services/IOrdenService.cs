using OrderManagementAPI.DTOs;

namespace OrderManagementAPI.Services
{
    public interface IOrdenService
    {
        Task<ApiResponse<OrdenResponseDto>> CrearOrdenAsync(OrdenRequestDto ordenRequest);
    }
}
