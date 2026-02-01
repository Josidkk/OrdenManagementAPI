using OrderManagementAPI.DTOs;

namespace OrderManagementAPI.Services
{
    /// <summary>
    /// Interfaz para el servicio de clientes
    /// Maneja la lógica de negocio relacionada con clientes
    /// </summary>
    public interface IClienteService
    {
        /// <summary>
        /// Obtiene todos los clientes
        /// </summary>
        /// <returns>ApiResponse con lista de clientes</returns>
        Task<ApiResponse<List<ClienteDto>>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene un cliente por su ID
        /// </summary>
        /// <param name="clienteId">ID del cliente</param>
        /// <returns>ApiResponse con el cliente o error si no existe</returns>
        Task<ApiResponse<ClienteDto>> ObtenerPorIdAsync(long clienteId);

        /// <summary>
        /// Crea un nuevo cliente
        /// </summary>
        /// <param name="clienteDto">Datos del cliente a crear</param>
        /// <returns>ApiResponse con el cliente creado o errores de validación</returns>
        Task<ApiResponse<ClienteDto>> CrearAsync(ClienteDto clienteDto);

        /// <summary>
        /// Actualiza un cliente existente
        /// </summary>
        /// <param name="clienteId">ID del cliente a actualizar</param>
        /// <param name="clienteDto">Nuevos datos del cliente</param>
        /// <returns>ApiResponse con el cliente actualizado o errores</returns>
        Task<ApiResponse<ClienteDto>> ActualizarAsync(long clienteId, ClienteDto clienteDto);
    }
}
