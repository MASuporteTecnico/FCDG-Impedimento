using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSistemas.Model;

[Table("SistemaGrupoUsuarios")]
public class SistemaGrupoUsuarioModel
{
  [Key]
  public int Id { get; set; }
  public int SistemaUsuarioId { get; set; } // Id do Usuário
  public int SistemaGrupoUsuarioId { get; set; }  // Id do SistemaGrupo

  [ForeignKey("SistemaUsuarioId")]
  public SistemaUsuarioModel Usuario { get; set; }
  
  [ForeignKey("SistemaGrupoUsuarioId")]
  public SistemaGrupoModel Grupo { get; set; }
  
}