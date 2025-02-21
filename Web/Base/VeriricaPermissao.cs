using System.Net;
using MaSistemas.Business;
using MaSistemas.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
namespace MaSistemas.Web
{
  public class VeriricaPermissaoAttribute : ActionFilterAttribute, IActionFilter
  {
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {

      //Para Controllers que tenham a annotation [VeriricaPermissao]
      //Caso alguma Action tenha a annotation [PularVeriricaPermissao]
      //a verificação de permissõa será ignorada para esta Action. Mais utilizada para login, logoff, menu e etc...
      bool pularVeriricaPermissao = ((ControllerActionDescriptor)filterContext.ActionDescriptor).ControllerTypeInfo.IsDefined(typeof(PularVeriricaPermissaoAttribute), true) ||
    filterContext.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(PularVeriricaPermissaoAttribute));


      const string FORBIDDEN = "/api/Sistema/Usuario/Negado";
      const string UNAUTHORIZED = "/api/Sistema/Usuario/NaoLogado";
      string ControllerName = ((ControllerActionDescriptor)filterContext.ActionDescriptor).ControllerName ?? "";
      string ControllerAction = ((ControllerActionDescriptor)filterContext.ActionDescriptor).ActionName ?? "";

      base.OnActionExecuting(filterContext);
      
      if (!pularVeriricaPermissao)
      {
        try
        {
          SistemaUsuarioViewModel usuario = Funcoes.GetUsuarioSistema(filterContext.HttpContext);

          if (usuario == null)
          {
            filterContext.Result = new RedirectResult(UNAUTHORIZED);
          }
          else
          {
            VerificaPermissaoBusiness business = new();
            int status = business.VerificaPermissao(usuario, filterContext);

            switch (status)
            {
              case ((int)HttpStatusCode.Forbidden):
                filterContext.Result = new RedirectResult(FORBIDDEN);
                break;

              case ((int)HttpStatusCode.Unauthorized):
                filterContext.Result = new RedirectResult(UNAUTHORIZED);
                break;

              case ((int)HttpStatusCode.OK):
                //filterContext.Result = new RedirectResult(OK);
                break;

              default:
                filterContext.Result = new RedirectResult(FORBIDDEN);
                break;
            }
          }
        }
        catch(Exception erro)
        {
          Console.Write(erro.Message);
          filterContext.Result = new RedirectResult(UNAUTHORIZED);
        }
      }
    }
  }

  public class PularVeriricaPermissaoAttribute : Attribute { }
}