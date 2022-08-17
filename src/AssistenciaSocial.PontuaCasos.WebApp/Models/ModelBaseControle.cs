namespace AssistenciaSocial.PontuaCasos.WebApp.Models;

public abstract class ModelBaseControle
{
    public DateTime CriadoEm { get; set; }
    public string CriadoPorId { get; set; } = null!;
    public Usuario? CriadoPor { get; set; } = null!;
    public string ModificadoPorId { get; set; } = null!;
    public DateTime ModificadoEm { get; set; }
    public Usuario? ModificadoPor { get; set; } = null!;
}