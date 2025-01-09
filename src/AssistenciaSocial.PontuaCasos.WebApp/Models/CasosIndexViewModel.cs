namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class CasosIndexViewModel
{
    public List<Caso> Casos { get; set; } = null!;
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}