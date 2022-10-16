using Microsoft.AspNetCore.Identity;

namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class Perfil
{
    public const string PERFIL_USUARIOS = "Usuários";
    public const string PERFIL_ADMINISTRADORES = "Administradores";
    public const string PERFIL_GESTORES = "Gestores";
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
}