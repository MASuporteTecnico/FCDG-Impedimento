using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using MaSistemas.Model;
using MaSistemas.ViewModel;
using Omu.ValueInjecter;

namespace MaSistemas.Business
{
  public class EmpresaBusiness : BaseBusiness<EmpresaViewModel, EmpresaModel, PaginacaoViewModel>
  {
    private readonly MaSistemasContext _context = new();

    public EmpresaBusiness()
    {
      _context = new MaSistemasContext();
    }

    public override void Delete(SistemaUsuarioViewModel usuario, EmpresaViewModel view)
    {
      EmpresaValidator validador = new();
      validador.ValidaExclusao(view);
      EmpresaModel model = ViewToEntity(view, EnumOperacao.Excluir);
      _context.EmpresasModel.Remove(model);
      _context.SaveChanges();
    }

    public override void Dispose()
    {
      _context.Dispose();
    }

    public override EmpresaViewModel EntityToView(EmpresaModel entity)
    {
      if (entity == null)
      {
        return new EmpresaViewModel();
      }

      EmpresaViewModel view = (EmpresaViewModel)(new EmpresaViewModel()).InjectFrom(entity);

      return view;
    }

    public override List<EmpresaViewModel> Index(ref PaginacaoViewModel paginacao)
    {

      //Ajuste de Filtro e Ordenação
      MontaOrderBylist<EmpresaModel> odbList = new();
      AplicaOrderBy<EmpresaModel> appOdb = new();
      OrderByExpression<EmpresaModel>[] oderByExp = odbList.Montar(paginacao).ToArray();

      JsonNode jsonFiltro = JsonSerializer.Deserialize<JsonNode>(paginacao.Filtro.GetRawText());
      string Nome = jsonFiltro["Busca"].GetValue<String>() ?? "";
      bool flInativos = jsonFiltro["Inativos"].GetValue<bool?>() == null || jsonFiltro["Inativos"].GetValue<bool>();


      Expression<Func<EmpresaModel, bool>> flATivo = a => a.Ativo;
      Expression<Func<EmpresaModel, bool>> filtroNome = a => a.Nome.Contains(Nome);
      // Fim do Ajuste de  Filtro e Ordenação

      List<EmpresaViewModel> view = new();

      IQueryable<EmpresaModel> model = _context.EmpresasModel;

      if (Nome != "")
        model = model.Where(filtroNome);

      if (flInativos == false)
      {
        model = model.Where(flATivo);
      }

      if (oderByExp.Length > 0)
        model = appOdb.Ordenar(model, oderByExp);

      view = (from u in model
              select (EmpresaViewModel)new EmpresaViewModel().InjectFrom(u)
              ).ToList();

      paginacao.itemsLength = view.Count;
      paginacao.pageCount = Convert.ToInt32(Math.Ceiling((Decimal)paginacao.itemsLength / paginacao.itemsPerPage));
      paginacao.page = (paginacao.page > paginacao.pageCount) ? 1 : paginacao.page;

      return view.Skip((paginacao.page - 1) * paginacao.itemsPerPage).Take(paginacao.itemsPerPage).ToList();

    }

    public override void Save(SistemaUsuarioViewModel colaborador, EmpresaViewModel view)
    {
      EmpresaModel model;
      EmpresaValidator validador = new();
      if (view.Id == 0)
      {
        validador.ValidaInclusao(view);
        model = ViewToEntity(view, EnumOperacao.Incluir);
        _context.EmpresasModel.Add(model);
      }
      else
      {
        validador.ValidaAlteracao(view);
        model = ViewToEntity(view, EnumOperacao.Alterar);
        _context.EmpresasModel.Attach(model);
      }
      
      _context.SaveChanges();
    }

    public override EmpresaViewModel SelectOne(Expression<Func<EmpresaModel, bool>> pCondicao)
    {
      EmpresaModel model = _context.EmpresasModel
                            .Where(pCondicao)
                            .FirstOrDefault();

      EmpresaViewModel view = EntityToView(model);
      return view;
    }

    public override EmpresaModel ViewToEntity(EmpresaViewModel view, EnumOperacao operacao)
    {
      if (view == null)
      {
        return null;
      }

      EmpresaModel model = _context.EmpresasModel.Where(x => x.Id == view.Id).FirstOrDefault() ?? new EmpresaModel();
      model.InjectFrom(view);
      return model;
    }
  }
}