using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSistemas.Model;

[Table("SistemaGrupos")]
public class SistemaGrupoModel
{
  public SistemaGrupoModel()
  {
    Ativo = true;
    GrupoDeMenu = false;
    Usuarios = new HashSet<SistemaGrupoUsuarioModel>();
    Menus = new HashSet<SistemaGrupoMenuModel>();
  }

  [Key]
  public int Id { get; set; }
  public string Nome { get; set; }
  public bool Ativo { get; set; }
  public bool GrupoDeMenu { get; set; } // É menu ou user
  public bool UsoInterno { get; set; }
  public bool AdminSistema { get; set; }
  public virtual ICollection<SistemaGrupoUsuarioModel> Usuarios { get; set; } // popula com grupo usuario
  public virtual ICollection<SistemaGrupoMenuModel> Menus { get; set; } // popula com grupo menu

}