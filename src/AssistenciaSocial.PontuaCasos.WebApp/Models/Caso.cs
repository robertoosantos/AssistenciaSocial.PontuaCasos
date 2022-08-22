using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class Caso : ModelBaseControle
{
    public int Id { get; set; }
    public int Pontos { get; set; }
    public bool Ativo { get; set; }
    public Organizacao Organizacao { get; set; } = null!;
    public List<Item> Itens { get; set; } = null!;
    [NotMapped]
    public List<Item>? Categorias { get; set; }

    internal void CalcularPontos(List<Item> categorias)
    {
        var mapaCategorias = new Dictionary<int, Item>();

        foreach (var item in categorias)
        {
            if (item.ItemId == null)
            {
                Item? existe = null;
                if (!mapaCategorias.TryGetValue(item.Id, out existe))
                {
                    mapaCategorias.Add(item.Id, item);
                }
            }
        }

        int pontos = 0;

        if (mapaCategorias.Count != 0)
        {
            foreach (var item in Itens)
            {
                if (item.ItemId != null)
                    pontos += mapaCategorias[(int)item.ItemId].Pontos * item.Pontos;
            }
        }

        this.Pontos = pontos;
    }
}