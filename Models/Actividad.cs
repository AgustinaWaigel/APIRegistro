public class Actividad{
   public int Id { get; set; }
  public string? Nombre { get; set; }
    public Actividad()
    {
    }

    public Actividad(int id, string nombre)
    {
    Id = id;
    Nombre = nombre;
    }

   
}