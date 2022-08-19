using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class Caso : ModelBaseControle
{
    public int Id { get; set; }
    public int Pontos { get; set; }
    public bool Ativo { get; set; }
    public Organizacao Organizacao { get; set; } = null!;
    public List<Item> Itens { get; set; } = null!;
    [NotMapped]
    public List<Item>? Categorias { get; set; }
}