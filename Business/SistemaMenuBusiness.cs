using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using MaSistemas.Model;
using MaSistemas.ViewModel;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;

namespace MaSistemas.Business
{
  public class SistemaMenuBusiness : BaseBusiness<SistemaMenuViewModel, SistemaMenuModel, PaginacaoViewModel>
  {
    private readonly MaSistemasContext _context;

    public SistemaMenuBusiness()
    {
      _context = new MaSistemasContext();
    }

    public override void Delete(SistemaUsuarioViewModel usuario, SistemaMenuViewModel entity)
    {
      throw new NotImplementedException();
    }

    public override void Dispose()
    {
      _context.Dispose();
    }

    public override SistemaMenuViewModel EntityToView(SistemaMenuModel entity)
    {
      if (entity == null)
      {
        return new SistemaMenuViewModel();
      }

      SistemaMenuViewModel view = (SistemaMenuViewModel)(new SistemaMenuViewModel() {
        SubMenu = SubmenuModelToView(entity.SubMenu)
      }).InjectFrom(entity);

      return view;
    }

    public List<SistemaMenuViewModel> SubmenuModelToView(ICollection<SistemaMenuModel> model)
    {
      if (model == null)
      {
        return new List<SistemaMenuViewModel>();
      }

      List<SistemaMenuViewModel> lista = (
                from i in model.OrderBy(x => x.Ordem)
                select (SistemaMenuViewModel)(new SistemaMenuViewModel()
                {
                  SubMenu = SubmenuModelToView(i.SubMenu)
                }).InjectFrom(i)
            ).ToList();

      return lista;
    }

    public List<SistemaMenuModel> SubmenuViewToModel(ICollection<SistemaMenuViewModel> model)
    {
      if (model == null)
      {
        return new List<SistemaMenuModel>();
      }

      int Ordem = 1;

      foreach(SistemaMenuViewModel m in model)
      {
        m.Ordem = Ordem++;        
      }
      
      List<SistemaMenuModel> lista = new();

      foreach(SistemaMenuViewModel m in model)
      {
        SistemaMenuModel menu;
        if(m.Id == 0)
          menu = new();
        else
          menu = _context.SistemaMenusModel.Where(x => x.Id == m.Id).FirstOrDefault();

        menu.InjectFrom(m);
        menu.SubMenu = SubmenuViewToModel(m.SubMenu);

        lista.Add(menu);
      }

      return lista;
    }

    public override List<SistemaMenuViewModel> Index(ref PaginacaoViewModel paginacao)
    {

      //Ajuste de Filtro e Ordenação
      MontaOrderBylist<SistemaMenuModel> odbList = new();
      AplicaOrderBy<SistemaMenuModel> appOdb = new();
      OrderByExpression<SistemaMenuModel>[] oderByExp = odbList.Montar(paginacao).ToArray();

      JsonNode jsonFiltro = JsonSerializer.Deserialize<JsonNode>(paginacao.Filtro.GetRawText());
      string Nome = jsonFiltro["Busca"].GetValue<String>() ?? "";
      bool flInativos = jsonFiltro["Inativos"].GetValue<bool?>() == null || jsonFiltro["Inativos"].GetValue<bool>();


      Expression<Func<SistemaMenuModel, bool>> flATivo = a => a.Ativo;
      Expression<Func<SistemaMenuModel, bool>> filtroNome = a => a.Nome.Contains(Nome);
      // Fim do Ajuste de  Filtro e Ordenação

      List<SistemaMenuViewModel> view = new();

      IQueryable<SistemaMenuModel> model = _context.SistemaMenusModel;

      if (Nome != "")
        model = model.Where(filtroNome);

      if (flInativos == false)
      {
        model = model.Where(flATivo);
      }

      if (oderByExp.Length > 0)
        model = appOdb.Ordenar(model, oderByExp);

      view = (from u in model
              select (SistemaMenuViewModel)new SistemaMenuViewModel().InjectFrom(u)
              ).ToList();

      paginacao.itemsLength = view.Count;
      paginacao.pageCount = Convert.ToInt32(Math.Ceiling((Decimal)paginacao.itemsLength / paginacao.itemsPerPage));
      paginacao.page = (paginacao.page > paginacao.pageCount) ? 1 : paginacao.page;

      return view.Skip((paginacao.page - 1) * paginacao.itemsPerPage).Take(paginacao.itemsPerPage).ToList();

    }

    public override void Save(SistemaUsuarioViewModel colaborador, SistemaMenuViewModel entity)
    {
      SistemaMenuModel model = ViewToEntity(entity,EnumOperacao.Alterar);

      if(model.Id == 0)
      {
        _context.SistemaMenusModel.Add(model);
      }else{
        _context.SistemaMenusModel.Attach(model);        
      }      
      _context.SaveChanges();

      //Limpa menus removidos das tabelas
      _context.SistemaMenusModel.RemoveRange(_context.SistemaMenusModel.Where(x => x.Id > 1 && x.MenuPai == null));
      _context.SaveChanges();
      
    }

    public override SistemaMenuViewModel SelectOne(Expression<Func<SistemaMenuModel, bool>> pCondicao)
    {

      SistemaMenuModel model = _context.SistemaMenusModel
                              .Include(x => x.SubMenu).ThenInclude(x => x.SubMenu)
                              .Where(pCondicao)
                              .FirstOrDefault();

      SistemaMenuViewModel view = EntityToView(model);    
      return view;
    }

    public override SistemaMenuModel ViewToEntity(SistemaMenuViewModel view, EnumOperacao operacao)
    {
      if (view == null)
      {
        return null;
      }

      SistemaMenuModel model = _context.SistemaMenusModel
                              .Where(x => x.Id == view.Id)
                              .Include(x => x.SubMenu).ThenInclude(y => y.SubMenu)
                              .FirstOrDefault() ?? new SistemaMenuModel();

      
      model.InjectFrom(view);
      model.SubMenu = [.. SubmenuViewToModel(view.SubMenu)];

      return model;
    }
    
  }
}