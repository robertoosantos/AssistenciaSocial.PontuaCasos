using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class IndividuoEmViolacao
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    public int ItemId { get; set; }
    public Item? Item { get; set; }
    public int CasoId { get; set; }
    public Caso? Caso { get; set; }
    public List<Item>? SituacoesDeSaude { get; set; }
    public List<ViolenciaSofrida>? ViolenciasSofridas { get; set; }
    [NotMapped]
    public List<Item>? OpcoesViolencias { get; set; }
    [NotMapped]
    public List<Item>? OpcoesSaude { get; set; }
}