using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;

public class ListaCasosViewModel
{
    public int Id { get; set; }
    [ValidateNever]
    public int Pontos { get; set; }
    [DisplayName("Responsável Familiar")]
    public string ResponsavelFamiliar { get; set; } = null!;
    [DisplayName("Caso")]
    public string Titulo { get; set; } = null!;
    [DisplayName("Número do Prontuário")]
    public string? Prontuario { get; set; } = null!;
    public bool Ativo { get; set; }
    public bool EmAtualizacao { get; set; }
    [DisplayName("Criado Em")]
    public DateTime CriadoEm { get; set; }
    public string CriadoPorId { get; set; } = null!;
    [DisplayName("Criado Por")]
    public string CriadoPor { get; set; } = "Usuário Desconhecido";
    [DisplayName("Modificado Em")]
    public DateTime ModificadoEm { get; set; }
    [DisplayName("Modificado Por")]
    public string ModificadoPor { get; set; } = "Usuário Desconhecido";
}