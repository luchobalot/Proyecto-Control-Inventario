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

            // Configurar índice único para Codigo
            modelBuilder.Entity<Material>()
                .HasIndex(m => m.Codigo)
                .IsUnique();

            // Configurar enums para que se guarden como enteros
            modelBuilder.Entity<Material>()
                .Property(m => m.Tipo)
                .HasConversion<int>();

            modelBuilder.Entity<Material>()
                .Property(m => m.Estado)
                .HasConversion<int>();

            // Configurar relación Material-Persona (asignación actual)
            modelBuilder.Entity<Material>()
                .HasOne(m => m.PersonaAsignada)
                .WithMany() // Una persona puede tener múltiples materiales asignados
                .HasForeignKey(m => m.PersonaAsignadaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configurar relación Material-Oficina (ubicación actual)
            modelBuilder.Entity<Material>()
                .HasOne(m => m.Oficina)
                .WithMany() // Una oficina puede tener múltiples materiales
                .HasForeignKey(m => m.OficinaId)
                .OnDelete(DeleteBehavior.Restrict); // No permitir eliminar oficina si tiene materiales

            // Configurar relación Material-AsignacionHistorial (historial)
            modelBuilder.Entity<Material>()
                .HasMany(m => m.Historial)
                .WithOne(ah => ah.Material)
                .HasForeignKey(ah => ah.MaterialId)
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina material, eliminar su historial

            // ==================== CONFIGURACIÓN ASIGNACION HISTORIAL ====================

            // Configurar enum para que se guarde como entero
            modelBuilder.Entity<AsignacionHistorial>()
                .Property(ah => ah.Estado)
                .HasConversion<int>();

            // Configurar relación AsignacionHistorial-Material
            modelBuilder.Entity<AsignacionHistorial>()
                .HasOne(ah => ah.Material)
                .WithMany(m => m.Historial)
                .HasForeignKey(ah => ah.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurar relación AsignacionHistorial-Persona (persona asignada)
            modelBuilder.Entity<AsignacionHistorial>()
                .HasOne(ah => ah.Persona)
                .WithMany() // Una persona puede aparecer en múltiples historiales
                .HasForeignKey(ah => ah.PersonaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configurar relación AsignacionHistorial-Oficina
            modelBuilder.Entity<AsignacionHistorial>()
                .HasOne(ah => ah.Oficina)
                .WithMany() // Una oficina puede aparecer en múltiples historiales
                .HasForeignKey(ah => ah.OficinaId)
                .OnDelete(DeleteBehavior.Restrict); // No permitir eliminar oficina si aparece en historial

            // Configurar relación AsignacionHistorial-UsuarioRegistro
            modelBuilder.Entity<AsignacionHistorial>()
                .HasOne(ah => ah.UsuarioRegistro)
                .WithMany() // Un usuario puede registrar múltiples movimientos
                .HasForeignKey(ah => ah.UsuarioRegistroId)
                .OnDelete(DeleteBehavior.Restrict); // No permitir eliminar usuario si ha registrado movimientos

            // Configurar índice compuesto para mejorar consultas de historial
            modelBuilder.Entity<AsignacionHistorial>()
                .HasIndex(ah => new { ah.MaterialId, ah.FechaAsignacion })
                .HasDatabaseName("IX_AsignacionHistorial_Material_Fecha");

            // ==================== CONFIGURACIONES ADICIONALES ====================

            // Configurar precisión decimal para precios
            modelBuilder.Entity<Material>()
                .Property(m => m.Precio)
                .HasColumnType("decimal(18,2)");

            // Configurar valores por defecto para fechas
            modelBuilder.Entity<Material>()
                .Property(m => m.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<AsignacionHistorial>()
                .Property(ah => ah.FechaRegistro)
                .HasDefaultValueSql("GETDATE()");

            base.OnModelCreating(modelBuilder);
        }
    }
}