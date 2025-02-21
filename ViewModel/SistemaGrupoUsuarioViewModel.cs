namespace MaSistemas.ViewModel;

public class SistemaGrupoUsuarioViewModel
{
  public int Id { get; set; }
  public int SistemaUsuarioId { get; set; }
  public int SistemaGrupoUsuarioId { get; set; }  

  public SistemaUsuarioViewModel Usuario { get; set; }
  
  public SistemaGrupoViewModel Grupo { get; set; }

  // [ForeignKey("IdSistemaGrupoUsuario")] 
  // public SistemaMenuAcessoViewModel MenuAcesso { get; set; }

}
