using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using MaSistemas.Model;
using MaSistemas.ViewModel;
using Omu.ValueInjecter;

namespace MaSistemas.Business
{
  public class SistemaAuditoriaBusiness : BaseBusiness<SistemaAuditoriaViewModel, SistemaAuditoriaModel, PaginacaoViewModel>
  {
    private readonly MaSistemasContext _context = new();

    public SistemaAuditoriaBusiness()
    {
      _context = new MaSistemasContext();
    }

    public override void Delete(SistemaUsuarioViewModel usuario, SistemaAuditoriaViewModel entity)
    {
      throw new NotImplementedException();
    }

    public override void Dispose()
    {
      _context.Dispose();
    }

    public override SistemaAuditoriaViewModel EntityToView(SistemaAuditoriaModel entity)
    {
      if (entity == null)
      {
        return new SistemaAuditoriaViewModel();
      }

      SistemaAuditoriaViewModel view = (SistemaAuditoriaViewModel)(new SistemaAuditoriaViewModel()).InjectFrom(entity);

      return view;
    }

    public override List<SistemaAuditoriaViewModel> Index(ref PaginacaoViewModel paginacao)
    {

      //Ajuste de Filtro e Ordenação
      MontaOrderBylist<SistemaAuditoriaModel> odbList = new();
      AplicaOrderBy<SistemaAuditoriaModel> appOdb = new();
      OrderByExpression<SistemaAuditoriaModel>[] oderByExp = odbList.Montar(paginacao).ToArray();

      JsonNode jsonFiltro = JsonSerializer.Deserialize<JsonNode>(paginacao.Filtro.GetRawText());
      string Nome = jsonFiltro["Busca"].GetValue<String>() ?? "";
      bool flInativos = jsonFiltro["Inativos"].GetValue<bool?>() == null || jsonFiltro["Inativos"].GetValue<bool>();


      // Fim do Ajuste de  Filtro e Ordenação

      List<SistemaAuditoriaViewModel> view = new();

      IQueryable<SistemaAuditoriaModel> model = _context.SistemaAuditoriasModel;


      if (oderByExp.Length > 0)
        model = appOdb.Ordenar(model, oderByExp);

      view = [..
              (from u in model
              select (SistemaAuditoriaViewModel)new SistemaAuditoriaViewModel().InjectFrom(u)
            )];

      paginacao.itemsLength = view.Count;
      paginacao.pageCount = Convert.ToInt32(Math.Ceiling((Decimal)paginacao.itemsLength / paginacao.itemsPerPage));
      paginacao.page = (paginacao.page > paginacao.pageCount) ? 1 : paginacao.page;

      return view.Skip((paginacao.page - 1) * paginacao.itemsPerPage).Take(paginacao.itemsPerPage).ToList();

    }

    public override void Save(SistemaUsuarioViewModel colaborador, SistemaAuditoriaViewModel entity)
    {
      throw new NotImplementedException();
    }

    public override SistemaAuditoriaViewModel SelectOne(Expression<Func<SistemaAuditoriaModel, bool>> pCondicao)
    {
      throw new NotImplementedException();
    }

    public override SistemaAuditoriaModel ViewToEntity(SistemaAuditoriaViewModel view, EnumOperacao operacao)
    {
      throw new NotImplementedException();
    }
  }
}