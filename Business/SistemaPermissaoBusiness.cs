using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using MaSistemas.Model;
using MaSistemas.ViewModel;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;

namespace MaSistemas.Business
{
  public class SistemaPermissaoBusiness : BaseBusiness<SistemaPermissaoViewModel, SistemaPermissaoModel, PaginacaoViewModel>
  {
    private readonly MaSistemasContext _context = new();

    public SistemaPermissaoBusiness()
    {
      _context = new MaSistemasContext();
    }

    public override void Delete(SistemaUsuarioViewModel usuario, SistemaPermissaoViewModel entity)
    {
      SistemaPermissaoModel model = ViewToEntity(entity, EnumOperacao.Excluir);
      _context.Operador = (SistemaUsuarioModel)(new SistemaUsuarioModel()).InjectFrom(usuario);
      _context.SistemaPermissoesModel.Remove(model);

      _context.SaveChanges();
    }

    public override void Dispose()
    {
      _context.Dispose();
    }

    public override SistemaPermissaoViewModel EntityToView(SistemaPermissaoModel entity)
    {
      if (entity == null)
      {
        var n = new SistemaPermissaoViewModel();
        return n;
      }

      SistemaPermissaoViewModel view = new();
      view.InjectFrom(entity);
      view.Menu = (entity.Menu != null) ? (SistemaMenuViewModel)(new SistemaMenuViewModel()).InjectFrom(entity.Menu) : null;
      view.Usuario = (entity.Usuario != null) ? (SistemaUsuarioViewModel)(new SistemaUsuarioViewModel()).InjectFrom(entity.Usuario) : null;
      view.GrupoUsuario = (entity.GrupoUsuario != null) ? (SistemaGrupoViewModel)(new SistemaGrupoViewModel()).InjectFrom(entity.GrupoUsuario) : null;
      view.GrupoMenu = (entity.GrupoMenu != null) ? (SistemaGrupoViewModel)(new SistemaGrupoViewModel()).InjectFrom(entity.GrupoMenu) : null;
      return view;
    }

    public override void Save(SistemaUsuarioViewModel usuario, SistemaPermissaoViewModel entity)
    {
      _context.Operador = (SistemaUsuarioModel)(new SistemaUsuarioModel()).InjectFrom(usuario);

      //Para que seja setado apenas o grupo ou o usuário, nunca os dois
      if (entity.PermissaoDeGrupoUsuario == true)
      {
        entity.SistemaUsuarioId = null;
        entity.Usuario = null;
      }
      else
      {
        entity.SistemaGrupoUsuarioId = null;
        entity.GrupoUsuario = null;
      }
      ////////////////////////////////////////////////
      
      /////Para que seja setado apenas o grupo de menu ou o menu, nunca os dois
      if (entity.PermissaoDeGrupoMenu == true)
      {
        entity.SistemaMenuId = null;
        entity.Menu = null;
      }
      else
      {
        entity.SistemaGrupoMenuId = null;
        entity.GrupoMenu = null;
      }
      ////////////////////////////////////////////////

      var model = ViewToEntity(entity, EnumOperacao.Incluir);

      if (model.Id == 0)
      {
        _context.SistemaPermissoesModel.Add(model);
      }
      else
      {
        _context.SistemaPermissoesModel.Attach(model);
      }

      _context.SaveChanges();
    }

