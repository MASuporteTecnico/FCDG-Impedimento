using System.Net;
using MaSistemas.Model;
using MaSistemas.ViewModel;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;


namespace MaSistemas.Business
{
  public class VerificaPermissaoBusiness : IDisposable
  {

    const int FORBIDDEN = (int)HttpStatusCode.Forbidden;
    const int UNAUTHORIZED = (int)HttpStatusCode.Unauthorized;
    const int OK = (int)HttpStatusCode.OK;

    private readonly MaSistemasContext _context = new();

    public VerificaPermissaoBusiness()
    {
      _context = new MaSistemasContext();
    }

    public void Dispose()
    {
      _context.Dispose();
    }

    public int VerificaPermissao(SistemaUsuarioViewModel usuario, ActionExecutingContext contexto)
    {

      string ControllerName = ((ControllerActionDescriptor)contexto.ActionDescriptor).ControllerName ?? "";
      string ControllerAction = ((ControllerActionDescriptor)contexto.ActionDescriptor).ActionName ?? "";


      string RequestFullPath = contexto.HttpContext.Request.Path.ToString();
      int IndexAction = RequestFullPath.LastIndexOf("/" + ControllerAction);
      string ApiRequestPath = RequestFullPath.Substring(0, IndexAction).Replace("/api", "");

      // Se for um acesso fora o sistema, nego o acesso...  
      if ((ControllerName == "") || (ControllerAction == ""))
      {
        return FORBIDDEN;
      }

      // Gravar os LOGs de acesso às rotas
      // using (var context = new MaSistemasContext())
      // {
      //   var dbLog = new SistemaLogControllerAction { };
      //   List<string> lines = new List<string>();
      //   if (ControllerAction != "IsLogged")
      //   {
      //     dbLog.IdUsuario = usuario.Id;
      //     dbLog.Usuario = usuario.Nome;
      //     dbLog.Log = contexto.HttpContext.Request.Path.Value + " ==> " + ControllerName + "/" + ControllerAction;
      //     dbLog.HashLog = "";
      //     context.SistemaLogControllerActions.Add(dbLog);
      //     context.SaveChanges();
      //   }
      //}
      
      bool sOk = false;

      //Verificação por Grupo de Menu
      List<SistemaGrupoModel> GrupoMenu = [.. _context.SistemaGruposModel.Where( x => x.Menus.Any( y => y.Menu.Rota == ApiRequestPath))];
      foreach (SistemaGrupoModel group in GrupoMenu)
      {
        if (group.AdminSistema)
          return OK;

        SistemaPermissaoModel MenuGrupo = _context.SistemaPermissoesModel.Where(x => x.SistemaGrupoMenuId == group.Id && x.Ativo).FirstOrDefault();
        if (MenuGrupo != null)
        {
          switch (ControllerAction.ToUpper())
          {
            case "INDEX":
              sOk = MenuGrupo.Index;
              break;

            case "EDIT":
              sOk = MenuGrupo.Edit;
              break;

            case "SAVE":
              sOk = MenuGrupo.Save;
              break;

            case "DELETE":
              sOk = MenuGrupo.Save;
              break;

            default:
              break;
          }
        }
      }

      //Verificação por Grupo de Usuário
      List<SistemaGrupoModel> GrupoUsuario = [.. _context.SistemaGruposModel.Where(x => x.Usuarios.Any( x => x.SistemaUsuarioId == usuario.Id))];

      //Verificar se é do grupo admin do sistema, se sim, tem acesso a tudo
      foreach (SistemaGrupoModel group in GrupoUsuario)
      {
        if (group.AdminSistema)
          return OK;

        //Verifica Permissão de Grupo
        SistemaPermissaoModel MenuGrupo = _context.SistemaPermissoesModel.Where(x => x.Menu.Rota == ApiRequestPath && x.SistemaGrupoUsuarioId == group.Id && x.Ativo).FirstOrDefault();
        if (MenuGrupo != null)
        {
          switch (ControllerAction.ToUpper())
          {
            case "INDEX":
              sOk = MenuGrupo.Index;
              break;

            case "EDIT":
              sOk = MenuGrupo.Edit;
              break;

            case "SAVE":
              sOk = MenuGrupo.Save;
              break;

            case "DELETE":
              sOk = MenuGrupo.Save;
              break;

            default:
              break;
          }
        }
      }
      
      //Verifica Permissão de Usuário
      SistemaPermissaoModel MenuUsuario = _context.SistemaPermissoesModel.Where(x => x.Menu.Rota == ApiRequestPath && x.SistemaUsuarioId == usuario.Id && x.Ativo).FirstOrDefault();
      if (MenuUsuario != null)
      {
        switch (ControllerAction.ToUpper())
        {
          case "INDEX":
            sOk = MenuUsuario.Index;
            break;

          case "EDIT":
            sOk = MenuUsuario.Edit;
            break;

          case "SAVE":
            sOk = MenuUsuario.Save;
            break;

          case "DELETE":
            sOk = MenuUsuario.Save;
            break;

          default:
            break;
        }
      }

      if (sOk)
        return OK;

      return FORBIDDEN;
    }
  }
}