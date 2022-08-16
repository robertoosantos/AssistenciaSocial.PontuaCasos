using Microsoft.AspNetCore.Identity;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class Usuario : IdentityUser
{
    public List<Organizacao>? Organizacoes { get; set; }
}