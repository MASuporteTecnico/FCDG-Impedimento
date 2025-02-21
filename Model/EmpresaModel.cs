using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSistemas.Model
{
  [Table("Empresas")]
  public partial class EmpresaModel : BaseModel
  {
    [Key]
    public override int Id {get; set;}
    public string CpfCnpj { get; set; } = "";
    public string Nome { get; set; } = "";
    public bool Ativo { get; set; } = false;
    public bool AdministradoraGlobal { get; set; } = false;

    public virtual ICollection<SistemaUsuarioModel> Usuarios { get; set; } = new HashSet<SistemaUsuarioModel>();
  }
}
