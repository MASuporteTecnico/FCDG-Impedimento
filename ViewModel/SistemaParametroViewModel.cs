#nullable enable
namespace MaSistemas.ViewModel
{
  public partial class SistemaParametroViewModel : BaseViewModel
  {
    public override int Id {get; set;}
    public string? EmailFrom { get; set; }
    public string? EmailTo { get; set; }
    public string? EmailCc { get; set; }
    public string? EmailCco { get; set; }
    public string? EmailServidor { get; set; }
    public int EmailPorta { get; set; }
    public string? EmailLogin { get; set; }
    public string? EmailSenha { get; set; }
    public bool EmailSsl { get; set; }
    public string PastaTemporarios { get; set; } = "";
    public string PastaArquivos { get; set; } = "";
  }
}
