using System.Collections;
using System.Collections.Generic;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class ViolenciaSofrida
{
    public string Id { get; set; }
    public Item? Situacao { get; set; }
    public Item Violencia { get; set; }
    public IndividuoEmViolacao IndividuoEmViolacao { get; set; }
}