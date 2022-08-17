using Microsoft.AspNetCore.Identity;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class Perfil
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
}