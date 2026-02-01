using OrderManagementAPI.DTOs;

namespace OrderManagementAPI.Repositories
{
    /// <summary>
    /// Interfaz para el repositorio de productos
    /// Maneja todas las operaciones de acceso a datos relacionadas con productos
    /// </summary>
    public interface IProductoRepository
    {
        /// <summary>
        /// Obtiene la lista de todos los productos
        /// </summary>
        /// <returns>Lista de ProductoDto</returns>
        Task<List<ProductoDto>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        /// <param name="productoId">ID del producto</param>
        /// <returns>ProductoDto o null si no existe</returns>
        Task<ProductoDto?> ObtenerPorIdAsync(long productoId);

        /// <summary>
        /// Inserta un nuevo producto
        /// </summary>
        /// <param name="nombre">Nombre del producto</param>
        /// <param name="descripcion">Descripción del producto</param>
        /// <param name="precio">Precio del producto</param>
        /// <param name="existencia">Existencia en inventario</param>
        /// <returns>Tupla con resultado (ID creado, -1 error, -2 validación) y mensaje</returns>
        Task<(long resultado, string mensaje)> InsertarAsync(string nombre, string descripcion, decimal precio, int existencia);

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        /// <param name="productoId">ID del producto a actualizar</param>
        /// <param name="nombre">Nuevo nombre</param>
        /// <param name="descripcion">Nueva descripción</param>
        /// <param name="precio">Nuevo precio</param>
        /// <param name="existencia">Nueva existencia</param>
        /// <returns>Tupla con resultado (1 éxito, 0 no existe, -1 error, -2 validación) y mensaje</returns>
        Task<(long resultado, string mensaje)> ActualizarAsync(long productoId, string nombre, string descripcion, decimal precio, int existencia);
    }
}
