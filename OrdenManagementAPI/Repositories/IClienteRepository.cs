using OrderManagementAPI.DTOs;

namespace OrderManagementAPI.Repositories
{
    /// <summary>
    /// Interfaz para el repositorio de clientes
    /// Maneja todas las operaciones de acceso a datos relacionadas con clientes
    /// </summary>
    public interface IClienteRepository
    {
        /// <summary>
        /// Obtiene la lista de todos los clientes
        /// </summary>
        /// <returns>Lista de ClienteDto</returns>
        Task<List<ClienteDto>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene un cliente por su ID
        /// </summary>
        /// <param name="clienteId">ID del cliente</param>
        /// <returns>ClienteDto o null si no existe</returns>
        Task<ClienteDto?> ObtenerPorIdAsync(long clienteId);

        /// <summary>
        /// Inserta un nuevo cliente
        /// </summary>
        /// <param name="nombre">Nombre del cliente</param>
        /// <param name="identidad">Identidad del cliente</param>
        /// <returns>Tupla con resultado (ID creado, -1 error, -2 duplicado) y mensaje</returns>
        Task<(long resultado, string mensaje)> InsertarAsync(string nombre, string identidad);

        /// <summary>
        /// Actualiza un cliente existente
        /// </summary>
        /// <param name="clienteId">ID del cliente a actualizar</param>
        /// <param name="nombre">Nuevo nombre</param>
        /// <param name="identidad">Nueva identidad</param>
        /// <returns>Tupla con resultado (1 éxito, 0 no existe, -1 error, -2 duplicado) y mensaje</returns>
        Task<(long resultado, string mensaje)> ActualizarAsync(long clienteId, string nombre, string identidad);
    }
}
