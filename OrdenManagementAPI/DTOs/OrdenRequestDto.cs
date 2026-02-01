namespace OrderManagementAPI.DTOs
{
    public class DetalleRequestDto
    {
        public long ProductoId { get; set; }
        public int Cantidad { get; set; }
    }

    public class OrdenRequestDto
    {
        public long ClienteId { get; set; }
        public List<DetalleRequestDto> Detalles { get; set; } = new();
    }
}
