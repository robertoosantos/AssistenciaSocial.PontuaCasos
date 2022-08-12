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
}