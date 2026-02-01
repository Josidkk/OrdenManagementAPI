using Microsoft.AspNetCore.Mvc;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Services;

namespace OrderManagementAPI.Controllers
{
    [ApiController]
    [Route("api/productos")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _service;

        public ProductoController(IProductoService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ProductoDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<ProductoDto>>>> ObtenerTodosAsync()
        {
            var resultado = await _service.ObtenerTodosAsync();
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProductoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ProductoDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ProductoDto>>> ObtenerPorIdAsync(long id)
        {
            var resultado = await _service.ObtenerPorIdAsync(id);

            if (!resultado.Success)
                return NotFound(resultado);

            return Ok(resultado);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ProductoDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<ProductoDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ProductoDto>>> CrearAsync([FromBody] ProductoDto productoDto)
        {
            // Validar que productoId sea 0 para crear
            if (productoDto.ProductoId != 0)
            {
                var validationError = ApiResponse<ProductoDto>.ErrorResponse(
                    "Error en la validación",
                    new List<string> { "Para crear un nuevo producto, productoId debe ser 0" }
                );
                return BadRequest(validationError);
            }

            var resultado = await _service.CrearAsync(productoDto);

            if (!resultado.Success)
                return BadRequest(resultado);

            // Retornar 201 Created con URI del recurso creado
            return Created($"/api/productos/{resultado.Data.ProductoId}", resultado);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProductoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ProductoDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<ProductoDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ProductoDto>>> ActualizarAsync(long id, [FromBody] ProductoDto productoDto)
        {
            var resultado = await _service.ActualizarAsync(id, productoDto);

            if (!resultado.Success)
            {
                // Si el mensaje es "Producto no encontrado", retornar 404
                if (resultado.Message == "Producto no encontrado")
                    return NotFound(resultado);

                // Otros errores retornan 400
                return BadRequest(resultado);
            }

            return Ok(resultado);
        }
    }
}
