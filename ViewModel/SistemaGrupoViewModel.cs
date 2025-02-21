using System.ComponentModel.DataAnnotations;

namespace MaSistemas.ViewModel;

public class SistemaGrupoViewModel
{
  public SistemaGrupoViewModel()
  {
    Ativo = true;
    GrupoDeMenu = false;
    Usuarios = new HashSet<SistemaGrupoUsuarioViewModel>();
    Menus = new HashSet<SistemaGrupoMenuViewModel>();
  }

  [Key]
  public int Id { get; set; }
  public string Nome { get; set; } = "";
  public bool Ativo { get; set; }
  public bool GrupoDeMenu { get; set; }
  public bool UsoInterno { get; set; }
  public bool AdminSistema { get; set; }
  public  virtual ICollection<SistemaGrupoUsuarioViewModel> Usuarios { get; set; }
  public virtual ICollection<SistemaGrupoMenuViewModel> Menus { get; set; }
}