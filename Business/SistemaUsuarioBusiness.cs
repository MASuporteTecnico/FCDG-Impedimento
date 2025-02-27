using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using MaSistemas.Model;
using MaSistemas.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Omu.ValueInjecter;

namespace MaSistemas.Business
{
  public class SistemaUsuarioBusiness : BaseBusiness<SistemaUsuarioViewModel, SistemaUsuarioModel, PaginacaoViewModel>
  {

    private readonly MaSistemasContext _context = new();
    private readonly SistemaUsuarioViewModel _usuario;
    private readonly string _ServerUrl;
    private static readonly Random random = new();

    public SistemaUsuarioBusiness()
    {
      _context = new MaSistemasContext();
    }
    public SistemaUsuarioBusiness(SistemaUsuarioViewModel usuario, string ServerUrl = "")
    {
      _usuario = usuario;
      _context = new MaSistemasContext();
      _ServerUrl = ServerUrl;
    }

    public override void Delete(SistemaUsuarioViewModel usuario, SistemaUsuarioViewModel view)
    {
      SistemaUsuarioValidator validador = new();
      validador.ValidaExclusao(view);
      SistemaUsuarioModel model = ViewToEntity(view, EnumOperacao.Excluir);
      _context.SistemaUsuariosModel.Remove(model);
      _context.SaveChanges();
    }

    public override void Dispose()
    {
      _context.Dispose();
    }

    public override SistemaUsuarioViewModel EntityToView(SistemaUsuarioModel entity)
    {
      if (entity == null)
      {
        return new SistemaUsuarioViewModel();
      }

      SistemaUsuarioViewModel view = (SistemaUsuarioViewModel)(new SistemaUsuarioViewModel()
      {
        Empresa = (EmpresaViewModel)(new EmpresaViewModel()).InjectFrom(entity.Empresa ?? new EmpresaModel())
      }).InjectFrom(entity);


      // Sem envio da senha para o FrontEnd
      view.Senha = string.Empty;

      return view;
    }

    public override List<SistemaUsuarioViewModel> Index(ref PaginacaoViewModel paginacao)
    {

      //Ajuste de Filtro e Ordenação
      MontaOrderBylist<SistemaUsuarioModel> odbList = new();
      AplicaOrderBy<SistemaUsuarioModel> appOdb = new();
      OrderByExpression<SistemaUsuarioModel>[] oderByExp = odbList.Montar(paginacao).ToArray();

      JsonNode jsonFiltro = JsonSerializer.Deserialize<JsonNode>(paginacao.Filtro.GetRawText());
      string Nome = jsonFiltro["Busca"].GetValue<String>() ?? "";
      bool flInativos = jsonFiltro["Inativos"].GetValue<bool?>() == null || jsonFiltro["Inativos"].GetValue<bool>();

      Expression<Func<SistemaUsuarioModel, bool>> flATivo = a => a.Ativo;
      Expression<Func<SistemaUsuarioModel, bool>> filtroNome = a => a.Nome.Contains(Nome) || a.EMail.Contains(Nome);
      // Fim do Ajuste de  Filtro e Ordenação

      List<SistemaUsuarioViewModel> view = new();

      IQueryable<SistemaUsuarioModel> model = _context.SistemaUsuariosModel
                                              .Include(x => x.Empresa)
                                              .Where(x => x.Id > 1); //Não listar o Admin do sistema

      if (Nome != "")
        model = model.Where(filtroNome);

      if (flInativos == false)
      {
        model = model.Where(flATivo);
      }

      if (oderByExp.Length > 0)
        model = appOdb.Ordenar(model, oderByExp);

      view = (from u in model
              select (SistemaUsuarioViewModel)(new SistemaUsuarioViewModel()
              {
                Empresa = (EmpresaViewModel)(new EmpresaViewModel()).InjectFrom(u.Empresa ?? new EmpresaModel())
              }).InjectFrom(u)
              ).ToList();

      paginacao.itemsLength = view.Count;
      paginacao.pageCount = Convert.ToInt32(Math.Ceiling((Decimal)paginacao.itemsLength / paginacao.itemsPerPage));
      paginacao.page = (paginacao.page > paginacao.pageCount) ? 1 : paginacao.page;

      return view.Skip((paginacao.page - 1) * paginacao.itemsPerPage).Take(paginacao.itemsPerPage).ToList();

    }

