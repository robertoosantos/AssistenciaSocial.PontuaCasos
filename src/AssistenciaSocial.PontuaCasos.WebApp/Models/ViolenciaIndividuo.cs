using System.Collections;
using System.Collections.Generic;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class ViolenciaIndividuo
{
    public Item Violencia { get; set; }
    public IndividuoEmViolacao Individuo { get; set; }
    public int? ViolenciaId { get; set; }
    public int? IndividuoId { get; set; }
}