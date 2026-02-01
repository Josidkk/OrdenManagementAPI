using OrderManagementAPI.DTOs;
using OrderManagementAPI.Repositories;

namespace OrderManagementAPI.Services
{
    public class OrdenService : IOrdenService
    {
        private readonly IOrdenRepository _repository;
        private readonly IProductoRepository _productoRepository;
        private readonly IClienteRepository _clienteRepository;

        public OrdenService(IOrdenRepository repository, IProductoRepository productoRepository, IClienteRepository clienteRepository)
        {
            _repository = repository;
            _productoRepository = productoRepository;
            _clienteRepository = clienteRepository;
        }

        public async Task<ApiResponse<OrdenResponseDto>> CrearOrdenAsync(OrdenRequestDto ordenRequest)
        {
            var cliente = await _clienteRepository.ObtenerPorIdAsync(ordenRequest.ClienteId);
            if (cliente == null)
            {
                 return ApiResponse<OrdenResponseDto>.ErrorResponse(
                     "Error al crear la orden", 
                     new List<string> { "El cliente especificado no existe" }
                 );
            }

            foreach (var detalle in ordenRequest.Detalles)
            {
                var producto = await _productoRepository.ObtenerPorIdAsync(detalle.ProductoId);
                
                if (producto == null)
                {
                    return ApiResponse<OrdenResponseDto>.ErrorResponse(
                        "Error al procesar la orden", 
                        new List<string> { $"El producto con ID {detalle.ProductoId} no existe" }
                    );
                }

                if (producto.Existencia < detalle.Cantidad)
                {
                    return ApiResponse<OrdenResponseDto>.ErrorResponse(
                        "Error al procesar la orden",
                        new List<string> { $"El producto '{producto.Nombre}' no tiene suficientes existencias. Disponible: {producto.Existencia}, Solicitado: {detalle.Cantidad}" }
                    );
                }
            }

            var (orden, mensaje, errorCode) = await _repository.CrearOrdenAsync(ordenRequest);

            if (orden != null)
            {
                return ApiResponse<OrdenResponseDto>.SuccessResponse(orden, "Orden creada exitosamente");
            }

            
            if (errorCode == 50001) 
            {
                return ApiResponse<OrdenResponseDto>.ErrorResponse("Error al crear la orden", new List<string> { "El cliente especificado no existe" });
            }
            
            if (errorCode == 50002) 
            {
              
                return ApiResponse<OrdenResponseDto>.ErrorResponse("Error al procesar la orden", new List<string> { mensaje });
            }

            return ApiResponse<OrdenResponseDto>.ErrorResponse("Error interno", new List<string> { mensaje });
        }
    }
}
