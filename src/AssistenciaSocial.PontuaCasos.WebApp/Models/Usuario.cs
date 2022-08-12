namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public List<Organizacao> Organizacoes { get; set; } = null!;
    public DateTime CriadoEm { get; set; }
    public Usuario CriadoPor { get; set; } = null!;
    public DateTime ModificadoEm { get; set; }
    public Usuario ModificadoPor { get; set; } = null!;
}