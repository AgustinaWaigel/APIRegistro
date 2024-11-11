
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/actividades")]
[Authorize(Roles = "Admin")]
public class ActividadController : ControllerBase
{
  private readonly IActividadService _actividadService;

  public ActividadController(IActividadService actividadService)
  {
    _actividadService = actividadService;
  }


  [HttpGet]
  public ActionResult<List<Actividad>> GetAllActividades()
  {

    return Ok(_actividadService.GetAll());
  }

  [HttpGet("{id}")]
  public ActionResult<Actividad> GetById(int id)
  {
    Actividad? a = _actividadService.GetById(id);
    if (a is null) return NotFound();
    return Ok(a);
  }

  [HttpPost]
  public ActionResult<Actividad> NuevaActividad(ActividadDTO a)
  {
    Actividad _a = _actividadService.Create(a);
    return CreatedAtAction(nameof(GetById), new { id = _a.Id }, _a);
  }

  [HttpPut("{id}")]

  public ActionResult UpdateActividad(int id, Actividad updatedActividad)
  {
    // Si el id de la URL no coincide con el id de la actividad, retorna 400 Bad Request  
    if (id != updatedActividad.Id)
    {
      return BadRequest("El id de la URL no coincide con el id de la actividad");
    }

    var actividad = _actividadService.Update(id, updatedActividad);

    if (actividad is null)
    {
      return NotFound(); // Si no se encontró la actividad, retorna 404 Not Found
    }

    // Si la actualización fue exitosa, retorna 204 NoContent o 200 OK
    return NoContent();
  }


  [HttpDelete("{id}")]
  public ActionResult DeleteActividad(int id)
  {
    if (!_actividadService.Delete(id)) return NotFound();
    return NoContent();
  }



}