    public override void Save(SistemaUsuarioViewModel colaborador, SistemaUsuarioViewModel entity)
    {

      SistemaUsuarioModel model;

      //Validador de cadastro de Usuário
      SistemaUsuarioValidator validator = new();
      if (entity.Id == 0)
      {
        validator.ValidaInclusao(entity);
        model = ViewToEntity(entity, EnumOperacao.Incluir);
        _context.SistemaUsuariosModel.Add(model);
      }
      else //Se não, atualiza um registro
      {
        validator.ValidaAlteracao(entity);
        model = ViewToEntity(entity, EnumOperacao.Alterar);
        _context.SistemaUsuariosModel.Attach(model);
      }

      _context.SaveChanges();
    }

    public override SistemaUsuarioViewModel SelectOne(Expression<Func<SistemaUsuarioModel, bool>> pCondicao)
    {
      SistemaUsuarioModel model = _context.SistemaUsuariosModel
                                  .Where(pCondicao)
                                  .Include(x => x.Empresa)
                                  .FirstOrDefault();

      if (model?.Id == 1)
        throw new InvalidOperationException("Acesso Negado");

      SistemaUsuarioViewModel view = EntityToView(model);
      return view;
    }

    public override SistemaUsuarioModel ViewToEntity(SistemaUsuarioViewModel view, EnumOperacao operacao)
    {
      if (view == null)
      {
        return null;
      }

      SistemaUsuarioModel model = _context.SistemaUsuariosModel
                          .Include(x => x.Empresa)
                          .Where(x => x.Id == view.Id)
                          .FirstOrDefault() ?? new SistemaUsuarioModel();

      //Caso a senha não seja alterada, estes dados voltarão ao model.
      SenhaComSaltHashResult SenhaSalt = new(model.Salt, model.Senha);

      model.InjectFrom(view);

      if (operacao == EnumOperacao.Excluir)
        return model;

      model.Empresa = (view.Empresa != null) ? _context.EmpresasModel.Where(x => x.Id == view.Empresa.Id).FirstOrDefault() : null;


      if (view.Senha.Trim() != string.Empty)
      {
        SenhaSalt = GetNovaSenhacomSalt(RandomNumberGenerator.GetBytes(128 / 8), view.Senha);
      }

      model.Senha = SenhaSalt.Senha;
      model.Salt = SenhaSalt.Salt;

      return model;
    }

    public SistemaUsuarioViewModel TrocarSenha(SistemaUsuarioViewModel usuarioTrocaDeSenha, bool RecuperacaoDeSenha = false)
    {
      SistemaUsuarioModel usuario;
      SistemaUsuarioViewModel view = new();

      try
      {

        if (string.IsNullOrEmpty(usuarioTrocaDeSenha.NovaSenha))
          throw new InvalidOperationException("Nova senha não pode estar em branco.");

        if (usuarioTrocaDeSenha.NovaSenha != usuarioTrocaDeSenha.ConfirmaSenha)
          throw new InvalidOperationException("Nova senha não confere com a confirmação.");

        if (RecuperacaoDeSenha)
          usuario = _context.SistemaUsuariosModel.Where(x => x.ChaveResetSenha == usuarioTrocaDeSenha.ChaveResetSenha && x.Ativo).FirstOrDefault();
        else
          usuario = _context.SistemaUsuariosModel.Where(x => x.Id == _usuario.Id && x.Ativo).FirstOrDefault();

        if (usuario != null)
        {
          if (!RecuperacaoDeSenha)
          {
            SenhaComSaltHashResult SenhaAtual = GetNovaSenhacomSalt(Convert.FromBase64String(usuario.Salt), usuarioTrocaDeSenha.Senha);

            if (SenhaAtual.Senha != usuario.Senha)
              throw new InvalidOperationException("Senha atual inválida.");
          }

          SenhaComSaltHashResult NovaSenhaSalt = GetNovaSenhacomSalt(RandomNumberGenerator.GetBytes(128 / 8), usuarioTrocaDeSenha.NovaSenha);
          usuario.Salt = NovaSenhaSalt.Salt;
          usuario.Senha = NovaSenhaSalt.Senha;
          usuario.ChaveResetSenha = null;

          _context.SistemaUsuariosModel.Attach(usuario);
          _context.SaveChanges();

          view.InjectFrom(usuario);
          view.Senha = "";

        }
        else
        {
          throw new InvalidOperationException("Conta de usuário inválida.");
        }

      }
      catch (InvalidOperationException exception)
      {
        throw new InvalidOperationException(exception.Message);
      }
      catch
      {
        throw new Exception("Erro ao atualizar senha.");
      }

      return view;
    }

