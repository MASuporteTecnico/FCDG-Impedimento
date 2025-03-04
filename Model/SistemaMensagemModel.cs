using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MaSistemas.Model
{
  [Table("SistemaMensagens")]
  public class SistemaMensagemModel : BaseModel
  {
    public SistemaMensagemModel (){
      Para = new HashSet<SistemaMensagemParaModel>();
      Caixa = new HashSet<SistemaMensagemCaixaModel>();
    }

    public int UsuarioId { get; set; }
    public string Titulo { get; set; } = "";   
    public string Texto { get; set; } = "";

    [ForeignKey("UsuarioId")]
    public virtual SistemaUsuarioModel De { get; set; }
    public virtual ICollection<SistemaMensagemParaModel> Para { get; set; }
    public virtual ICollection<SistemaMensagemCaixaModel> Caixa { get; set; }
  }
}