using Microsoft.EntityFrameworkCore;

public class RegistroContext : DbContext
{
    public DbSet<Actividad> Actividades { get; set; }
    public DbSet<Registro> Registros { get; set; }

    public RegistroContext(DbContextOptions<RegistroContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actividad>(entity =>
        {
            entity.Property(a => a.Nombre).IsRequired();
        });

        modelBuilder.Entity<Registro>(entity =>
        {
            // No necesitas establecer la longitud de las propiedades de tipo DateTime y float
            entity.Property(r => r.Fecha).IsRequired();
            entity.Property(r => r.Duracion).IsRequired();
            entity.Property(r => r.Distancia).IsRequired();

         // Configuración de la relación uno a muchos.
            entity.HasOne(r => r.Actividad) // Cada Registro tiene una Actividad asociada.
              .WithMany() // No necesitas la propiedad Registros en Actividad.
              .HasForeignKey(r => r.ActividadId).IsRequired();
              

        });
    }
}
