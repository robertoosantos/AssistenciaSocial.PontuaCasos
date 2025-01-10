using AssistenciaSocial.PontuaCasos.WebApp.Models;

public class ItemService
{
    private readonly PontuaCasosContext _context;
    public ItemService(PontuaCasosContext context)
    {
        _context = context;
    }

    public List<Item> ConsultarAtendidos()
    {
        var itens = _context.Itens
            .IncludeSubItensAtivos()
            .Where(i => i.Ativo && i.ECategoria && i.UnicaPorAtendido)
            .ToList();

        foreach (var item in itens)
        {
            item.Itens?.Insert(0, new Item { Id = 0, Titulo = "", Pontos = int.MaxValue });
        }

        return itens;
    }
    
    public List<Item> ConsultarViolencias()
    {
        var filtro = new List<string> { Item.ITENS_VIOLENCIAS, Item.ITENS_SITUACAO_VIOLENCIAS };

        var itens = _context.Itens
            .IncludeSubItensAtivos()
            .Where(i => filtro.Contains(i.Titulo))
            .OrderByDescending(i => i.Pontos)
            .ToList();

        foreach (var item in itens)
        {
            item.Itens?.Insert(0, new Item { Id = 0, Titulo = "", Pontos = int.MaxValue });
        }

        return itens;
    }
}
