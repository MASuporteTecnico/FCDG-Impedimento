namespace MaSistemas.ViewModel
{
  public class SistemaMensagemParaViewModel : BaseViewModel
  {
    public int MensagemId { get; set; }
    public int UsuarioId { get; set; }
    public virtual SistemaMensagemViewModel Mensagem { get; set; }
    public virtual SistemaUsuarioViewModel Usuario { get; set; }
  }
}