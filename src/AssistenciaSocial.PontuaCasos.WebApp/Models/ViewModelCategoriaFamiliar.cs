namespace AssistenciaSocial.PontuaCasos.WebApp.Models;
public class ViewModelCategoriaFamiliar
{
    public Item Categoria { get; set; } = null!;
    public List<ViewModelItemFamiliar> Itens { get; set; } = null!;
}