using OrderManagementAPI.DTOs;
using OrderManagementAPI.Repositories;

namespace OrderManagementAPI.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repository;

        public ProductoService(IProductoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<List<ProductoDto>>> ObtenerTodosAsync()
        {
            try
            {
                var productos = await _repository.ObtenerTodosAsync();
                return ApiResponse<List<ProductoDto>>.SuccessResponse(productos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ProductoDto>>.ErrorResponse(
                    "Error al obtener los productos",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<ProductoDto>> ObtenerPorIdAsync(long productoId)
        {
            try
            {
                var producto = await _repository.ObtenerPorIdAsync(productoId);

                if (producto == null)
                {
                    return ApiResponse<ProductoDto>.ErrorResponse(
                        "Producto no encontrado",
                        new List<string> { "No existe un producto con el ID especificado" }
                    );
                }

                return ApiResponse<ProductoDto>.SuccessResponse(producto);
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductoDto>.ErrorResponse(
                    "Error al obtener el producto",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<ProductoDto>> CrearAsync(ProductoDto productoDto)
        {
            try
            {
                var (resultado, mensaje) = await _repository.InsertarAsync(
                    productoDto.Nombre,
                    productoDto.Descripcion,
                    productoDto.Precio,
                    productoDto.Existencia
                );

                if (resultado == -2)
                {
                    return ApiResponse<ProductoDto>.ErrorResponse(
                        "Error al crear el producto",
                        new List<string> { "El precio debe ser mayor a 0 y la existencia no puede ser negativa" }
                    );
                }

                if (resultado <= 0)
                {
                    return ApiResponse<ProductoDto>.ErrorResponse(
                        "Error al crear el producto",
                        new List<string> { mensaje }
                    );
                }

                // Retornar el producto creado con el ID generado
                var productoCreado = new ProductoDto
                {
                    ProductoId = resultado,
                    Nombre = productoDto.Nombre,
                    Descripcion = productoDto.Descripcion,
                    Precio = productoDto.Precio,
                    Existencia = productoDto.Existencia
                };

                return ApiResponse<ProductoDto>.SuccessResponse(
                    productoCreado,
                    "Producto creado exitosamente"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductoDto>.ErrorResponse(
                    "Error al crear el producto",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<ProductoDto>> ActualizarAsync(long productoId, ProductoDto productoDto)
        {
            try
            {
                var (resultado, mensaje) = await _repository.ActualizarAsync(
                    productoId,
                    productoDto.Nombre,
                    productoDto.Descripcion,
                    productoDto.Precio,
                    productoDto.Existencia
                );

                if (resultado == 0)
                {
                    return ApiResponse<ProductoDto>.ErrorResponse(
                        "Producto no encontrado",
                        new List<string> { "No existe un producto con el ID especificado" }
                    );
                }

                if (resultado == -2)
                {
                    return ApiResponse<ProductoDto>.ErrorResponse(
                        "Error al actualizar el producto",
                        new List<string> { "El precio debe ser mayor a 0 y la existencia no puede ser negativa" }
                    );
                }

                if (resultado != 1)
                {
                    return ApiResponse<ProductoDto>.ErrorResponse(
                        "Error al actualizar el producto",
                        new List<string> { mensaje }
                    );
                }

                // Retornar el producto actualizado
                var productoActualizado = new ProductoDto
                {
                    ProductoId = productoId,
                    Nombre = productoDto.Nombre,
                    Descripcion = productoDto.Descripcion,
                    Precio = productoDto.Precio,
                    Existencia = productoDto.Existencia
                };

                return ApiResponse<ProductoDto>.SuccessResponse(
                    productoActualizado,
                    "Producto actualizado exitosamente"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductoDto>.ErrorResponse(
                    "Error al actualizar el producto",
                    new List<string> { ex.Message }
                );
            }
        }
    }
}
