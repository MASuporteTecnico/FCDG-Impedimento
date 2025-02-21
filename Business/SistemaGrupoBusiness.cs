using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using MaSistemas.Model;
using MaSistemas.ViewModel;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;

namespace MaSistemas.Business
{
  public class SistemaGrupoBusiness : BaseBusiness<SistemaGrupoViewModel, SistemaGrupoModel, PaginacaoViewModel>
  {
    private readonly MaSistemasContext _context = new();

    public SistemaGrupoBusiness()
    {
      _context = new MaSistemasContext();
    }

    public override void Delete(SistemaUsuarioViewModel usuario, SistemaGrupoViewModel entity)
    {
      throw new NotImplementedException();
    }

    public override void Dispose()
    {
      _context.Dispose();
    }

    public override SistemaGrupoViewModel EntityToView(SistemaGrupoModel entity)
    {
      if (entity == null)
      {
        return new SistemaGrupoViewModel();
      }

      SistemaGrupoViewModel view = (SistemaGrupoViewModel)(new SistemaGrupoViewModel(){
        Usuarios = GrupoUsuariosModelToView(entity.Usuarios),
        Menus = GrupoMenusModelToView(entity.Menus)
      }).InjectFrom(entity);

      return view;
    }

    public ICollection<SistemaGrupoMenuViewModel> GrupoMenusModelToView(ICollection<SistemaGrupoMenuModel> entity)
    {

      if (entity == null)
      {
        return new List<SistemaGrupoMenuViewModel>();
      }

      List<SistemaGrupoMenuViewModel> view = (
            from i in entity
            select (SistemaGrupoMenuViewModel)(new SistemaGrupoMenuViewModel
            {
              Menu = (i.Menu != null) ? (SistemaMenuViewModel)(new SistemaMenuViewModel()).InjectFrom(i.Menu) : null
            }).InjectFrom(i)
        ).ToList();

      return view;
    }

    public ICollection<SistemaGrupoMenuModel> GrupoMenusViewToModel(ICollection<SistemaGrupoMenuViewModel> entity)
    {

      if (entity == null)
      {
        return null;
      }

      List<SistemaGrupoMenuModel> view = (
            from i in entity
            select (SistemaGrupoMenuModel)(new SistemaGrupoMenuModel ()).InjectFrom(i)
        ).ToList();

      return view;
    }

    public ICollection<SistemaGrupoUsuarioViewModel> GrupoUsuariosModelToView(ICollection<SistemaGrupoUsuarioModel> entity)
    {

      if (entity == null)
      {
        return new List<SistemaGrupoUsuarioViewModel>();
      }

      List<SistemaGrupoUsuarioViewModel> view = (
            from i in entity
            select (SistemaGrupoUsuarioViewModel)(new SistemaGrupoUsuarioViewModel
            {
              Usuario = (i.Usuario != null) ? (SistemaUsuarioViewModel)(new SistemaUsuarioViewModel()).InjectFrom(i.Usuario) : null
            }).InjectFrom(i)
        ).ToList();

      return view;
    }

    public ICollection<SistemaGrupoUsuarioModel> GrupoUsuariosViewToModel(ICollection<SistemaGrupoUsuarioViewModel> entity)
    {

      if (entity == null)
      {
        return null;
      }

      List<SistemaGrupoUsuarioModel> view = (
            from i in entity
            select (SistemaGrupoUsuarioModel)(new SistemaGrupoUsuarioModel()).InjectFrom(i)
        ).ToList();

      return view;
    }

    public override List<SistemaGrupoViewModel> Index(ref PaginacaoViewModel paginacao)
    {

      //Ajuste de Filtro e Ordenação
      MontaOrderBylist<SistemaGrupoModel> odbList = new();
      AplicaOrderBy<SistemaGrupoModel> appOdb = new();
      OrderByExpression<SistemaGrupoModel>[] oderByExp = odbList.Montar(paginacao).ToArray();

      JsonNode jsonFiltro = JsonSerializer.Deserialize<JsonNode>(paginacao.Filtro.GetRawText());
      string Nome = jsonFiltro["Busca"].GetValue<String>() ?? "";
      bool flInativos = jsonFiltro["Inativos"].GetValue<bool?>() == null || jsonFiltro["Inativos"].GetValue<bool>();


      Expression<Func<SistemaGrupoModel, bool>> flATivo = a => a.Ativo;
      Expression<Func<SistemaGrupoModel, bool>> filtroNome = a => a.Nome.Contains(Nome);
      // Fim do Ajuste de  Filtro e Ordenação

      List<SistemaGrupoViewModel> view = new();

      IQueryable<SistemaGrupoModel> model = _context.SistemaGruposModel.Where(x => !x.UsoInterno);

      if (Nome != "")
        model = model.Where(filtroNome);

      if (flInativos == false)
      {
        model = model.Where(flATivo);
      }

      if (oderByExp.Length > 0)
        model = appOdb.Ordenar(model, oderByExp);

      view = (from u in model
              select (SistemaGrupoViewModel)new SistemaGrupoViewModel().InjectFrom(u)
              ).ToList();

      paginacao.itemsLength = view.Count;
      paginacao.pageCount = Convert.ToInt32(Math.Ceiling((Decimal)paginacao.itemsLength / paginacao.itemsPerPage));
      paginacao.page = (paginacao.page > paginacao.pageCount) ? 1 : paginacao.page;

      return view.Skip((paginacao.page - 1) * paginacao.itemsPerPage).Take(paginacao.itemsPerPage).ToList();

    }

    public override void Save(SistemaUsuarioViewModel colaborador, SistemaGrupoViewModel entity)
    {
      SistemaGrupoModel model = ViewToEntity(entity, EnumOperacao.Alterar);

      if (model.Id == 0)
      {
        _context.SistemaGruposModel.Add(model);
      }
      else
      {
        _context.SistemaGruposModel.Attach(model);
      }

      _context.SaveChanges();
    }

    public override SistemaGrupoViewModel SelectOne(Expression<Func<SistemaGrupoModel, bool>> pCondicao)
    {

      SistemaGrupoModel model = _context.SistemaGruposModel
                            .Where(pCondicao)
                            .Where(x => !x.UsoInterno)
                            .Include(x => x.Menus).ThenInclude(y => y.Menu)
                            .Include(x => x.Usuarios).ThenInclude(y => y.Usuario)
                            .FirstOrDefault();

      SistemaGrupoViewModel view = EntityToView(model);
      return view;
    }

    public override SistemaGrupoModel ViewToEntity(SistemaGrupoViewModel view, EnumOperacao operacao)
    {
      if (view == null)
      {
        return null;
      }

      SistemaGrupoModel model = _context.SistemaGruposModel
                                .Include(x => x.Usuarios)
                                .Include(x => x.Menus)
                                .Where(x => x.Id == view.Id)
                                .FirstOrDefault() ?? new SistemaGrupoModel();

      model.InjectFrom(view);
      model.Menus = GrupoMenusViewToModel(view.Menus);
      model.Usuarios = GrupoUsuariosViewToModel(view.Usuarios);

      return model;
    }
  }
}