using System.Collections;
using System.Collections.Generic;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class Item : ModelBaseControle, IComparable<Item>
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

    public int CompareTo(Item? item)
    {
        if (item == null)
            return 1;

        if ((CategoriaId != null && Categoria == null) || (item.CategoriaId != null && item.Categoria == null))
        {
            throw new ApplicationException("A categoria precisa estar carregada.");
        }

        if ((RelacionadoAoId != null && RelacionadoA == null) || (item.RelacionadoAoId != null && item.RelacionadoA == null))
        {
            throw new ApplicationException("O item relacionado precisa estar carregado.");
        }

        if (ECategoria)
        {
            if (!item.ECategoria)
                return 1;

            return Id.CompareTo(item.Id);
        }

        if (Categoria.RelacionadoAoId == null)
        {
            if (item.Categoria.RelacionadoAoId == null)
                return Id.CompareTo(item.Id);

            return 1;
        }

        if (item.Categoria.RelacionadoAoId == null)
            return -1;

        return ((int)Categoria.RelacionadoAoId).CompareTo(item.Categoria.RelacionadoAoId);
    }
}