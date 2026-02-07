using OrderManagementAPI.DTOs;
using OrderManagementAPI.Repositories;
using AutoMapper;

namespace OrderManagementAPI.Services
{
    public class OrdenService : IOrdenService
    {
        private readonly IOrdenRepository _repository;
        private readonly IProductoRepository _productoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public OrdenService(IOrdenRepository repository, IProductoRepository productoRepository, IClienteRepository clienteRepository, IMapper mapper)
        {
            _repository = repository;
            _productoRepository = productoRepository;
            _clienteRepository = clienteRepository;
            _mapper = mapper;
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
        public async Task<ApiResponse<IEnumerable<OrdenResponseDto>>> ObtenerOrdenesAsync()
        {
            try
            {
                var ordenes = await _repository.ObtenerOrdenesEncabezadoAsync();
                var ordenesDto = _mapper.Map<IEnumerable<OrdenResponseDto>>(ordenes);
                return ApiResponse<IEnumerable<OrdenResponseDto>>.SuccessResponse(ordenesDto, "Ordenes recuperadas exitosamente");
            }
            catch (Exception ex)
            {
                // Log error
                return ApiResponse<IEnumerable<OrdenResponseDto>>.ErrorResponse("Error al recuperar las ordenes", new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<OrdenResponseDto>> ObtenerOrdenPorIdAsync(long id)
        {
            try
            {
                // Get Header (using SP for all and filtering is inefficient, but strictly following 'Encabezado' SP usage implies it. 
                // However, for single ID, usually we'd use Find. I'll use Encabezado SP + Filter to strictly follow 'Use these SPs')
                // Actually, efficient approach: just use the SPs provided.
                var ordenes = await _repository.ObtenerOrdenesEncabezadoAsync();
                var orden = ordenes.FirstOrDefault(o => o.OrdenId == id);

                if (orden == null)
                {
                    return ApiResponse<OrdenResponseDto>.ErrorResponse("Orden no encontrada", new List<string> { "La orden no existe" });
                }

                var detalles = await _repository.ObtenerOrdenesDetalleAsync(id);
                orden.DetalleOrdens = detalles.ToList();

                var ordenDto = _mapper.Map<OrdenResponseDto>(orden);
                return ApiResponse<OrdenResponseDto>.SuccessResponse(ordenDto, "Orden recuperada exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<OrdenResponseDto>.ErrorResponse("Error al recuperar la orden", new List<string> { ex.Message });
            }
        }
    }
}
