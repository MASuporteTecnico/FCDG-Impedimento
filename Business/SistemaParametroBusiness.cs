using System.Linq.Expressions;
using MaSistemas.Model;
using MaSistemas.ViewModel;
using Omu.ValueInjecter;

namespace MaSistemas.Business
{
  public class SistemaParametroBusiness : BaseBusiness<SistemaParametroViewModel, SistemaParametroModel, PaginacaoViewModel>
  {

    private readonly MaSistemasContext _context = new();

    public SistemaParametroBusiness()
    {
      _context = new MaSistemasContext();
    }

    public override SistemaParametroViewModel SelectOne(Expression<Func<SistemaParametroModel, bool>> pCondicao)
    {
      SistemaParametroModel model = _context.SistemaParametrosModel
                                  .Where(pCondicao)
                                  .FirstOrDefault();

      SistemaParametroViewModel view = EntityToView(model);
      return view;
    }

    public override void Save(SistemaUsuarioViewModel colaborador, SistemaParametroViewModel view)
    {
      SistemaParametroModel model;
      SistemaParametroValidator validador = new();

      if (view.Id == 0)
      {
        validador.ValidaInclusao(view);
        model = ViewToEntity(view, EnumOperacao.Incluir);
        _context.SistemaParametrosModel.Add(model);
      }
      else
      {
        validador.ValidaAlteracao(view);
        model = ViewToEntity(view, EnumOperacao.Alterar);
        _context.SistemaParametrosModel.Attach(model);
      }

      _context.SaveChanges();
    }

    public override void Delete(SistemaUsuarioViewModel usuario, SistemaParametroViewModel entity)
    {
      throw new NotImplementedException();
    }

    public override List<SistemaParametroViewModel> Index(ref PaginacaoViewModel paginacao)
    {
      throw new NotImplementedException();
    }

    public override SistemaParametroViewModel EntityToView(SistemaParametroModel entity)
    {
      if (entity == null)
      {
        return new SistemaParametroViewModel();
      }

      SistemaParametroViewModel view = (SistemaParametroViewModel)(new SistemaParametroViewModel()).InjectFrom(entity);
      return view;
    }

    public override SistemaParametroModel ViewToEntity(SistemaParametroViewModel view, EnumOperacao operacao)
    {
      if (view == null)
      {
        return null;
      }

      SistemaParametroModel model = _context.SistemaParametrosModel.Where(x => x.Id == view.Id).FirstOrDefault() ?? new SistemaParametroModel();
      model.InjectFrom(view);

      return model;
    }

    public override void Dispose()
    {
      _context.Dispose();
    }
  }
}