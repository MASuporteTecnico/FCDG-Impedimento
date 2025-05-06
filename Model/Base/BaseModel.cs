using System.ComponentModel;

namespace MaSistemas.Model
{
  public class BaseModel
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