using System.Collections;
using System.Collections.Generic;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class IndividuoEmViolacao
{
    public string Id { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; }
    public int CasoId { get; set; }
    public Caso Caso { get; set; }
    public List<Item>? Violencias { get; set; }
    public List<Item>? SituacoesDeSaude { get; set; }
    public List<ViolenciaSofrida>? ViolenciasSofridas { get; set; }
}