using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class Caso : ModelBaseControle
{
    // TODO: indivíduos devem vir antes da questão familiar
    // TODO: Incluir indivíduos em violação
    // TODO: violências dos indivíduos podem ter frequências diferentes Caso > n Indivíduos > n violências - frequência

    public int Id { get; set; }
    public int Pontos { get; set; }
    public string ResponsavelFamiliar { get; set; } = null!;
    public string Titulo { get; set; } = null!;
    public string Prontuario { get; set; } = null!;
    public bool Ativo { get; set; }
    public Organizacao? Organizacao { get; set; }
    public List<Item>? Itens { get; set; }
    public List<IndividuoEmViolacao>? Individuos { get; set; }
    [NotMapped]
    public List<Item>? Categorias { get; set; }

    internal void CalcularPontos()
    {
        int pontos = 0;

        if (Itens != null)
        {
            foreach (var item in Itens)
            {
                if (item == null || item.Categoria == null)
                    throw new ApplicationException("Necessário carregar as categorias dos itens antes de calcular a pontuação do caso.");
                
                pontos += item.Categoria.Pontos * item.Pontos;
            }
        }

        this.Pontos = pontos;
    }
}