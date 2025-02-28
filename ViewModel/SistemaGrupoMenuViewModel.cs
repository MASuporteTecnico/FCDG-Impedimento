#nullable enable
namespace MaSistemas.ViewModel;

public class SistemaGrupoMenuViewModel
{
  public int Id { get; set; }
  public int SistemaMenuId { get; set; }
  public int SistemaGrupoMenuId { get; set; }  
  public SistemaMenuViewModel? Menu { get; set; }
  public SistemaGrupoViewModel? Grupo { get; set; }

}
