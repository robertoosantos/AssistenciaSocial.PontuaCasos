using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class Item : ModelBaseControle
{
    internal const string ITENS_ATENDIDOS = "Ciclos de Vida";
    internal const string ITENS_SAUDE = "Condições da Pessoa em Violação";
    internal const string ITENS_VIOLENCIAS = "Violências";
    internal const string ITENS_SITUACAO_VIOLENCIAS = "Situações das Violências";

    public int Id { get; set; }
    [DisplayName("Título")]
    public string Titulo { get; set; } = null!;
    public int Pontos { get; set; }
    public bool Ativo { get; set; }
    [DisplayName("Referente à Família")]
    public bool UnicaPorFamilia { get; set; }
    [DisplayName("Identifica os Atendidos")]
    public bool UnicaPorAtendido { get; set; }
    [DisplayName("É uma categoria?")]
    public bool ECategoria { get; set; }
    public int OrganizacaoId { get; set; }
    public Organizacao? Organizacao { get; set; }
    public int? CategoriaId { get; set; }
    public List<Item>? Itens { get; set; }
    public List<Caso>? Casos { get; set; }
    public List<IndividuoEmViolacao>? IndividuosSaude { get; set; }
    public Item? Categoria { get; set; }
    public int? RelacionadoAoId { get; set; }
    public Item? RelacionadoA { get; set; }

    public Item()
    {
        Itens = new List<Item>();
    }

    private PontuaCasosContext _context;

    public Item(PontuaCasosContext context)
    {
        _context = context;
    }

    public List<Item> ConsultarAtendidos()
    {
        var itens = _context.Itens.IncludeSubItensAtivos().Where(i => i.Ativo && i.ECategoria && i.UnicaPorAtendido).ToList();

        foreach (var item in itens)
        {
            if (item.Itens != null)
            {
                item.Itens.Insert(0, new Item { Id = 0, Titulo = "", Pontos = int.MaxValue });
            }
        }

        return itens;
    }

    public List<Item> ConsultarViolencias()
    {
        var filtro = new List<string> { ITENS_VIOLENCIAS, ITENS_SITUACAO_VIOLENCIAS };

        var itens = _context.Itens.IncludeSubItensAtivos().Where(i => filtro.Contains(i.Titulo)).OrderByDescending(i => i.Pontos).ToList();

        foreach (var item in itens)
        {
            if (item.Itens != null)
            {
                item.Itens.Insert(0, new Item { Id = 0, Titulo = "", Pontos = int.MaxValue });
            }
        }

        return itens;
    }

}