using System.ComponentModel.DataAnnotations.Schema;

namespace MaSistemas.Model
{
  [Table("SistemaMensagensPara")]
  public class SistemaMensagemParaModel : BaseModel
  {

    public int MensagemId { get; set; }
    public int UsuarioId { get; set; }
    
    [ForeignKey("MensagemId")]
    public virtual SistemaMensagemModel Mensagem { get; set; }
    
    [ForeignKey("UsuarioId")]
    public virtual SistemaUsuarioModel Usuario { get; set; }
  }
}