public interface IRegistroService
{
  public IEnumerable<Registro> GetAll();
  public Registro? GetById(int id);
  public Registro Create(RegistroDTO r);
  public bool Delete(int id);
  public Registro Update(int id, RegistroDTO r, string UserId);
  public List<Registro> GetAllByUserId(string userId);
  Registro? GetByIdAndUserId(int id, string userId);
  bool ExisteRegistroEnFecha(string userId, DateTime fecha);
}