using System.ComponentModel;
using System.Linq.Expressions;
using MaSistemas.ViewModel;

namespace MaSistemas.Business
{
  public abstract class BaseBusiness<V, E, P> : IDisposable
      where V : class
      where E : class
      where P : class
  {
    public BaseBusiness()
    {
      // TODO: Complete member initialization
    }

    public abstract V SelectOne(Expression<Func<E, bool>> pCondicao);
    public abstract void Save(SistemaUsuarioViewModel colaborador, V entity);
    public abstract void Delete(SistemaUsuarioViewModel usuario, V entity);
    public abstract List<V> Index(ref P paginacao);
    public abstract V EntityToView(E entity);
    public abstract E ViewToEntity(V view, EnumOperacao operacao);

    public abstract void Dispose();

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
