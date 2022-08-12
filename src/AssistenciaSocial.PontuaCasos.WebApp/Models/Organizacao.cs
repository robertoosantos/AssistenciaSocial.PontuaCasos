namespace AssistenciaSocial.PontuaCasos.WebApp.Models
{
    public class Organizacao
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public List<Usuario> Membros { get; set; } = null!;
        public List<Usuario> Administradores { get; set; } = null!;
        public DateTime CriadoEm { get; set; }
        public Usuario CriadoPor { get; set; } = null!;
        public DateTime ModificadoEm { get; set; }
        public Usuario ModificadoPor { get; set; } = null!;
    }
}