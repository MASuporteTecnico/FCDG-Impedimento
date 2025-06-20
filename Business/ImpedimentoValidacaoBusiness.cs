using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using MaSistemas.Model;
using MaSistemas.ViewModel;
using Omu.ValueInjecter;

namespace MaSistemas.Business
{
  public class ImpedimentoVerificacaoBusiness : BaseBusiness<ImpedimentoVerificacaoViewModel, ImpedimentoVerificacaoModel, PaginacaoViewModel>
  {
    private readonly MaSistemasContext _context = new();

    public ImpedimentoVerificacaoBusiness()
    {
      _context = new MaSistemasContext();
    }

    public override void Delete(SistemaUsuarioViewModel usuario, ImpedimentoVerificacaoViewModel view)
    {
      ImpedimentoVerificacaoValidator validador = new();
      validador.ValidaExclusao(view);
      ImpedimentoVerificacaoModel model = ViewToEntity(view, EnumOperacao.Excluir);

      //Para Auditoria
      _context.Operador = (SistemaUsuarioModel)(new SistemaUsuarioModel()).InjectFrom(usuario);

      _context.ImpedimentoVerificacaoModel.Remove(model);
      _context.SaveChanges();
    }

    public override void Dispose()
    {
      _context.Dispose();
    }

    public override ImpedimentoVerificacaoViewModel EntityToView(ImpedimentoVerificacaoModel entity)
    {
      if (entity == null)
      {
        return new ImpedimentoVerificacaoViewModel();
      }

      ImpedimentoVerificacaoViewModel view = (ImpedimentoVerificacaoViewModel)(new ImpedimentoVerificacaoViewModel()).InjectFrom(entity);

      return view;
    }

    public override List<ImpedimentoVerificacaoViewModel> Index(ref PaginacaoViewModel paginacao)
    {

      //Ajuste de Filtro e Ordenação
      MontaOrderBylist<ImpedimentoVerificacaoModel> odbList = new();
      AplicaOrderBy<ImpedimentoVerificacaoModel> appOdb = new();
      OrderByExpression<ImpedimentoVerificacaoModel>[] oderByExp = odbList.Montar(paginacao).ToArray();

      JsonNode jsonFiltro = JsonSerializer.Deserialize<JsonNode>(paginacao.Filtro.GetRawText());
      string Nome = jsonFiltro["Busca"].GetValue<String>() ?? "";
      bool flInativos = jsonFiltro["Inativos"].GetValue<bool?>() == null || jsonFiltro["Inativos"].GetValue<bool>();


      //Expression<Func<ImpedimentoVerificacaoModel, bool>> flATivo = a => a.Ativo;
      // Expression<Func<ImpedimentoVerificacaoModel, bool>> filtroNome = a => a.Nome.Contains(Nome);
      // Fim do Ajuste de  Filtro e Ordenação

      List<ImpedimentoVerificacaoViewModel> view = new();

      IQueryable<ImpedimentoVerificacaoModel> model = _context.ImpedimentoVerificacaoModel;

      // if (Nome != "")
      //   model = model.Where(filtroNome);

      if (flInativos == false)
      {
        //model = model.Where(flATivo);
      }

      if (oderByExp.Length > 0)
        model = appOdb.Ordenar(model, oderByExp);

      view = (from u in model
              select (ImpedimentoVerificacaoViewModel)new ImpedimentoVerificacaoViewModel().InjectFrom(u)
              ).ToList();

      paginacao.itemsLength = view.Count;
      paginacao.pageCount = Convert.ToInt32(Math.Ceiling((Decimal)paginacao.itemsLength / paginacao.itemsPerPage));
      paginacao.page = (paginacao.page > paginacao.pageCount) ? 1 : paginacao.page;

      return view.Skip((paginacao.page - 1) * paginacao.itemsPerPage).Take(paginacao.itemsPerPage).ToList();

    }

    public override void Save(SistemaUsuarioViewModel colaborador, ImpedimentoVerificacaoViewModel view)
    {
      ImpedimentoVerificacaoModel model;
      ImpedimentoVerificacaoValidator validador = new();
      
      //Para Auditoria
      _context.Operador = (SistemaUsuarioModel)(new SistemaUsuarioModel()).InjectFrom(colaborador);

      if (view.Id == 0)
      {
        validador.ValidaInclusao(view);
        model = ViewToEntity(view, EnumOperacao.Incluir);
        _context.ImpedimentoVerificacaoModel.Add(model);
      }
      else
      {
        validador.ValidaAlteracao(view);
        model = ViewToEntity(view, EnumOperacao.Alterar);
        _context.ImpedimentoVerificacaoModel.Attach(model);
      }
      
      _context.SaveChanges();
    }

    public override ImpedimentoVerificacaoViewModel SelectOne(Expression<Func<ImpedimentoVerificacaoModel, bool>> pCondicao)
    {
      ImpedimentoVerificacaoModel model = _context.ImpedimentoVerificacaoModel
                            .Where(pCondicao)
                            .FirstOrDefault();

      ImpedimentoVerificacaoViewModel view = EntityToView(model);
      return view;
    }

    public override ImpedimentoVerificacaoModel ViewToEntity(ImpedimentoVerificacaoViewModel view, EnumOperacao operacao)
    {
      if (view == null)
      {
        return null;
      }

      ImpedimentoVerificacaoModel model = _context.ImpedimentoVerificacaoModel.Where(x => x.Id == view.Id).FirstOrDefault() ?? new ImpedimentoVerificacaoModel();
      model.InjectFrom(view);
      return model;
    }
  }
}