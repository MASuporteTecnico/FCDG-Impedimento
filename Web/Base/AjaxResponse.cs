//Classe de resposta HTTP padrão, para as requisições Ajax
using MaSistemas.ViewModel;

namespace MaSistemas.Web
{
  public class AjaxResponse<T> where T : class, new()
  {
    public bool Sucesso { get; set; } = true;
    public string Mensagem { get; set; } = "";
    public T Dados { get; set; } = new T();
    public int NivelAcesso { get; set; }
    public PaginacaoViewModel Paginacao { get; set; } = new();
  }
}