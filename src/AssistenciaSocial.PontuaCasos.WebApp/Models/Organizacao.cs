using System.ComponentModel.DataAnnotations;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models
{
    public class Organizacao : ModelBaseControle
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Nome é obrigatório")]
        public string Nome { get; set; } = null!;
        public List<Usuario>? Membros { get; set; }
        public List<Item>? Itens { get; set; }
        public List<Usuario>? Administradores { get; set; }
    }
}