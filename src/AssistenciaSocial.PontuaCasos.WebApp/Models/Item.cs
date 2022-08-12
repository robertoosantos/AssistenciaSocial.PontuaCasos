using System.Collections.Generic;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class Item
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;
    public int Pontos { get; set; }
    public bool Ativo { get; set; }
    public bool Multiplo { get; set; }
    public Organizacao Organizacao { get; set; } = null!;
    public List<Item> Itens { get; set; } = null!;
    public DateTime CriadoEm { get; set; }
    public Usuario CriadoPor { get; set; } = null!;
    public DateTime ModificadoEm { get; set; }
    public Usuario ModificadoPor { get; set; } = null!;
}