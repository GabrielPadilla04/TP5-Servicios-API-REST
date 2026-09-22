using Microsoft.EntityFrameworkCore;
using TP5_Servicios_API_REST.Models;

namespace TP5_Servicios_API_REST.Data
{
    // Asegurate de agregar ": DbContext" acá
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<IngresoDetalle> IngresoDetalles { get; set; }
        public DbSet<Salida> Salidas { get; set; }
        public DbSet<SalidaDetalle> SalidaDetalles { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Ingreso>()
                .HasOne(i => i.Usuario)
                .WithMany(u => u.Ingresos)
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Salida>()
                .HasOne(s => s.Usuario)
                .WithMany(u => u.Salidas)
                .HasForeignKey(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}