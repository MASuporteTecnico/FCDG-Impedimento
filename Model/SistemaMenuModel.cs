using System.ComponentModel.DataAnnotations.Schema;

namespace MaSistemas.Model
{
  [Table("SistemaMenus")]
  public class SistemaMenuModel : BaseModel
  {
    public SistemaMenuModel (){
      SubMenu = new HashSet<SistemaMenuModel>();
    }

    public string Nome { get; set; } = "";   
    public string Rota { get; set; } = "";
    public string Icone { get; set; } = "";
    public bool Divisor { get; set; } = false;
    public int? MenuPaiId { get; set; }
    public int Ordem { get; set; }
    public bool Ativo { get; set; }
    
    [ForeignKey("MenuPaiId")]
    public virtual SistemaMenuModel? MenuPai { get; set; }
    public virtual ICollection<SistemaMenuModel> SubMenu { get; set; }
  }
}