    public Task<bool> MenuLateral(SistemaUsuarioViewModel usuario, bool Ativo)
    {
      SistemaUsuarioModel usrMenu = _context.SistemaUsuariosModel.Where(x => x.Id == usuario.Id).FirstOrDefault();
      usrMenu.MenuLateral = Ativo;
      _context.Attach(usrMenu);
      _context.SaveChanges();

      return Task.Run(() => Ativo);
    }

    public Task<SistemaMenuViewModel> MenuUsuario(SistemaUsuarioViewModel usuario)
    {

      List<SistemaRotaPermissaoViewModel> rotas = new();
      bool AdminSistema = false;

      //Buscando Permissaão de Grupo
      List<SistemaGrupoModel> grupos = [.. _context.SistemaGruposModel.Where(x => x.Usuarios.Any(x => x.Usuario.Id == usuario.Id))];
      foreach (SistemaGrupoModel group in grupos)
      {

        if (group.AdminSistema)
        {
          AdminSistema = true;
          break;
        }

        List<SistemaPermissaoModel> MenuGrupos = [.. _context.SistemaPermissoesModel
                                          .Include(x => x.Menu)
                                          .Include(x => x.GrupoMenu).ThenInclude(y => y.Menus ).ThenInclude(z => z.Menu)
                                          .Include(x => x.GrupoUsuario).ThenInclude(y => y.Menus)
                                          .Where(x => x.SistemaGrupoUsuarioId == group.Id)];

        foreach (SistemaPermissaoModel MenuGrupo in MenuGrupos)
        {
          if (MenuGrupo != null)
          {
            int Permissao = 0;
            Permissao += MenuGrupo.Index ? 1 : 0;
            Permissao += MenuGrupo.Edit ? 2 : 0;
            Permissao += MenuGrupo.Save ? 4 : 0;

            if (!MenuGrupo.PermissaoDeGrupoMenu)
            {

              SistemaRotaPermissaoViewModel r = rotas.Where(x => x.Rota == MenuGrupo.Menu.Rota).FirstOrDefault();
              if (r == null)
                rotas.Add(new SistemaRotaPermissaoViewModel() { Rota = MenuGrupo.Menu.Rota, Permissao = Permissao });
              else
              {
                if (r.Permissao < Permissao)
                {
                  rotas.Remove(r);
                  rotas.Add(new SistemaRotaPermissaoViewModel() { Rota = MenuGrupo.Menu.Rota, Permissao = Permissao });
                }
              }
            }
            else
            {
              foreach (SistemaGrupoMenuModel menu in MenuGrupo.GrupoMenu.Menus.ToList())
              {
                if (menu.Menu != null)
                {
                  SistemaRotaPermissaoViewModel r = rotas.Where(x => x.Rota == menu.Menu.Rota).FirstOrDefault();
                  if (r == null)
                  {
                    rotas.Add(new SistemaRotaPermissaoViewModel() { Rota = menu.Menu.Rota, Permissao = Permissao });
                  }
                  else
                  {
                    if (r.Permissao < Permissao)
                    {
                      rotas.Remove(r);
                      rotas.Add(new SistemaRotaPermissaoViewModel() { Rota = menu.Menu.Rota, Permissao = Permissao });
                    }
                  }
                }
              }
            }
          }
        }
      }

      if (AdminSistema)
      {
        rotas.Clear();
        List<SistemaMenuModel> RotasAdmin = [.. _context.SistemaMenusModel];

        RotasAdmin.ForEach(x =>
        {
          rotas.Add(new SistemaRotaPermissaoViewModel() { Rota = x.Rota, Permissao = 10 });
        });
      }
      else
      {
        //Buscando Permissaão de Usuário
        List<SistemaPermissaoModel> MenuUsuarioPermissao = [.. _context.SistemaPermissoesModel
                                          .Include(x => x.Menu)
                                          .Include(x => x.GrupoMenu).ThenInclude(y => y.Menus).ThenInclude(z => z.Menu)
                                          .Include(x => x.Usuario).Include(y => y.Menu)
                                          .Where(x => x.SistemaUsuarioId == usuario.Id)];

        foreach (SistemaPermissaoModel MenuUsuario in MenuUsuarioPermissao)
        {
          if (MenuUsuario != null)
          {
            int Permissao = 0;
            Permissao += MenuUsuario.Index ? 1 : 0;
            Permissao += MenuUsuario.Edit ? 2 : 0;
            Permissao += MenuUsuario.Save ? 4 : 0;


            if (!MenuUsuario.PermissaoDeGrupoMenu)
            {
              SistemaRotaPermissaoViewModel r = rotas.Where(x => x.Rota == MenuUsuario.Menu.Rota).FirstOrDefault();
              if (r != null)
                rotas.Remove(r);

              rotas.Add(new SistemaRotaPermissaoViewModel() { Rota = MenuUsuario.Menu.Rota, Permissao = Permissao });

            }
            else
            {
              foreach (SistemaGrupoMenuModel menu in MenuUsuario.GrupoMenu.Menus.ToList())
              {
                if (menu.Menu != null)
                {
                  SistemaRotaPermissaoViewModel r = rotas.Where(x => x.Rota == menu.Menu.Rota).FirstOrDefault();
                  if (r != null && !MenuUsuario.PermissaoDeGrupoUsuario)
                  {
                    rotas.Remove(r);

                    rotas.Add(new SistemaRotaPermissaoViewModel() { Rota = menu.Menu.Rota, Permissao = Permissao });
                  }
                  else
                  {

                    if (r == null)
                    {
                      rotas.Add(new SistemaRotaPermissaoViewModel() { Rota = menu.Menu.Rota, Permissao = Permissao });
                    }
                    else
                    {
                      if (r.Permissao < Permissao)
                      {
                        rotas.Remove(r);
                        rotas.Add(new SistemaRotaPermissaoViewModel() { Rota = menu.Menu.Rota, Permissao = Permissao });
                      }
                    }

                  }
                }
              }
            }
          }
        }
      }

      List<SistemaMenuModel> model = [.. _context.SistemaMenusModel
                              .Include(x => x.SubMenu).ThenInclude(x => x.SubMenu)
                              .Where(x => x.Id == 1)
                              .SelectMany(x => x.SubMenu)];

      if (AdminSistema)
      {
        List<SistemaMenuViewModel> view = (
                from i in model.OrderBy(x => x.Ordem)
                select (SistemaMenuViewModel)(new SistemaMenuViewModel()
                {
                  SubMenu = SubmenuModelToViewAdmin(i.SubMenu)
                }).InjectFrom(i)
            ).ToList();

        return Task.Run(() => new SistemaMenuViewModel()
        {
          SubMenu = view,
          Rotas = [.. rotas]

        });
      }
      else
      {
        List<SistemaMenuViewModel> view = (
                from i in model.OrderBy(x => x.Ordem)
                select (SistemaMenuViewModel)(new SistemaMenuViewModel()
                {
                  SubMenu = SubmenuModelToView(i.SubMenu, rotas)
                }).InjectFrom(i)
            ).ToList();


        List<SistemaMenuViewModel> temp = [.. view];

        temp.ForEach(x =>
        {
          if (x.Rota == "/#" && x.SubMenu.Count == 0)
          {
            SistemaMenuViewModel sistemaMenuViewModel = view.Where(y => y.Id == x.Id).FirstOrDefault();
            view.Remove(sistemaMenuViewModel);
          }
        });

        return Task.Run(() => new SistemaMenuViewModel()
        {
          SubMenu = view,
          Rotas = [.. rotas]
        });

      }
    }

