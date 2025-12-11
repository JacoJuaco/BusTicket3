using Busticket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Busticket.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Ruta> Ruta { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Bus> Bus { get; set; }
        public DbSet<Conductor> Conductor { get; set; }
        public DbSet<Itinerario> Itinerario { get; set; }
        public DbSet<Boleto> Boleto { get; set; }
        public DbSet<Oferta> Oferta { get; set; }
        public DbSet<Resena> Resena { get; set; }
        public DbSet<Reporte> Reporte { get; set; }
        public DbSet<Asiento> Asiento { get; set; }
        public DbSet<Venta> Venta { get; set; }
        public DbSet<Ciudad> Ciudad { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapear tablas (todas en singular, coinciden con SQL Server)
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Ruta>().ToTable("Ruta");
            modelBuilder.Entity<Empresa>().ToTable("Empresa");
            modelBuilder.Entity<Bus>().ToTable("Bus");
            modelBuilder.Entity<Conductor>().ToTable("Conductor");
            modelBuilder.Entity<Itinerario>().ToTable("Itinerario");
            modelBuilder.Entity<Boleto>().ToTable("Boleto");
            modelBuilder.Entity<Oferta>().ToTable("Oferta");
            modelBuilder.Entity<Resena>().ToTable("Resena");
            modelBuilder.Entity<Reporte>().ToTable("Reporte");
            modelBuilder.Entity<Asiento>().ToTable("Asiento");
            modelBuilder.Entity<Venta>().ToTable("Venta");
            modelBuilder.Entity<Ciudad>().ToTable("Ciudad");

            // Precio decimal
            modelBuilder.Entity<Ruta>()
                .Property(r => r.Precio)
                .HasColumnType("decimal(18,2)");

            // Relaciones de Ruta
            modelBuilder.Entity<Ruta>()
                .HasOne(r => r.CiudadOrigen)
                .WithMany()
                .HasForeignKey(r => r.CiudadOrigenId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ruta>()
                .HasOne(r => r.CiudadDestino)
                .WithMany()
                .HasForeignKey(r => r.CiudadDestinoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones de Itinerario
            modelBuilder.Entity<Itinerario>()
                .HasOne(i => i.Ruta)
                .WithMany()
                .HasForeignKey(i => i.RutaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Itinerario>()
                .HasOne(i => i.Bus)
                .WithMany()
                .HasForeignKey(i => i.BusId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Itinerario>()
                .HasOne(i => i.Conductor)
                .WithMany()
                .HasForeignKey(i => i.ConductorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relaciones Asiento
            modelBuilder.Entity<Asiento>()
                .HasOne(a => a.Ruta)
                .WithMany(r => r.Asiento)
                .HasForeignKey(a => a.RutaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relaciones Boleto
            modelBuilder.Entity<Boleto>()
                .HasOne(b => b.Usuario)
                .WithMany()
                .HasForeignKey(b => b.UsuarioId);

            modelBuilder.Entity<Boleto>()
                .HasOne(b => b.Itinerario)
                .WithMany()
                .HasForeignKey(b => b.ItinerarioId);

            // Relaciones Resena
            modelBuilder.Entity<Resena>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.UsuarioId);

            modelBuilder.Entity<Resena>()
                .HasOne(r => r.Ruta)
                .WithMany()
                .HasForeignKey(r => r.RutaId);

            // Relaciones Venta
            modelBuilder.Entity<Venta>()
                .HasOne<Ruta>()
                .WithMany()
                .HasForeignKey(v => v.RutaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Venta>()
                .HasOne<Asiento>()
                .WithMany()
                .HasForeignKey(v => v.AsientoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed de Ciudades
            modelBuilder.Entity<Ciudad>().HasData(
                new Ciudad { CiudadId = 1, Nombre = "Bogotá", Lat = 4.60971, Lng = -74.08175 },
                new Ciudad { CiudadId = 2, Nombre = "Medellín", Lat = 6.2442, Lng = -75.58121 },
                new Ciudad { CiudadId = 3, Nombre = "Cali", Lat = 3.43722, Lng = -76.5225 },
                new Ciudad { CiudadId = 4, Nombre = "Cartagena", Lat = 10.4, Lng = -75.5 },
                new Ciudad { CiudadId = 5, Nombre = "Barranquilla", Lat = 10.96389, Lng = -74.79639 },
                new Ciudad { CiudadId = 6, Nombre = "Bucaramanga", Lat = 7.12539, Lng = -73.1198 }
            );
        }
    }
}
