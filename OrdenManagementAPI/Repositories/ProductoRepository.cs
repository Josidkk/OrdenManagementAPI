using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductoRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProductoDto>> ObtenerTodosAsync()
        {
            var productos = await _context.Productos
                .FromSqlRaw("EXEC sp_ObtenerProductos")
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<ProductoDto>>(productos);
        }

        public async Task<ProductoDto?> ObtenerPorIdAsync(long productoId)
        {
            var productos = await _context.Productos
                .FromSqlRaw("EXEC sp_ObtenerProductosPorID @ProductoId = {0}", productoId)
                .AsNoTracking()
                .ToListAsync();

            var producto = productos.FirstOrDefault();
            return producto != null ? _mapper.Map<ProductoDto>(producto) : null;
        }

        public async Task<(long resultado, string mensaje)> InsertarAsync(string nombre, string descripcion, decimal precio, int existencia)
        {
            try
            {
                using var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "sp_InsertarProducto";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var paramNombre = command.CreateParameter();
                paramNombre.ParameterName = "@Nombre";
                paramNombre.Value = nombre;
                command.Parameters.Add(paramNombre);

                var paramDescripcion = command.CreateParameter();
                paramDescripcion.ParameterName = "@Descripcion";
                paramDescripcion.Value = descripcion;
                command.Parameters.Add(paramDescripcion);

                var paramPrecio = command.CreateParameter();
                paramPrecio.ParameterName = "@Precio";
                paramPrecio.Value = precio;
                command.Parameters.Add(paramPrecio);

                var paramExistencia = command.CreateParameter();
                paramExistencia.ParameterName = "@Existencia";
                paramExistencia.Value = existencia;
                command.Parameters.Add(paramExistencia);

                await _context.Database.OpenConnectionAsync();
                
                using var reader = await command.ExecuteReaderAsync();
                long valor = -1;
                
                if (await reader.ReadAsync())
                {
                    valor = Convert.ToInt64(reader.GetValue(0));
                }

                return valor switch
                {
                    -2 => (valor, "El precio debe ser mayor a 0 y la existencia no puede ser negativa"),
                    -1 => (valor, "Error al insertar el producto en la base de datos"),
                    _ => (valor, string.Empty)
                };
            }
            catch (Exception ex)
            {
                return (-1, $"Error en InsertarAsync: {ex.Message}");
            }
        }

        public async Task<(long resultado, string mensaje)> ActualizarAsync(long productoId, string nombre, string descripcion, decimal precio, int existencia)
        {
            try
            {
                using var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "sp_ActualizarProducto";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var paramProductoId = command.CreateParameter();
                paramProductoId.ParameterName = "@ProductoId";
                paramProductoId.Value = productoId;
                command.Parameters.Add(paramProductoId);

                var paramNombre = command.CreateParameter();
                paramNombre.ParameterName = "@Nombre";
                paramNombre.Value = nombre;
                command.Parameters.Add(paramNombre);

                var paramDescripcion = command.CreateParameter();
                paramDescripcion.ParameterName = "@Descripcion";
                paramDescripcion.Value = descripcion;
                command.Parameters.Add(paramDescripcion);

                var paramPrecio = command.CreateParameter();
                paramPrecio.ParameterName = "@Precio";
                paramPrecio.Value = precio;
                command.Parameters.Add(paramPrecio);

                var paramExistencia = command.CreateParameter();
                paramExistencia.ParameterName = "@Existencia";
                paramExistencia.Value = existencia;
                command.Parameters.Add(paramExistencia);

                await _context.Database.OpenConnectionAsync();
                
                using var reader = await command.ExecuteReaderAsync();
                long valor = -1;
                
                if (await reader.ReadAsync())
                {
                    valor = Convert.ToInt64(reader.GetValue(0));
                }

                return valor switch
                {
                    1 => (valor, string.Empty),
                    0 => (valor, "El producto no existe"),
                    -2 => (valor, "El precio debe ser mayor a 0 y la existencia no puede ser negativa"),
                    -1 => (valor, "Error al actualizar el producto"),
                    _ => (valor, "Resultado desconocido del stored procedure")
                };
            }
            catch (Exception ex)
            {
                return (-1, $"Error en ActualizarAsync: {ex.Message}");
            }
        }
    }
}
