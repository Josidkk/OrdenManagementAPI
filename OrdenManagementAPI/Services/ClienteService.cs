using OrderManagementAPI.DTOs;
using OrderManagementAPI.Repositories;

namespace OrderManagementAPI.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<List<ClienteDto>>> ObtenerTodosAsync()
        {
            try
            {
                var clientes = await _repository.ObtenerTodosAsync();
                return ApiResponse<List<ClienteDto>>.SuccessResponse(clientes);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ClienteDto>>.ErrorResponse(
                    "Error al obtener los clientes",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<ClienteDto>> ObtenerPorIdAsync(long clienteId)
        {
            try
            {
                var cliente = await _repository.ObtenerPorIdAsync(clienteId);

                if (cliente == null)
                {
                    return ApiResponse<ClienteDto>.ErrorResponse(
                        "Cliente no encontrado",
                        new List<string> { "No existe un cliente con el ID especificado" }
                    );
                }

                return ApiResponse<ClienteDto>.SuccessResponse(cliente);
            }
            catch (Exception ex)
            {
                return ApiResponse<ClienteDto>.ErrorResponse(
                    "Error al obtener el cliente",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<ClienteDto>> CrearAsync(ClienteDto clienteDto)
        {
            try
            {
                var (resultado, mensaje) = await _repository.InsertarAsync(
                    clienteDto.Nombre, 
                    clienteDto.Identidad
                );

                if (resultado == -2)
                {
                    return ApiResponse<ClienteDto>.ErrorResponse(
                        "Error al crear el cliente",
                        new List<string> { $"Ya existe un cliente con la identidad {clienteDto.Identidad}" }
                    );
                }

                if (resultado <= 0)
                {
                    return ApiResponse<ClienteDto>.ErrorResponse(
                        "Error al crear el cliente",
                        new List<string> { mensaje }
                    );
                }

                // Retornar el cliente creado con el ID generado
                var clienteCreado = new ClienteDto
                {
                    ClienteId = resultado,
                    Nombre = clienteDto.Nombre,
                    Identidad = clienteDto.Identidad
                };

                return ApiResponse<ClienteDto>.SuccessResponse(
                    clienteCreado,
                    "Cliente creado exitosamente"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<ClienteDto>.ErrorResponse(
                    "Error al crear el cliente",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<ClienteDto>> ActualizarAsync(long clienteId, ClienteDto clienteDto)
        {
            try
            {
                var (resultado, mensaje) = await _repository.ActualizarAsync(
                    clienteId, 
                    clienteDto.Nombre, 
                    clienteDto.Identidad
                );

                if (resultado == 0)
                {
                    return ApiResponse<ClienteDto>.ErrorResponse(
                        "Cliente no encontrado",
                        new List<string> { "No existe un cliente con el ID especificado" }
                    );
                }

                if (resultado == -2)
                {
                    return ApiResponse<ClienteDto>.ErrorResponse(
                        "Error al actualizar el cliente",
                        new List<string> { $"Ya existe un cliente con la identidad {clienteDto.Identidad}" }
                    );
                }

                if (resultado != 1)
                {
                    return ApiResponse<ClienteDto>.ErrorResponse(
                        "Error al actualizar el cliente",
                        new List<string> { mensaje }
                    );
                }

                // Retornar el cliente actualizado
                var clienteActualizado = new ClienteDto
                {
                    ClienteId = clienteId,
                    Nombre = clienteDto.Nombre,
                    Identidad = clienteDto.Identidad
                };

                return ApiResponse<ClienteDto>.SuccessResponse(
                    clienteActualizado,
                    "Cliente actualizado exitosamente"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<ClienteDto>.ErrorResponse(
                    "Error al actualizar el cliente",
                    new List<string> { ex.Message }
                );
            }
        }
    }
}
