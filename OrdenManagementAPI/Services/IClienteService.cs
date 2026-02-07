using OrderManagementAPI.DTOs;

namespace OrderManagementAPI.Services
{
    public interface IClienteService
    {
        Task<ApiResponse<List<ClienteDto>>> ObtenerTodosAsync();

        Task<ApiResponse<ClienteDto>> ObtenerPorIdAsync(long clienteId);

        Task<ApiResponse<ClienteDto>> CrearAsync(ClienteDto clienteDto);

        Task<ApiResponse<ClienteDto>> ActualizarAsync(long clienteId, ClienteDto clienteDto);
    }
}
