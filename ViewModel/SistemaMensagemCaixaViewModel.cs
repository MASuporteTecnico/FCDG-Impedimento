namespace MaSistemas.ViewModel
{
  public class SistemaMensagemCaixaViewModel : BaseViewModel
  {
    public int MensagemId { get; set; }
    public int UsuarioId { get; set; }
    public bool Lida { get; set; } = false;
    public virtual SistemaMensagemViewModel Mensagem { get; set; }
    public virtual SistemaUsuarioViewModel Usuario { get; set; }
  }
}