    public List<SistemaMenuViewModel> SubmenuModelToView(ICollection<SistemaMenuModel> model, List<SistemaRotaPermissaoViewModel> Rotas)
    {
      if (model == null)
      {
        return new List<SistemaMenuViewModel>();
      }

      List<SistemaMenuViewModel> lista = new();

      foreach (SistemaMenuModel a in model.OrderBy(x => x.Ordem))
      {
        if (a.MenuPaiId == 1)
        {
          lista.Add((SistemaMenuViewModel)(new SistemaMenuViewModel()
          {
            SubMenu = SubmenuModelToView(a.SubMenu, Rotas)
          }).InjectFrom(a));
        }
        else
        {
          var rot = Rotas.Where(x => x.Rota.Contains(a.Rota)).FirstOrDefault();
          if (rot != null)
          {
            lista.Add((SistemaMenuViewModel)(new SistemaMenuViewModel()
            {
              SubMenu = SubmenuModelToView(a.SubMenu, Rotas)
            }).InjectFrom(a));
          }
        }
      }

      return lista;
    }

    public List<SistemaMenuViewModel> SubmenuModelToViewAdmin(ICollection<SistemaMenuModel> model)
    {
      if (model == null)
      {
        return new List<SistemaMenuViewModel>();
      }

      List<SistemaMenuViewModel> lista = (
                from i in model.OrderBy(x => x.Ordem)
                select (SistemaMenuViewModel)(new SistemaMenuViewModel()
                {
                  SubMenu = SubmenuModelToViewAdmin(i.SubMenu)
                }).InjectFrom(i)
            ).ToList();

      return lista;
    }

