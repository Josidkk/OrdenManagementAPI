using OrderManagementAPI.DTOs;

namespace OrderManagementAPI.Services
{
    /// <summary>
    /// Interfaz para el servicio de productos
    /// Maneja la lógica de negocio relacionada con productos
    /// </summary>
    public interface IProductoService
    {
        /// <summary>
        /// Obtiene todos los productos
        /// </summary>
        /// <returns>ApiResponse con lista de productos</returns>
        Task<ApiResponse<List<ProductoDto>>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        /// <param name="productoId">ID del producto</param>
        /// <returns>ApiResponse con el producto o error si no existe</returns>
        Task<ApiResponse<ProductoDto>> ObtenerPorIdAsync(long productoId);

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        /// <param name="productoDto">Datos del producto a crear</param>
        /// <returns>ApiResponse con el producto creado o errores de validación</returns>
        Task<ApiResponse<ProductoDto>> CrearAsync(ProductoDto productoDto);

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        /// <param name="productoId">ID del producto a actualizar</param>
        /// <param name="productoDto">Nuevos datos del producto</param>
        /// <returns>ApiResponse con el producto actualizado o errores</returns>
        Task<ApiResponse<ProductoDto>> ActualizarAsync(long productoId, ProductoDto productoDto);
    }
}
