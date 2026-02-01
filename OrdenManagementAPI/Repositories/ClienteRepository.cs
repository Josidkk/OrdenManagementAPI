using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Models;
using System.Data;

namespace OrderManagementAPI.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ClienteRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ClienteDto>> ObtenerTodosAsync()
        {
            var clientes = await _context.Clientes
                .FromSqlRaw("EXEC sp_ObtenerClientes")
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<ClienteDto>>(clientes);
        }

        public async Task<ClienteDto?> ObtenerPorIdAsync(long clienteId)
        {
            var clientes = await _context.Clientes
                .FromSqlRaw("EXEC sp_ObtenerClientesPorId @ClienteId = {0}", clienteId)
                .AsNoTracking()
                .ToListAsync();

            var cliente = clientes.FirstOrDefault();
            return cliente != null ? _mapper.Map<ClienteDto>(cliente) : null;
        }

        public async Task<(long resultado, string mensaje)> InsertarAsync(string nombre, string identidad)
        {
            try
            {
                
                using var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "sp_InsertarCliente";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var paramNombre = command.CreateParameter();
                paramNombre.ParameterName = "@Nombre";
                paramNombre.Value = nombre;
                command.Parameters.Add(paramNombre);

                var paramIdentidad = command.CreateParameter();
                paramIdentidad.ParameterName = "@Identidad";
                paramIdentidad.Value = identidad;
                command.Parameters.Add(paramIdentidad);

                await _context.Database.OpenConnectionAsync();
                
                using var reader = await command.ExecuteReaderAsync();
                long valor = -1;
                
                if (await reader.ReadAsync())
                {
                    // Ejecutar SP usando DbCommand para manejo robusto de tipos
                    valor = Convert.ToInt64(reader.GetValue(0));
                }

                return valor switch
                {
                    -2 => (valor, "Ya existe un cliente con la identidad proporcionada"),
                    -1 => (valor, "Error al insertar el cliente en la base de datos"),
                    _ => (valor, string.Empty)
                };
            }
            catch (Exception ex)
            {
                return (-1, $"Error en InsertarAsync: {ex.Message}");
            }
        }

        public async Task<(long resultado, string mensaje)> ActualizarAsync(long clienteId, string nombre, string identidad)
        {
            try
            {
                // Ejecutar SP usando DbCommand
                using var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "sp_ActualizarCliente";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var paramClienteId = command.CreateParameter();
                paramClienteId.ParameterName = "@ClienteId";
                paramClienteId.Value = clienteId;
                command.Parameters.Add(paramClienteId);

                var paramNombre = command.CreateParameter();
                paramNombre.ParameterName = "@Nombre";
                paramNombre.Value = nombre;
                command.Parameters.Add(paramNombre);

                var paramIdentidad = command.CreateParameter();
                paramIdentidad.ParameterName = "@Identidad";
                paramIdentidad.Value = identidad;
                command.Parameters.Add(paramIdentidad);

                await _context.Database.OpenConnectionAsync();
                
                using var reader = await command.ExecuteReaderAsync();
                long valor = -1;
                
                if (await reader.ReadAsync())
                {
                    // Leer el valor sin importar si es int o decimal
                    valor = Convert.ToInt64(reader.GetValue(0));
                }

                return valor switch
                {
                    1 => (valor, string.Empty),
                    0 => (valor, "El cliente no existe"),
                    -2 => (valor, "Ya existe un cliente con la identidad proporcionada"),
                    -1 => (valor, "Error al actualizar el cliente"),
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
