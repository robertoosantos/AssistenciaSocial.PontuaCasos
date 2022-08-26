using System.Collections;
using System.Collections.Generic;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class Item : ModelBaseControle, IComparer<Item>
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;
    public int Pontos { get; set; }
    public bool Ativo { get; set; }
    public bool UnicaPorFamilia { get; set; }
    public bool UnicaPorAtendido { get; set; }
    public bool ECategoria { get; set; }
    public int OrganizacaoId { get; set; }
    public Organizacao? Organizacao { get; set; }
    public int? CategoriaId { get; set; }
    public List<Item>? Itens { get; set; }
    public List<Caso>? Casos { get; internal set; }
    public Item? Categoria { get; set; }
    public int? RelacionadoAoId { get; set; }
    public Item? RelacionadoA { get; set; }

    public int Compare(Item? x, Item? y)
    {
        if (x == null)
        {
            if (y == null)
                return 0;
            return -1;
        }

        if (y == null)
            return 1;

        if (x.ECategoria){
            if (!y.ECategoria)
                return 1;

            return CompararPontos(x, y);
            }

        if (x.RelacionadoAoId == null)
        {
            if (y.RelacionadoAoId == null)
                return CompararPontos(x, y);

            return -1;
        }

        if (y.RelacionadoAoId != null)
            return 1;

        if (x.Id < y.Id)
            return -1;
        
        if (x.Id > y.Id)
             return 1;

        return 0;
    }

    private static int CompararPontos(Item x, Item y)
    {
        if (x.Pontos > y.Pontos)
            return 1;
        if (x.Pontos < y.Pontos)
            return -1;

        return 0;
    }
}