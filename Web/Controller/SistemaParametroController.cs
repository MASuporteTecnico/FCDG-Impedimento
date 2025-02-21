using MaSistemas.Business;
using MaSistemas.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaSistemas.Web.Controller;

[ApiController]
[Authorize]
[VeriricaPermissao]
[Route("/api/Sistema/Parametro")]
public class SistemaParametroController : ControllerBase
{

  [Route("Edit/{id:int}")]
  [HttpGet]
  public IActionResult Edit(int id)
  {
    AjaxResponse<SistemaParametroViewModel> Retorno = new();
    SistemaParametroBusiness Business = new();

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
  public IActionResult Save(SistemaParametroViewModel view)
  {
    AjaxResponse<SistemaParametroViewModel> Retorno = new();
    SistemaParametroBusiness Business = new();

    //Foça ter apenas 1 regisrto
    int Id = 1;

    try
    {
      SistemaUsuarioViewModel usuario = Funcoes.GetUsuarioSistema(HttpContext);
      Business.Save(usuario, view);
      Retorno.Dados = Business.SelectOne(x => x.Id == Id);
      Retorno.Mensagem = "Parâmetros salvos.";

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