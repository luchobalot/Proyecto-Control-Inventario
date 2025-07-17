using Microsoft.AspNetCore.Mvc;
using Control.Services.Interfaces;
using Control.Models.Dtos.DTOOficina;
using Control.Services.Implementations;
using Control.Models.Dtos.DTOPersona;

namespace Control.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OficinaController : ControllerBase
    {
        private readonly IOficinaService _oficinaService;

        public OficinaController(IOficinaService oficinaService)
        {
            _oficinaService = oficinaService;
        }

        // ==================== GET: Obtener todas las oficinas ====================

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllOficinas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validar parámetros
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var (oficinas, totalCount) = await _oficinaService.GetPagedAsync(pageNumber, pageSize);

                if (oficinas == null || !oficinas.Any())
                {
                    return NotFound("No se ha encontrado ninguna oficina.");
                }

                var response = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    TotalCount = totalCount,
                    Items = oficinas
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error recuperando datos de la aplicación!");
            }
        }

        // ==================== GET: Obtener oficina por ID ====================
        [HttpGet("{id}", Name = "GetOficina")]
        public async Task<IActionResult> GetOficina(int id)
        {
            var oficina = await _oficinaService.GetByIdAsync(id);
            if (oficina == null)
                return NotFound();
            return Ok(oficina);
        }

        // ==================== POST: Crear nueva oficina ====================
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(OficinaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearOficina([FromBody] CreateOficinaDTO createOficinaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (createOficinaDto == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Verificar si el número de oficina ya existe
                if (!await _oficinaService.IsNumberAvailableAsync(createOficinaDto.Numero))
                {
                    ModelState.AddModelError("", $"Ya existe una oficina con el número: {createOficinaDto.Numero}");
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
                }


                // Crear la oficina
                var oficinaCreada = await _oficinaService.CreateAsync(createOficinaDto);

                // Retornar 201 Created con la oficina creada
                return CreatedAtRoute("GetOficina", new { id = oficinaCreada.IdOficina }, oficinaCreada);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creando la oficina!");
            }
        }

        // ==================== PUT: Actualizar oficina existente ====================
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActualizarOficina(int id, [FromBody] UpdateOficinaDTO updateOficinaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (updateOficinaDto == null)
            {
                return BadRequest(ModelState);
            }

            // Verificar que el ID del parámetro coincida con el del DTO
            if (id != updateOficinaDto.IdOficina)
            {
                ModelState.AddModelError("", "El ID de la URL no coincide con el ID del objeto!");
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            try
            {
                // Verificar si la oficina existe
                var oficinaExistente = await _oficinaService.GetByIdAsync(id);
                if (oficinaExistente == null)
                {
                    return NotFound($"No se encontró la oficina con ID {id}");
                }

                // Verificar si el número de oficina ya existe (excluyendo la oficina actual)
                if (!await _oficinaService.IsNumberAvailableAsync(updateOficinaDto.Numero, id))
                {
                    ModelState.AddModelError("", $"Ya existe otra oficina con el número {updateOficinaDto.Numero}!");
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
                }

                // Actualizar la oficina
                var oficinaActualizada = await _oficinaService.UpdateAsync(updateOficinaDto);

                return Ok(oficinaActualizada);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error actualizando la oficina en la aplicación!");
            }
        }

        // ==================== DELETE: Eliminar oficina ====================
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarOficina(int id)
        {
            try
            {
                // Verificar si la oficina existe
                var oficinaExistente = await _oficinaService.GetByIdAsync(id);
                if (oficinaExistente == null)
                {
                    return NotFound($"No se encontró la oficina con ID {id}");
                }

                // Eliminar la oficina
                var resultado = await _oficinaService.DeleteAsync(id);

                if (resultado)
                {
                    return NoContent(); // 204 No Content - eliminación exitosa
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error eliminando la oficina de la aplicación!");
                }
            }
            catch (InvalidOperationException ex)
            {
                // Si es un error de validación de negocio, retornar Conflict
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error eliminando la oficina de la aplicación!");
            }
        }


    }
}
