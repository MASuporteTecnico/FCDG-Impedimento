using System.ComponentModel;

namespace MaSistemas.ViewModel
{
  public class BaseViewModel
  {
    public virtual int Id {get; set;}
  }

  public enum EnumOperacao
  {
    [Description("I")]
    Incluir,
    
    [Description("A")]
    Alterar,

    [Description("E")]
    Excluir
  }

}