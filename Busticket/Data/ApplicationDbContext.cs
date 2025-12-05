using Busticket.Models;
using Microsoft.EntityFrameworkCore;

namespace Busticket.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ruta> Rutas { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Conductor> Conductores { get; set; }
        public DbSet<Itinerario> Itinerarios { get; set; }
        public DbSet<Boleto> Boletos { get; set; }
        public DbSet<Oferta> Ofertas { get; set; }
        public DbSet<Resena> Reseñas { get; set; }
        public DbSet<Reporte> Reportes { get; set; }
        public DbSet<Asiento> Asientos { get; set; }  
        public DbSet<Venta> Ventas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            // --- Relaciones ---

    
            modelBuilder.Entity<Asiento>()
              .HasOne(a => a.Ruta)
              .WithMany(r => r.Asientos)
              .HasForeignKey(a => a.RutaId)
              .OnDelete(DeleteBehavior.Cascade);

            // Boleto ? Usuario e Itinerario
            modelBuilder.Entity<Boleto>()
                        .HasOne(b => b.Usuario)
                        .WithMany()
                        .HasForeignKey(b => b.UsuarioId);

            modelBuilder.Entity<Boleto>()
                        .HasOne(b => b.Itinerario)
                        .WithMany()
                        .HasForeignKey(b => b.ItinerarioId);

            // Itinerario ? Ruta, Bus, Conductor
            modelBuilder.Entity<Itinerario>()
                        .HasOne(i => i.Ruta)
                        .WithMany()
                        .HasForeignKey(i => i.RutaId);

            modelBuilder.Entity<Itinerario>()
                        .HasOne(i => i.Bus)
                        .WithMany()
                        .HasForeignKey(i => i.BusId);

            modelBuilder.Entity<Itinerario>()
                        .HasOne(i => i.Conductor)
                        .WithMany()
                        .HasForeignKey(i => i.ConductorId);

            // Resena ? Usuario y Ruta
            modelBuilder.Entity<Resena>()
                        .HasOne(r => r.Usuario)
                        .WithMany()
                        .HasForeignKey(r => r.UsuarioId);

            modelBuilder.Entity<Resena>()
                        .HasOne(r => r.Ruta)
                        .WithMany()
                        .HasForeignKey(r => r.RutaId);

            // Reporte ? Usuario
            modelBuilder.Entity<Reporte>()
                        .HasOne(r => r.Usuario)
                        .WithMany()
                        .HasForeignKey(r => r.UsuarioId);
        }
    }
}
