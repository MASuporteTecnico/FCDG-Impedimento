namespace MaSistemas.ViewModel;
using Microsoft.AspNetCore.Http;
public class ArquivoViewModel : BaseViewModel
{

  public ArquivoViewModel()
  {
    Arquivos = new HashSet<IFormFile>();
  }

  public override int Id { get; set; }
  public string Nome { get; set; }
  public string Descricao { get; set; }
  public string Caminho { get; set; }
  public bool ArquivoTemporario {get; set;}
  public bool ApagarArquivo { get; set; }
  public ICollection<IFormFile> Arquivos { get; set; }
}