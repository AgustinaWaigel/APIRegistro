
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/registros")]
[Authorize(Roles = "user,Admin")]

public class RegistroController : ControllerBase
{
    private readonly IRegistroService _registroService;
    private readonly IActividadService _actividadService;

    public RegistroController(IRegistroService registroService, IActividadService actividadService)
    {
        _registroService = registroService;
        _actividadService = actividadService;
    }

    [HttpGet]
    public ActionResult<List<Registro>> GetAll()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var registros = _registroService.GetAllByUserId(userId);
            return Ok(registros);

        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            return Problem(detail: e.Message, statusCode: 500);
        }
    }

    [HttpGet("{id}")]
    public ActionResult<Registro> GetById(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Obtener el userId desde el token
        var registro = _registroService.GetByIdAndUserId(id, userId);

        if (registro is null)
        {
            return NotFound(new { Message = "Registro no encontrado o no pertenece al usuario" });
        }
        return Ok(registro);
    }

    [HttpPost]
    public ActionResult<Registro> NuevoRegistro(RegistroDTO r)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return BadRequest(new { message = "No se encontró el usuario en el token." });
        }

        r.UserId = userId;


        if (r.Distancia <= 0)
        {
            return BadRequest(new { message = "La distancia debe ser mayor a 0 kilómetros." });
        }
        if (r.Duracion <= 0 || r.Duracion > 24)
        {
            return BadRequest(new { message = "La duración debe estar entre 0 y 24 horas." });
        }

        //Verificar si ya existe un registro en la misma fecha para el usuario
        if (_registroService.ExisteRegistroEnFecha(r.UserId, r.Fecha))
        {
            return BadRequest(new { message = "Ya existe un registro para esta fecha." });
        }

        Registro registro = _registroService.Create(r);
        return CreatedAtAction(nameof(GetById), new { id = registro.Id }, registro);
    }

    [HttpPut("{id}")]
    public ActionResult<Registro> Update(int id, RegistroDTO r)
    {
        // Obtener el userId desde los Claims del usuario autenticado
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Buscar el registro por id y asegurarse de que pertenece al usuario
        var registroExistente = _registroService.GetByIdAndUserId(id, userId);

        if (registroExistente == null)
        {
            return NotFound(new { message = "Registro no encontrado o no pertenece al usuario." });
        }


        if (r.Distancia <= 0)
        {
            return BadRequest(new { message = "La distancia debe ser mayor a 0 kilometros." });
        }

        if (r.Duracion <= 0 || r.Duracion > 24)
        {
            return BadRequest(new { message = "La duracion debe estar entre 0 y 24 horas." });
        }

        try
        {

            var registroActualizado = _registroService.Update(id, r, userId);

            if (registroActualizado == null)
            {
                return NotFound(new { message = $"No se pudo actualizar el registro con id: {id}" });
            }

            return CreatedAtAction(nameof(GetById), new { id = registroActualizado.Id }, registroActualizado);
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            return Problem(detail: e.Message, statusCode: 500);
        }
    }





    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        //Asi se obtiene el ID del usuario autenticado
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //Asi se obtiene el registro correspondiente al id y al userId
        var registroExistente = _registroService.GetByIdAndUserId(id, userId);

        if (registroExistente == null)
        {
            return NotFound(new { message = "Registro no encontrado o no pertenece al usuario." });
        }


        bool deleted = _registroService.Delete(id);
        if (deleted)
        {
            return NoContent();
        }

        return NotFound(new { message = "No se pudo eliminar el registro." });
    }



}
