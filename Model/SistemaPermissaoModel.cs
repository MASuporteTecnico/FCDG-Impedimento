using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSistemas.Model;

[Table("SistemaPermissoes")]
public class SistemaPermissaoModel
{
  public SistemaPermissaoModel()
  {
    PermissaoDeGrupoUsuario = false;
    PermissaoDeGrupoMenu = false;
    UsoInterno = false;
    Index = false;
    Edit = false;
    Save = false;
    Ativo = true;
  }

  [Key]
  public int Id { get; set; }
  public int? SistemaGrupoUsuarioId { get; set; } // opção única para o grupo user
  public int? SistemaGrupoMenuId { get; set; } // opção única para o grupo user
  public int? SistemaUsuarioId { get; set; } // ou opção única para user nomeado
  public int? SistemaMenuId { get; set; } // ou opção única para user nomeado
  public bool PermissaoDeGrupoUsuario { get; set; }
  public bool PermissaoDeGrupoMenu { get; set; }
  public bool UsoInterno { get; set; }
  public bool Ativo { get; set; }

  public bool Index { get; set; } // Lista inicial da páginas
  public bool Edit { get; set; } // Apenas Leitura 
  public bool Save { get; set; } // Escrita / Delete


  [ForeignKey("SistemaGrupoUsuarioId")]
  public SistemaGrupoModel? GrupoUsuario { get; set; }

  [ForeignKey("SistemaGrupoMenuId")]
  public SistemaGrupoModel? GrupoMenu { get; set; }

  [ForeignKey("SistemaUsuarioId")]
  public SistemaUsuarioModel? Usuario { get; set; }

  [ForeignKey("SistemaMenuId")]
  public virtual SistemaMenuModel? Menu { get; set; }


}