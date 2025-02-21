using MaSistemas.Business;
using MaSistemas.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaSistemas.Web.Controller;

[ApiController]
[Authorize]
[VeriricaPermissao]
[Route("/api/Sistema/Menu")]
public class SistemaMenuController : ControllerBase
{

  [Route("Edit/{id:int}")]
  [HttpGet]
  public IActionResult Edit(int id)
  {
    AjaxResponse<SistemaMenuViewModel> Retorno = new();
    SistemaMenuBusiness Business = new();

    //Foça ter apenas 1 regisrto
    id = 1;

    try
    {
      Retorno.Dados = Business.SelectOne(x => x.Id == id);

      return Ok(Retorno);
    }
    catch (Exception erro)
    {
      Retorno.Mensagem = erro.Message;
      Retorno.Sucesso = false;
      return BadRequest(Retorno);
    }
  }

  [Route("Save")]
  [HttpPut]
  public IActionResult Save(SistemaMenuViewModel view)
  {
    AjaxResponse<SistemaMenuViewModel> Retorno = new();
    SistemaMenuBusiness Business = new();

    //Foça ter apenas 1 regisrto
    int Id = 1;

    try
    {
      SistemaUsuarioViewModel usuario = Funcoes.GetUsuarioSistema(HttpContext);
      Business.Save(usuario, view);
      Retorno.Dados = Business.SelectOne(x => x.Id == Id);
      Retorno.Mensagem = "Menu salvo.";

      return Ok(Retorno);
    }
    catch (Exception erro)
    {
      Retorno.Mensagem = erro.Message;
      Retorno.Sucesso = false;
      return BadRequest(Retorno);
    }
  }

}