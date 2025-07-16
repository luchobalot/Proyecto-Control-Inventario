using Microsoft.AspNetCore.Mvc;
using Control.Services.Interfaces;
using Control.Models.Dtos.DTOPersona;

namespace Control.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaController : ControllerBase
    {
        private readonly IPersonaService _personaService;

        public PersonaController(IPersonaService personaService)
        {
            _personaService = personaService;
        }

        // ==================== GET: Obtener todas las personas ====================

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPersonas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validar parámetros
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var (personas, totalCount) = await _personaService.GetPagedAsync(pageNumber, pageSize);

                if (personas == null || !personas.Any())
                {
                    return NotFound("No se ha encontrado ninguna persona.");
                }

                var response = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    TotalCount = totalCount,
                    Items = personas
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error recuperando datos de la aplicación!");
            }
        }

        // ==================== GET: Obtener persona por ID ====================

        [HttpGet("{id}", Name = "GetPersona")]
        public async Task<IActionResult> GetPersona(int id)
        {
            var persona = await _personaService.GetByIdAsync(id);
            if (persona == null)
                return NotFound();
            return Ok(persona);
        }


        // ==================== POST: Crear nueva persona ====================
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PersonaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearPersona([FromBody] CreatePersonaDTO createPersonaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (createPersonaDto == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Verificar si el nombre de usuario ya existe
                if (!await _personaService.IsUsernameAvailableAsync(createPersonaDto.NombreUsuario))
                {
                    ModelState.AddModelError("", $"Error. El nombre de usuario '{createPersonaDto.NombreUsuario}' ya existe!");
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
                }

                // Verificar si la oficina existe (si se proporciona)
                if (createPersonaDto.OficinaId.HasValue && !await _personaService.OfficeExistsAsync(createPersonaDto.OficinaId.Value))
                {
                    ModelState.AddModelError("", $"Error. La oficina con ID {createPersonaDto.OficinaId} no existe!");
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
                }

                // Crear la persona
                var personaCreada = await _personaService.CreateAsync(createPersonaDto);

                // Retornar 201 Created con la persona creada
                return CreatedAtRoute("GetPersona", new { id = personaCreada.IdPersona }, personaCreada);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creando la persona en la aplicación!");
            }
        }

        // ==================== PUT: Actualizar persona existente ====================
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActualizarPersona(int id, [FromBody] UpdatePersonaDTO updatePersonaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (updatePersonaDto == null)
            {
                return BadRequest(ModelState);
            }

            // Verificar que el ID del parámetro coincida con el del DTO
            if (id != updatePersonaDto.IdPersona)
            {
                ModelState.AddModelError("", "El ID de la URL no coincide con el ID del objeto!");
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            try
            {
                // Verificar si la persona existe
                var personaExistente = await _personaService.GetByIdAsync(id);
                if (personaExistente == null)
                {
                    return NotFound($"No se encontró la persona con ID {id}");
                }

                // Verificar si el nombre de usuario ya existe (excluyendo la persona actual)
                if (!await _personaService.IsUsernameAvailableAsync(updatePersonaDto.NombreUsuario, id))
                {
                    ModelState.AddModelError("", $"Error. El nombre de usuario '{updatePersonaDto.NombreUsuario}' ya existe!");
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
                }

                // Verificar si la oficina existe (si se proporciona)
                if (updatePersonaDto.OficinaId.HasValue && !await _personaService.OfficeExistsAsync(updatePersonaDto.OficinaId.Value))
                {
                    ModelState.AddModelError("", $"Error. La oficina con ID {updatePersonaDto.OficinaId} no existe!");
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
                }

                // Actualizar la persona
                var personaActualizada = await _personaService.UpdateAsync(updatePersonaDto);

                return Ok(personaActualizada);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error actualizando la persona en la aplicación!");
            }
        }

        // ==================== DELETE: Eliminar persona ====================
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarPersona(int id)
        {
            try
            {
                // Verificar si la persona existe
                var personaExistente = await _personaService.GetByIdAsync(id);
                if (personaExistente == null)
                {
                    return NotFound($"No se encontró la persona con ID {id}");
                }

                // Verificar si la persona puede ser eliminada
                if (!await _personaService.CanDeleteAsync(id))
                {
                    return Conflict("No se puede eliminar la persona porque tiene materiales asignados o aparece en el historial de asignaciones.");
                }

                // Eliminar la persona
                var resultado = await _personaService.DeleteAsync(id);

                if (resultado)
                {
                    return NoContent(); // 204 No Content - eliminación exitosa
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error eliminando la persona de la aplicación!");
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
                    "Error eliminando la persona de la aplicación!");
            }
        }


    }
}