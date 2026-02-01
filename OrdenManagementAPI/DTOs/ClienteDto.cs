namespace OrderManagementAPI.DTOs
{
    /// <summary>
    /// DTO para transferencia de datos de Cliente
    /// </summary>
    public class ClienteDto
    {
        /// <summary>
        /// ID único del cliente (0 para crear nuevo)
        /// </summary>
        public long ClienteId { get; set; }

        /// <summary>
        /// Nombre completo del cliente (3-100 caracteres)
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Número de identidad del cliente (formato: 0000-0000-00000)
        /// </summary>
        public string Identidad { get; set; } = string.Empty;
    }
}
