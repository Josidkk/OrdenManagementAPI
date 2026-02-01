namespace OrderManagementAPI.DTOs
{
    /// <summary>
    /// DTO para transferencia de datos de Producto
    /// </summary>
    public class ProductoDto
    {
        /// <summary>
        /// ID único del producto (0 para crear nuevo)
        /// </summary>
        public long ProductoId { get; set; }

        /// <summary>
        /// Nombre del producto (requerido, 3-100 caracteres)
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del producto (opcional, máximo 500 caracteres)
        /// </summary>
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Precio del producto (debe ser mayor a 0)
        /// </summary>
        public decimal Precio { get; set; }

        /// <summary>
        /// Existencia en inventario (debe ser mayor o igual a 0)
        /// </summary>
        public int Existencia { get; set; }
    }
}
