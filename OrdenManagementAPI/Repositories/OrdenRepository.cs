using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Models;
using System.Data;
using System.Text;

namespace OrderManagementAPI.Repositories
{
    public class OrdenRepository : IOrdenRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrdenRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(OrdenResponseDto? orden, string mensaje, int? errorCode)> CrearOrdenAsync(OrdenRequestDto ordenRequest)
        {
            try
            {
                // Construir XML de detalles
                var detallesXml = new StringBuilder();
                detallesXml.Append("<Detalles>");
                foreach (var detalle in ordenRequest.Detalles)
                {
                    detallesXml.Append("<Detalle>");
                    detallesXml.Append($"<ProductoId>{detalle.ProductoId}</ProductoId>");
                    detallesXml.Append($"<Cantidad>{detalle.Cantidad}</Cantidad>");
                    detallesXml.Append("</Detalle>");
                }
                detallesXml.Append("</Detalles>");

                long nuevoId = 0;

                // Ejecutar SP
                // Nota: Usamos ejecución manual para capturar errores específicos
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "sp_CreateOrden";
                    command.CommandType = CommandType.StoredProcedure;

                    var paramCliente = command.CreateParameter();
                    paramCliente.ParameterName = "@ClienteId";
                    paramCliente.Value = ordenRequest.ClienteId;
                    command.Parameters.Add(paramCliente);

                    var paramDetalles = command.CreateParameter();
                    paramDetalles.ParameterName = "@Detalles";
                    paramDetalles.Value = detallesXml.ToString();
                    paramDetalles.DbType = DbType.Xml;
                    command.Parameters.Add(paramDetalles);

                    await _context.Database.OpenConnectionAsync();

                    using var reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        nuevoId = Convert.ToInt64(reader["NewOrdenId"]);
                    }
                }

                // Cargar la orden completa para retornarla (incluyendo detalles calcualdos por el SP)
                var ordenCreada = await _context.Ordens
                    .Include(o => o.DetalleOrdens)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(o => o.OrdenId == nuevoId);

                if (ordenCreada == null)
                    return (null, "Error al recuperar la orden creada", -1);

                return (_mapper.Map<OrdenResponseDto>(ordenCreada), "Orden creada exitosamente", null);
            }
            catch (SqlException ex)
            {
                // Capturar códigos de error personalizados del SP
                if (ex.Number == 50001)
                {
                    return (null, "El cliente especificado no existe", 50001);
                }
                if (ex.Number == 50002)
                {
                    // El mensaje del SP es "Existencias insuficientes para uno o mas productos."
                    // Trateremos de dar el mensaje original del SP
                    return (null, ex.Message, 50002); 
                }
                return (null, $"Error de base de datos: {ex.Message}", ex.Number);
            }
            catch (Exception ex)
            {
                return (null, $"Error interno: {ex.Message}", -1);
            }
        }
    }
}
