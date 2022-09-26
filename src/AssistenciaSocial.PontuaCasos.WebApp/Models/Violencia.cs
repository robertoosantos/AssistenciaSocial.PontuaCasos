using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class ViolenciaSofrida
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    public int? SituacaoId { get; set; }
    public int ViolenciaId { get; set; }
    public Item? Situacao { get; set; }
    public Item Violencia { get; set; }
    public IndividuoEmViolacao IndividuoEmViolacao { get; set; }
}