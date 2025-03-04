using System.ComponentModel.DataAnnotations.Schema;

namespace MaSistemas.Model
{
  [Table("SistemaMensagensCaixa")]
  public class SistemaMensagemCaixaModel : BaseModel
  {
    public int MensagemId { get; set; }
    public int UsuarioId { get; set; }
    public bool Lida { get; set; } = false;
    
    [ForeignKey("MensagemId")]
    public virtual SistemaMensagemModel Mensagem { get; set; }
    
    [ForeignKey("UsuarioId")]
    public virtual SistemaUsuarioModel Usuario { get; set; }
  }
}