using AssistenciaSocial.PontuaCasos.WebApp.Models;
using Microsoft.EntityFrameworkCore;

public static class ItemQueryExtensions
{
    public static IQueryable<Item> IncludeSubItensAtivos(this IQueryable<Item> query)
    {
        return query.Include(i => i.Itens.Where(si => si.Ativo));
    }
}