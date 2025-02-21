using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSistemas.Model;

[Table("SistemaGrupoMenus")]
public class SistemaGrupoMenuModel
{
  [Key]
  public int Id { get; set; }
  public int SistemaMenuId { get; set; } // Id do Menu
  public int SistemaGrupoMenuId { get; set; }  // Id do SistemaGrupo

  [ForeignKey("SistemaMenuId")]
  public SistemaMenuModel Menu { get; set; }
  
  [ForeignKey("SistemaGrupoMenuId")]
  public SistemaGrupoModel Grupo{ get; set; }
  
}