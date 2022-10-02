using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;

public class PontuaCasosContext : IdentityDbContext<Usuario>
{
    public DbSet<Organizacao> Organizacoes => Set<Organizacao>();
    public DbSet<Caso> Casos => Set<Caso>();
    public DbSet<Item> Itens => Set<Item>();
    public DbSet<ViolenciaSofrida> ViolenciasSofridas => Set<ViolenciaSofrida>();
    public DbSet<IndividuoEmViolacao> IndividuosEmViolacoes => Set<IndividuoEmViolacao>();

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

        modelBuilder.Entity<Item>()
            .HasOne(i => i.RelacionadoA)
            .WithMany()
            .HasForeignKey(i => i.RelacionadoAoId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Item>()
            .HasOne(i => i.Categoria)
            .WithMany(c => c.Itens)
            .HasForeignKey(i => i.CategoriaId)
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

        modelBuilder.Entity<Caso>()
            .ToTable(
                "Casos",
                c => c.IsTemporal(
                    c => 
                    {
                        c.HasPeriodStart("De");
                        c.HasPeriodEnd("Ate");
                        c.UseHistoryTable("CasosHistorico");
                    }
                )
            );

        modelBuilder.Entity<Caso>()
            .HasOne(c => c.ModificadoPor)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Caso>()
            .HasMany(c => c.Individuos)
            .WithOne(i => i.Caso)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Caso>()
            .HasMany(c => c.ItensFamiliares)
            .WithMany(i => i.Casos)
            .UsingEntity<Dictionary<string, object>>(
                "ItensFamiliares",
                j => j
                    .HasOne<Item>()
                    .WithMany()
                    .HasForeignKey("ItemFamiliarId")
                    .HasConstraintName("FK_ItensFamiliares_Itens_ItemFamiliarId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Caso>()
                    .WithMany()
                    .HasForeignKey("CasoId")
                    .HasConstraintName("FK_ItensFamiliares_Casos_CasoId")
                    .OnDelete(DeleteBehavior.ClientCascade));

        modelBuilder.Entity<IndividuoEmViolacao>()
            .HasOne(i => i.Item)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<IndividuoEmViolacao>()
            .HasMany(i => i.SituacoesDeSaude)
            .WithMany(s => s.IndividuosSaude)
            .UsingEntity<Dictionary<string, object>>(
                "SaudeIndividuos",
                j => j
                    .HasOne<Item>()
                    .WithMany()
                    .HasForeignKey("ItemSaudeId")
                    .HasConstraintName("FK_SaudeIndividuos_Itens_ItemSaudeId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<IndividuoEmViolacao>()
                    .WithMany()
                    .HasForeignKey("IndividuoId")
                    .HasConstraintName("FK_SaudeIndividuos_Individuos_IndividuoId")
                    .OnDelete(DeleteBehavior.Cascade));

        modelBuilder.Entity<ViolenciaSofrida>()
            .HasOne(vs => vs.Violencia)
            .WithMany()
            .HasForeignKey(vs => vs.ViolenciaId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ViolenciaSofrida>()
            .HasOne(vs => vs.IndividuoEmViolacao)
            .WithMany(i => i.ViolenciasSofridas)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ViolenciaSofrida>()
            .HasOne(vs => vs.Situacao)
            .WithMany()
            .HasForeignKey(vs => vs.SituacaoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ViolenciaSofrida>()
            .HasIndex("IndividuoEmViolacaoId", "ViolenciaId")
            .IsUnique();
    }
}