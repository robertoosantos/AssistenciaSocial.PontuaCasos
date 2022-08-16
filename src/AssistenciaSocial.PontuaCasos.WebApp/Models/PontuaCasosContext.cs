using Microsoft.EntityFrameworkCore;
using System;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;

public class PontuaCasosContext : DbContext
{
    public DbSet<Organizacao> Organizacoes => Set<Organizacao>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Item> Itens => Set<Item>();

    public PontuaCasosContext(DbContextOptions<PontuaCasosContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>()
            .HasOne(i => i.ModificadoPor)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.CriadoPor)
            .WithMany();

        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.ModificadoPor)
            .WithMany();

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