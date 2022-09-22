using System.ComponentModel;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;

public abstract class ModelBaseControle
{
    [DisplayName("Criado Em")]
    public DateTime CriadoEm { get; set; }
    public string CriadoPorId { get; set; } = null!;
    public Usuario? CriadoPor { get; set; } = null!;
    public string ModificadoPorId { get; set; } = null!;
    [DisplayName("Modificado Em")]
    public DateTime ModificadoEm { get; set; }
    public Usuario? ModificadoPor { get; set; } = null!;
}