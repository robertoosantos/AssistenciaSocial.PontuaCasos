namespace AssistenciaSocial.PontuaCasos.WebApp.Models;

public class ListaPaginavel<T>
{
    public List<T> Itens { get; set; } = new List<T>();
    public int TotalItens { get; set; }
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }

    public int TotalPaginas => (int)Math.Ceiling((double)TotalItens / TamanhoPagina);
    public bool TemPaginaAnterior => Pagina > 1;
    public bool TemProximaPagina => Pagina < TotalPaginas;
}