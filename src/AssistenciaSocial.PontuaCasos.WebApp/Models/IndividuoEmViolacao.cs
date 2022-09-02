using System.Collections;
using System.Collections.Generic;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class IndividuoEmViolacao : Item
{
    public List<Violencia>? Violencias { get; set; }
    public List<Item>? Saude { get; set; }
}