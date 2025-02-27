using MaSistemas.Model;
using MaSistemas.ViewModel;
using Microsoft.EntityFrameworkCore;


namespace MaSistemas.Business
{
  public class ListaBusiness : IDisposable
  {
    private readonly MaSistemasContext _context = new();

    public ListaBusiness()
    {
      _context = new MaSistemasContext();
    }

    public Task<List<ListaViewModel>> Empresas()
    {
      IQueryable<EmpresaModel> model = _context.EmpresasModel.Where(x => x.Ativo);

      IQueryable<ListaViewModel> view = (
                from i in model
                select new ListaViewModel
                {
                  Id = i.Id,
                  Nome = i.Nome,
                }
            );

      return view.OrderBy(x => x.Nome).ToListAsync();
    }

    public Task<List<ListaViewModel>> Usuarios()
    {
      IQueryable<SistemaUsuarioModel> model = _context.SistemaUsuariosModel.Where(x => x.Ativo && !x.Admin);

      IQueryable<ListaViewModel> view = (
                from i in model
                select new ListaViewModel
                {
                  Id = i.Id,
                  Nome = i.Nome,
                }
            );

      return view.OrderBy(x => x.Nome).ToListAsync();
    }

    public Task<List<ListaViewModel>> GruposUsuarios()
    {
      IQueryable<SistemaGrupoModel> model = _context.SistemaGruposModel.Where(x => x.Ativo && !x.GrupoDeMenu && !x.UsoInterno);

      IQueryable<ListaViewModel> view = (
                from i in model
                select new ListaViewModel
                {
                  Id = i.Id,
                  Nome = i.Nome,
                }
            );

      return view.OrderBy(x => x.Nome).ToListAsync();
    }

    public Task<List<ListaViewModel>> GruposMenus()
    {
      IQueryable<SistemaGrupoModel> model = _context.SistemaGruposModel.Where(x => x.Ativo && x.GrupoDeMenu && !x.UsoInterno);

      IQueryable<ListaViewModel> view = (
                from i in model
                select new ListaViewModel
                {
                  Id = i.Id,
                  Nome = i.Nome,
                }
            );

      return view.OrderBy(x => x.Nome).ToListAsync();
    }

    public Task<List<ListaViewModel>> Menus()
    {
      IQueryable<SistemaMenuModel> model = _context.SistemaMenusModel
                                          .Where(x => x.Ativo && x.Id > 1 && x.Rota.Length > 2);

      IQueryable<ListaViewModel> view = (
                from i in model
                select new ListaViewModel
                {
                  Id = i.Id,
                  Nome = i.Rota,
                }
            );

      return view.OrderBy(x => x.Nome).ToListAsync();
    }

    public void Dispose()
    {
      _context.Dispose();
    }
  }
}