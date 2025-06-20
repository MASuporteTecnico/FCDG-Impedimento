using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using MaSistemas.Model;
using MaSistemas.ViewModel;
using Omu.ValueInjecter;

namespace MaSistemas.Business
{
  public class AdvogadoBusiness : BaseBusiness<AdvogadoViewModel, AdvogadoModel, PaginacaoViewModel>
  {
    private readonly MaSistemasContext _context = new();

    public AdvogadoBusiness()
    {
      _context = new MaSistemasContext();
    }

    public override void Delete(SistemaUsuarioViewModel usuario, AdvogadoViewModel view)
    {
      AdvogadoValidator validador = new();
      validador.ValidaExclusao(view);
      AdvogadoModel model = ViewToEntity(view, EnumOperacao.Excluir);

      //Para Auditoria
      _context.Operador = (SistemaUsuarioModel)(new SistemaUsuarioModel()).InjectFrom(usuario);

      _context.AdvogadosModel.Remove(model);
      _context.SaveChanges();
    }

    public override void Dispose()
    {
      _context.Dispose();
    }

    public override AdvogadoViewModel EntityToView(AdvogadoModel entity)
    {
      if (entity == null)
      {
        return new AdvogadoViewModel();
      }

      AdvogadoViewModel view = (AdvogadoViewModel)(new AdvogadoViewModel()).InjectFrom(entity);

      return view;
    }

    public override List<AdvogadoViewModel> Index(ref PaginacaoViewModel paginacao)
    {

      //Ajuste de Filtro e Ordenação
      MontaOrderBylist<AdvogadoModel> odbList = new();
      AplicaOrderBy<AdvogadoModel> appOdb = new();
      OrderByExpression<AdvogadoModel>[] oderByExp = odbList.Montar(paginacao).ToArray();

      JsonNode jsonFiltro = JsonSerializer.Deserialize<JsonNode>(paginacao.Filtro.GetRawText());
      string Nome = jsonFiltro["Busca"].GetValue<String>() ?? "";
      bool flInativos = jsonFiltro["Inativos"].GetValue<bool?>() == null || jsonFiltro["Inativos"].GetValue<bool>();


      //Expression<Func<AdvogadoModel, bool>> flATivo = a => a.Ativo;
      Expression<Func<AdvogadoModel, bool>> filtroNome = a => a.Nome.Contains(Nome);
      // Fim do Ajuste de  Filtro e Ordenação

      List<AdvogadoViewModel> view = new();

      IQueryable<AdvogadoModel> model = _context.AdvogadosModel;

      if (Nome != "")
        model = model.Where(filtroNome);

      if (flInativos == false)
      {
        //model = model.Where(flATivo);
      }

      if (oderByExp.Length > 0)
        model = appOdb.Ordenar(model, oderByExp);

      view = (from u in model
              select (AdvogadoViewModel)new AdvogadoViewModel().InjectFrom(u)
              ).ToList();

      paginacao.itemsLength = view.Count;
      paginacao.pageCount = Convert.ToInt32(Math.Ceiling((Decimal)paginacao.itemsLength / paginacao.itemsPerPage));
      paginacao.page = (paginacao.page > paginacao.pageCount) ? 1 : paginacao.page;

      return view.Skip((paginacao.page - 1) * paginacao.itemsPerPage).Take(paginacao.itemsPerPage).ToList();

    }

    public override void Save(SistemaUsuarioViewModel colaborador, AdvogadoViewModel view)
    {
      AdvogadoModel model;
      AdvogadoValidator validador = new();
      
      //Para Auditoria
      _context.Operador = (SistemaUsuarioModel)(new SistemaUsuarioModel()).InjectFrom(colaborador);

      if (view.Id == 0)
      {
        validador.ValidaInclusao(view);
        model = ViewToEntity(view, EnumOperacao.Incluir);
        _context.AdvogadosModel.Add(model);
      }
      else
      {
        validador.ValidaAlteracao(view);
        model = ViewToEntity(view, EnumOperacao.Alterar);
        _context.AdvogadosModel.Attach(model);
      }
      
      _context.SaveChanges();
    }

    public override AdvogadoViewModel SelectOne(Expression<Func<AdvogadoModel, bool>> pCondicao)
    {
      AdvogadoModel model = _context.AdvogadosModel
                            .Where(pCondicao)
                            .FirstOrDefault();

      AdvogadoViewModel view = EntityToView(model);
      return view;
    }

    public override AdvogadoModel ViewToEntity(AdvogadoViewModel view, EnumOperacao operacao)
    {
      if (view == null)
      {
        return null;
      }

      AdvogadoModel model = _context.AdvogadosModel.Where(x => x.Id == view.Id).FirstOrDefault() ?? new AdvogadoModel();
      model.InjectFrom(view);
      return model;
    }
  }
}