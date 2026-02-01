using Microsoft.AspNetCore.Mvc;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Services;

namespace OrderManagementAPI.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClienteController(IClienteService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ClienteDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<ClienteDto>>>> ObtenerTodosAsync()
        {
            var resultado = await _service.ObtenerTodosAsync();
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ClienteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ClienteDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ClienteDto>>> ObtenerPorIdAsync(long id)
        {
            var resultado = await _service.ObtenerPorIdAsync(id);

            if (!resultado.Success)
                return NotFound(resultado);

            return Ok(resultado);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ClienteDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<ClienteDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ClienteDto>>> CrearAsync([FromBody] ClienteDto clienteDto)
        {
            // Validar que clienteId sea 0 para crear
            if (clienteDto.ClienteId != 0)
            {
                var validationError = ApiResponse<ClienteDto>.ErrorResponse(
                    "Error en la validación",
                    new List<string> { "Para crear un nuevo cliente, clienteId debe ser 0" }
                );
                return BadRequest(validationError);
            }

            var resultado = await _service.CrearAsync(clienteDto);

            if (!resultado.Success)
                return BadRequest(resultado);

            // Retornar 201 Created con URI del recurso creado
            return Created($"/api/clientes/{resultado.Data.ClienteId}", resultado);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ClienteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ClienteDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<ClienteDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ClienteDto>>> ActualizarAsync(long id, [FromBody] ClienteDto clienteDto)
        {
            var resultado = await _service.ActualizarAsync(id, clienteDto);

            if (!resultado.Success)
            {
                // Si el mensaje es "Cliente no encontrado", retornar 404
                if (resultado.Message == "Cliente no encontrado")
                    return NotFound(resultado);

                // Otros errores retornan 400
                return BadRequest(resultado);
            }

            return Ok(resultado);
        }
    }
}
