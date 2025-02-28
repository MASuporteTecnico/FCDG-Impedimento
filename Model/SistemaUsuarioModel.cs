using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MaSistemas.Model
{
  [Table("SistemaUsuarios")]
  public partial class SistemaUsuarioModel : BaseModel
  {

    [Key]
    public override int Id {get; set;}
    public int? EmpresaId { get; set; }

    public bool Ativo { get; set; } = true;
    public bool Admin { get; set; } = false;
    public string Senha { get; set; } = "";
    public string Salt { get; set; } = "";
    public string Telefone { get; set; } = "";
    public string EMail { get; set; } = "";
    public bool MenuLateral { get; set; }
    public string Nome { get; set; } = "";
    public string? ChaveResetSenha { get; set; } = "";


    [ForeignKey("EmpresaId")]
    public virtual EmpresaModel? Empresa { get; set; }
    public ICollection<SistemaGrupoUsuarioModel>? Grupos { get; set; } = [];
  }
}
