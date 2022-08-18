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

        modelBuilder.Entity<Caso>()
            .HasOne(c => c.ModificadoPor)
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

        modelBuilder.Entity<Organizacao>()
            .HasMany(o => o.Itens)
            .WithOne(i => i.Organizacao)
            .HasForeignKey(i => i.OrganizacaoId);

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
            .UsingEntity(mo => mo.ToTable("MembrosOrganizacao"));
    }
}