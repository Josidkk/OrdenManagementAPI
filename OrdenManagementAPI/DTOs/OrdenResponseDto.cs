namespace OrderManagementAPI.DTOs
{
    public class DetalleResponseDto
    {
        public long DetalleOrdenId { get; set; }
        public long OrdenId { get; set; }
        public long ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }

    public class OrdenResponseDto
    {
        public long OrdenId { get; set; }
        public long ClienteId { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<DetalleResponseDto> Detalles { get; set; } = new();
    }
}
