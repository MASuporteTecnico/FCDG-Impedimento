namespace MaSistemas.ViewModel
{
  public class SistemaMensagemViewModel : BaseViewModel
  {
    public SistemaMensagemViewModel (){
      Para = new HashSet<SistemaMensagemParaViewModel>();
      Caixa = new HashSet<SistemaMensagemCaixaViewModel>();
    }

    public int UsuarioId { get; set; }
    public string Titulo { get; set; } = "";   
    public string Texto { get; set; } = "";
    public virtual SistemaUsuarioViewModel De { get; set; }
    public virtual ICollection<SistemaMensagemParaViewModel> Para { get; set; }
    public virtual ICollection<SistemaMensagemCaixaViewModel> Caixa { get; set; }
  }
}