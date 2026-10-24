using GerenciarSenhas.Application.Senhas;
using Microsoft.EntityFrameworkCore;

namespace GerenciarSenhas.Infrastructure.Persistencia;

public sealed class SenhasDbContext(DbContextOptions<SenhasDbContext> options) : DbContext(options)
{
    public DbSet<Senha> Senhas => Set<Senha>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Senha>(entity =>
        {
            entity.ToTable("Senhas");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id)
                .HasColumnName("Id")
                .HasColumnType("TEXT")
                .ValueGeneratedNever();
            entity.Property(s => s.Valor)
                .HasColumnName("Senha")
                .HasColumnType("TEXT")
                .IsRequired();
        });
    }
}
