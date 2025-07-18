using Control.Models;
using Microsoft.EntityFrameworkCore;

namespace Control.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Oficina> Oficinas { get; set; }
        public DbSet<Material> Materiales { get; set; }
        public DbSet<AsignacionHistorial> AsignacionHistorial { get; set; }
        public DbSet<CategoriaMaterial> CategorisasMaterial { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ==================== CONFIGURACIÓN PERSONA ====================

            // Configurar índice único para NombreUsuario
            modelBuilder.Entity<Persona>()
                .HasIndex(p => p.NombreUsuario)
                .IsUnique();

            // Configurar relación Persona-Oficina
            modelBuilder.Entity<Persona>()
                .HasOne(p => p.Oficina)
                .WithMany(o => o.Personas)
                .HasForeignKey(p => p.OficinaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configurar enums para que se guarden como enteros
            modelBuilder.Entity<Persona>()
                .Property(p => p.Rol)
                .HasConversion<int>();

            modelBuilder.Entity<Persona>()
                .Property(p => p.Jerarquia)
                .HasConversion<int>();



            // ==================== CONFIGURACIÓN OFICINA ====================

            // Configurar índice único para Numero de oficina
            modelBuilder.Entity<Oficina>()
                .HasIndex(o => o.Numero)
                .IsUnique();



            // ==================== CONFIGURACIÓN MATERIAL ====================

            modelBuilder.Entity<Material>()
                .Property(m => m.Estado)
                .HasConversion<int>();

            // Relación Material-Categoria
            modelBuilder.Entity<Material>()
                .HasOne(m => m.Categoria)
                .WithMany(c => c.Materiales)
                .HasForeignKey(m => m.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Material-Persona (asignación actual)
            modelBuilder.Entity<Material>()
                .HasOne(m => m.PersonaAsignada)
                .WithMany()
                .HasForeignKey(m => m.PersonaAsignadaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Relación Material-Oficina (ubicación actual)
            modelBuilder.Entity<Material>()
                .HasOne(m => m.Oficina)
                .WithMany()
                .HasForeignKey(m => m.OficinaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Material-AsignacionHistorial
            modelBuilder.Entity<Material>()
                .HasMany(m => m.Historial)
                .WithOne(ah => ah.Material)
                .HasForeignKey(ah => ah.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);




            // ==================== CONFIGURACIÓN ASIGNACION HISTORIAL ====================

            modelBuilder.Entity<AsignacionHistorial>()
                .Property(ah => ah.Estado)
                .HasConversion<int>();

            modelBuilder.Entity<AsignacionHistorial>()
                .HasOne(ah => ah.Material)
                .WithMany(m => m.Historial)
                .HasForeignKey(ah => ah.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AsignacionHistorial>()
                .HasOne(ah => ah.Persona)
                .WithMany()
                .HasForeignKey(ah => ah.PersonaId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<AsignacionHistorial>()
                .HasOne(ah => ah.Oficina)
                .WithMany()
                .HasForeignKey(ah => ah.OficinaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AsignacionHistorial>()
                .HasOne(ah => ah.UsuarioRegistro)
                .WithMany()
                .HasForeignKey(ah => ah.UsuarioRegistroId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AsignacionHistorial>()
                .HasIndex(ah => new { ah.MaterialId, ah.FechaAsignacion })
                .HasDatabaseName("IX_AsignacionHistorial_Material_Fecha");




            // ==================== CONFIGURACIONES ADICIONALES ====================

            modelBuilder.Entity<Material>()
                .Property(m => m.FechaRegistroSistema)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<AsignacionHistorial>()
                .Property(ah => ah.FechaRegistro)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<CategoriaMaterial>()
                .Property(c => c.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            base.OnModelCreating(modelBuilder);
        }
    }
}