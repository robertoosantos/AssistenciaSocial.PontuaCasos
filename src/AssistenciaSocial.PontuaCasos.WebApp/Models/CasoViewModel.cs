using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class CasoViewModel
{
    public List<Item> Familia { get; set; }
    public List<Item> Atendidos { get; set; }

    public CasoViewModel(List<Item> itens)
    {
        Familia = MontarCategorias(itens);
        Atendidos = MontarAtendidos(itens);
    }

    private List<Item> MontarCategorias(List<Item> itens)
    {

        var categorias = new Dictionary<int, Item>();

        foreach (var item in itens)
        {
            if (item.CategoriaId != null && item.Categoria != null)
            {
                if (item.UnicaPorFamilia)
                {
                    Item? existe;

                    if (categorias.TryGetValue((int)item.CategoriaId, out existe))
                    {
                        if (existe.Itens != null)
                            existe.Itens.Add(item);
                    }
                    else
                    {
                        var categoria = item.Categoria;
                        categoria.Itens = new List<Item>();
                        categoria.Itens.Add(item);
                        categorias.Add((int)item.CategoriaId, categoria);
                    }
                }
            }
        }

        return categorias.Values.ToList();
    }

    private List<Item> MontarAtendidos(List<Item> itens)
    {
        Dictionary<int, Item> atendidos = new Dictionary<int, Item>();

        foreach (var item in itens)
        {
            if (item.UnicaPorAtendido)
            {
                atendidos.Add(item.Id, item);
            }
            else
            {
                
            }
        }

        foreach (var item in atendidos)
        {
            item.Value.Itens = MontarCategorias(item.Value.Itens);
        }

        return atendidos.Values.ToList();
    }
}