    public override List<SistemaPermissaoViewModel> Index(ref PaginacaoViewModel paginacao)
    {

      MontaOrderBylist<SistemaPermissaoModel> odbList = new();
      AplicaOrderBy<SistemaPermissaoModel> appOdb = new();
      OrderByExpression<SistemaPermissaoModel>[] oderByExp = [.. odbList.Montar(paginacao)];

      JsonNode jsonFiltro = JsonSerializer.Deserialize<JsonNode>(paginacao.Filtro.GetRawText());
      string Nome = jsonFiltro["Busca"].GetValue<String>() ?? "";
      bool flInativos = jsonFiltro["Inativos"].GetValue<bool?>() == null || jsonFiltro["Inativos"].GetValue<bool>();

      Expression<Func<SistemaPermissaoModel, bool>> flATivo = a => a.Ativo;
      Expression<Func<SistemaPermissaoModel, bool>> filtroNome = a => a.GrupoUsuario.Nome.Contains(Nome) || a.Usuario.Nome.Contains(Nome) || a.Menu.Nome.Contains(Nome) || a.GrupoMenu.Nome.Contains(Nome);

      IQueryable<SistemaPermissaoModel> model = _context.SistemaPermissoesModel
      .Where(x => x.UsoInterno != true)
      .Include(x => x.Usuario).Include(x => x.GrupoUsuario).Include(x => x.Menu).Include(x => x.GrupoMenu);

      if (Nome != "")
        model = model.Where(filtroNome);

      if (flInativos == false)
      {
        model = model.Where(flATivo);
      }


      model = appOdb.Ordenar(model, oderByExp);

      IQueryable<SistemaPermissaoViewModel> view = (
          from i in model
          select (SistemaPermissaoViewModel)(new SistemaPermissaoViewModel()
          {
            Menu = (SistemaMenuViewModel)(new SistemaMenuViewModel()).InjectFrom(i.Menu ?? new SistemaMenuModel()),
            Usuario = (SistemaUsuarioViewModel)(new SistemaUsuarioViewModel()).InjectFrom(i.Usuario ?? new SistemaUsuarioModel()),
            GrupoUsuario = (SistemaGrupoViewModel)(new SistemaGrupoViewModel()).InjectFrom(i.GrupoUsuario ?? new SistemaGrupoModel()),
            GrupoMenu = (SistemaGrupoViewModel)(new SistemaGrupoViewModel()).InjectFrom(i.GrupoMenu ?? new SistemaGrupoModel()),
          }).InjectFrom(i)
      );

      paginacao.itemsLength = view.Count();
      paginacao.pageCount = Convert.ToInt32(Math.Ceiling((Decimal)paginacao.itemsLength / paginacao.itemsPerPage));
      paginacao.page = (paginacao.page > paginacao.pageCount) ? 1 : paginacao.page;

      return [.. view.Skip((paginacao.page - 1) * paginacao.itemsPerPage).Take(paginacao.itemsPerPage)];
    }

    public override SistemaPermissaoViewModel SelectOne(Expression<Func<SistemaPermissaoModel, bool>> pCondicao)
    {
      SistemaPermissaoModel model = new();

      model = _context.SistemaPermissoesModel
      .Where(pCondicao)
      .Include(x => x.Usuario).Include(x => x.GrupoUsuario).Include(x => x.Menu).Include(x => x.GrupoMenu)
      .FirstOrDefault();

      SistemaPermissaoViewModel view = EntityToView(model);
      return view;
    }

    public override SistemaPermissaoModel ViewToEntity(SistemaPermissaoViewModel view, EnumOperacao operacao)
    {
      if (view == null)
      {
        return null;
      }

      SistemaPermissaoModel model = _context.SistemaPermissoesModel
                            .Include(x => x.Usuario)
                            .Include(x => x.Menu)
                            .Include(x => x.GrupoUsuario)
                            .Include(x => x.GrupoMenu)
                            .Where(x => x.Id == view.Id).FirstOrDefault() ?? new SistemaPermissaoModel();

      if (operacao == EnumOperacao.Excluir)
        return model;

      model.InjectFrom(view);
      model.Menu = (view.Menu != null) ? _context.SistemaMenusModel.Where(x => x.Id == view.Menu.Id).FirstOrDefault() : null;
      model.Usuario = (view.Usuario != null) ? _context.SistemaUsuariosModel.Where(x => x.Id == view.Usuario.Id).FirstOrDefault() : null;
      model.GrupoUsuario = (view.GrupoUsuario != null) ? _context.SistemaGruposModel.Where(x => x.Id == view.GrupoUsuario.Id).FirstOrDefault() : null;
      model.GrupoMenu = (view.GrupoMenu != null) ? _context.SistemaGruposModel.Where(x => x.Id == view.GrupoMenu.Id).FirstOrDefault() : null;

      return model;
    }
  }
}