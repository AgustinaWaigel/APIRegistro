public class Registro
{
    public int Id { get; set; }
    
    public int? ActividadId { get; set; }
    public Actividad Actividad { get; set; }
    
    public float Duracion { get; set; }
    public float Distancia { get; set; }
    public DateTime Fecha { get; set; }

     public string? UserId { get; set; }
    

}
