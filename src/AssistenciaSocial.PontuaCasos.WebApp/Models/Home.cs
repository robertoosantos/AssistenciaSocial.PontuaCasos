namespace AssistenciaSocial.PontuaCasos.WebApp.Models
{
    public class Home
    {
        public List<Caso>? CasosEmAtualizacao { get; set; }
        public List<Caso>? CasosSemAtualizacao { get; set; }
    }
}