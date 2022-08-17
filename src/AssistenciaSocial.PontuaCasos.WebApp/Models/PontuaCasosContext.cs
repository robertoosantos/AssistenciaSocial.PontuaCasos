using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;

public class PontuaCasosContext : IdentityDbContext<Usuario>
{
    public DbSet<Organizacao> Organizacoes => Set<Organizacao>();
    public DbSet<Caso> Casos => Set<Caso>();
    public DbSet<Item> Itens => Set<Item>();

    public PontuaCasosContext(DbContextOptions<PontuaCasosContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Item>()
            .HasOne(i => i.ModificadoPor)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Organizacao>()
            .HasOne(o => o.CriadoPor)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Organizacao>()
            .HasOne(o => o.ModificadoPor)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<Organizacao>()
            .HasMany(o => o.Administradores)
            .WithOne();

        modelBuilder.Entity<Usuario>()
            .HasMany<Item>()
            .WithOne(i => i.ModificadoPor)
            .HasForeignKey(i => i.ModificadoPorId);

        modelBuilder.Entity<Usuario>()
        .HasMany<Item>()
        .WithOne(i => i.CriadoPor)
        .HasForeignKey(i => i.CriadoPorId);

        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Organizacoes)
            .WithMany(o => o.Membros)
            .UsingEntity<Dictionary<string, object>>(
                "MembrosOrganizacao",
                mo => mo
                .HasOne<Organizacao>()
                .WithMany()
                .HasForeignKey("OrganizacaoId")
                .HasConstraintName("FK_MembrosOrganizacao_Organizacoes_OrganizacaoId")
                .OnDelete(DeleteBehavior.Cascade),
                mo => mo
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey("UsuarioId")
                .HasConstraintName("FK_MembrosOrganizacao_Usuarios_UsuarioId")
                .OnDelete(DeleteBehavior.Cascade)
            );
    }
}