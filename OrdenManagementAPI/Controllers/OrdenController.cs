using Microsoft.AspNetCore.Mvc;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Services;

namespace OrderManagementAPI.Controllers
{
    [ApiController]
    [Route("api/ordenes")]
    public class OrdenController : ControllerBase
    {
        private readonly IOrdenService _service;

        public OrdenController(IOrdenService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<OrdenResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<OrdenResponseDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<OrdenResponseDto>>> CrearOrdenAsync([FromBody] OrdenRequestDto ordenRequest)
        {
            var resultado = await _service.CrearOrdenAsync(ordenRequest);

            if (!resultado.Success)
            {
                return BadRequest(resultado);
            }

            return Ok(resultado); 
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<OrdenResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrdenResponseDto>>>> ObtenerOrdenes()
        {
            var resultado = await _service.ObtenerOrdenesAsync();
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<OrdenResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<OrdenResponseDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<OrdenResponseDto>>> ObtenerOrdenPorId(long id)
        {
            var resultado = await _service.ObtenerOrdenPorIdAsync(id);
            if (!resultado.Success)
            {
                return NotFound(resultado);
            }
            return Ok(resultado);
        }
    }
}
