using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class Caso : ModelBaseControle
{
    public int Id { get; set; }
    [ValidateNever]
    public int Pontos
    {
        get { return CalcularPontos(); }
    }

    [DisplayName("Responsável Familiar")]
    [Required(ErrorMessage = "Responsável familiar é obrigatório")]
    public string ResponsavelFamiliar { get; set; } = null!;
    [DisplayName("Caso")]
    [Required(ErrorMessage = "Caso é obrigatório")]
    public string Titulo { get; set; } = null!;
    [DisplayName("Número do Prontuário")]
    public string? Prontuario { get; set; } = null!;
    public bool Ativo { get; set; }
    public Organizacao? Organizacao { get; set; }
    public List<Item>? ItensFamiliares { get; set; }
    public List<IndividuoEmViolacao>? Individuos { get; set; }
    public bool EmAtualizacao { get; set; }
    [NotMapped]
    public List<Item>? Categorias { get; set; }

    private int CalcularPontos()
    {
        int pontos = 0;

        if (ItensFamiliares != null)
        {
            foreach (var item in ItensFamiliares)
            {
                if (item == null || (!item.ECategoria && item.Categoria == null))
                    throw new ApplicationException("Necessário carregar as categorias dos itens antes de calcular a pontuação do caso.");

                pontos += item.Categoria.Pontos * item.Pontos;
            }
        }

        if (Individuos != null)
        {
            var maiorPontuacaoIndividuo = 0;

            foreach (var individuo in Individuos)
            {
                if (individuo.Item == null || individuo.Item.Categoria == null)
                    throw new ApplicationException("Necessário carregar as categorias das pessoas antes de calcular a pontuação do caso.");


                var pontuacaoIndividuo = individuo.Item.Categoria.Pontos * individuo.Item.Pontos;

                if (individuo.ViolenciasSofridas != null)
                {
                    foreach (var violencia in individuo.ViolenciasSofridas)
                    {
                        var itemViolencia = violencia.Violencia;

                        if (itemViolencia.Categoria == null)
                            throw new ApplicationException("Necessário carregar as categorias das violências antes de calcular a pontuação do caso.");

                        var pontosCategoria = itemViolencia.Categoria.Pontos;
                        int pontosSituacao = 0;
                        int pontosSituacaoAtual = 0;

                        if (violencia.Situacao != null && violencia.Situacao.Categoria != null)
                        {
                            pontosSituacao = violencia.Situacao.Categoria.Pontos;
                            pontosSituacaoAtual = violencia.Situacao.Pontos;
                        }

                        pontuacaoIndividuo += pontosCategoria * itemViolencia.Pontos + pontosSituacao * pontosSituacaoAtual;
                    }
                }

                if (individuo.SituacoesDeSaude != null)
                {
                    foreach (var saude in individuo.SituacoesDeSaude)
                    {
                        if (saude.Categoria == null)
                            throw new ApplicationException("Necessário carregar as categorias das condições da pessoa antes de calcular a pontuação do caso.");

                        pontuacaoIndividuo += saude.Categoria.Pontos * saude.Pontos;
                    }
                }

                if (pontuacaoIndividuo > maiorPontuacaoIndividuo)
                    maiorPontuacaoIndividuo = pontuacaoIndividuo;
            }

            pontos += maiorPontuacaoIndividuo;
        }

        // Para efeito de exibição a pontuação do caso é apenas 1 décimo da 
        // pontuação total dos itens multiplicada por suas categorias.
        return pontos / 10;
    }
}