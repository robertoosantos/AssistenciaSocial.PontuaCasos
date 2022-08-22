using System.ComponentModel.DataAnnotations.Schema;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class ItensCasos
{
    public int Id { get; set; }
    public int CasoId { get; set; }
    public int ItemId { get; set; }
    public int? ItemPai { get; set; }
    public bool Ativo { get; set; }
}