    public SistemaUsuarioViewModel Login(SistemaUsuarioViewModel view)
    {

      SistemaUsuarioModel model = _context.SistemaUsuariosModel
      .Where(x => x.EMail == view.EMail && x.Ativo)
      .Include(x => x.Empresa)
      .FirstOrDefault();

      if (model == null)
      {
        throw new InvalidOperationException("Login inválido.");
      }
      else
      {

        //Senha com Salt será verificada depois        
        SenhaComSaltHashResult senhaCrypt = GetNovaSenhacomSalt(Convert.FromBase64String(model.Salt), view.Senha);
        if (senhaCrypt.Senha != model.Senha)
          throw new InvalidOperationException("Login inválido.");

      }

      SistemaUsuarioViewModel Usuario = EntityToView(model);
      return Usuario;
    }

    public SistemaUsuarioViewModel Logout()
    {
      return new SistemaUsuarioViewModel();
    }

    public static string RandomString(int length)
    {
      const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
      return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static SenhaComSaltHashResult GetNovaSenhacomSalt(byte[] salt, string senha)
    {
      byte[] passwordAsBytes = Encoding.UTF8.GetBytes(senha);

      List<byte> passWithSaltBytes = new();
      passWithSaltBytes.AddRange(salt);
      passWithSaltBytes.AddRange(passwordAsBytes);

      byte[] digestBytes = SHA512.Create().ComputeHash(passWithSaltBytes.ToArray());

      return new SenhaComSaltHashResult(Convert.ToBase64String(salt), Convert.ToBase64String(digestBytes));
    }
  }

  public class SenhaComSaltHashResult
  {
    public string Salt { get; }
    public string Senha { get; set; }

    public SenhaComSaltHashResult(string salt, string digest)
    {
      Salt = salt;
      Senha = digest;
    }

  }
}