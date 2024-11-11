
using System.Data;
using Microsoft.EntityFrameworkCore;

public class RegistroDbService : IRegistroService
{

    private readonly RegistroContext _context;

    public RegistroDbService(RegistroContext context)
    {
        _context = context;
    }

    public bool ExisteRegistroEnFecha(string userId, DateTime fecha)
    {
        return _context.Registros.Any(r => r.UserId == userId && r.Fecha.Date == fecha.Date);
    }
    public Registro Create(RegistroDTO r)
    {
        var nuevoRegistro = new Registro
        {
            UserId = r.UserId,
            ActividadId = r.ActividadId,
            Fecha = (DateTime)r.Fecha,
            Duracion = (float)r.Duracion,
            Distancia = (float)r.Distancia

        };

        _context.Add(nuevoRegistro);
        _context.SaveChanges();
        return nuevoRegistro;
    }

    public bool Delete(int id)
    {
        Registro? registro = _context.Registros.Find(id);
        if (registro == null)
        {
            return false;
        }
        _context.Registros.Remove(registro);
        _context.SaveChanges();
        return true;
    }

    public IEnumerable<Registro> GetAll()
    {
        return _context.Registros.Include(r => r.Actividad);
    }

    public Registro? GetById(int id)
    {
        return _context.Registros.Find(id);
    }

    public Registro? Update(int id, RegistroDTO r, string userId)
    {

        var registroExistente = _context.Registros
            .FirstOrDefault(r => r.Id == id && r.UserId == userId);

        if (registroExistente == null)
        {
            return null;
        }

        registroExistente.ActividadId = r.ActividadId;
        registroExistente.Distancia = r.Distancia;
        registroExistente.Duracion = r.Duracion;



        _context.SaveChanges();

        return registroExistente;
    }

    public List<Registro> GetAllByUserId(string userId)
    {
        return _context.Registros
            .Where(r => r.UserId == userId)
            .Include(r => r.Actividad)
            .ToList();
    }


    public Registro? GetByIdAndUserId(int id, string userId)
    {
        return _context.Registros
            .FirstOrDefault(r => r.Id == id && r.UserId == userId);
    }




}