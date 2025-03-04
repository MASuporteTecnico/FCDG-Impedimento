using MaSistemas.Business;
using MaSistemas.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaSistemas.Web.Controller;

[ApiController]
[Authorize]
[Route("/api/Sistema/Mensagem")]
public class SistemaMensagemController : ControllerBase
{

  [Route("Edit/{id:int}")]
  [HttpGet]
  public IActionResult Edit(int id)
  {
    AjaxResponse<SistemaMensagemViewModel> Retorno = new();
    SistemaMensagemBusiness Business = new();

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
  public IActionResult Save(SistemaMensagemViewModel view)
  {
    AjaxResponse<SistemaMensagemViewModel> Retorno = new();
    SistemaMensagemBusiness Business = new();

    try
    {
      SistemaUsuarioViewModel usuario = Funcoes.GetUsuarioSistema(HttpContext);
      Business.Save(usuario, view);
      Retorno.Dados = view;
      Retorno.Mensagem = "Mensagem salva.";

      return Ok(Retorno);
    }
    catch (Exception erro)
    {
      Retorno.Mensagem = erro.Message;
      Retorno.Sucesso = false;
      return BadRequest(Retorno);
    }
  }

  [Route("SetarLidaNaoLida")]
  [HttpPut]
  public IActionResult SetarLidaNaoLida(SistemaMensagemViewModel view)
  {
    AjaxResponse<SistemaMensagemViewModel> Retorno = new();
    SistemaMensagemBusiness Business = new();

    try
    {
      SistemaUsuarioViewModel usuario = Funcoes.GetUsuarioSistema(HttpContext);
      Business.SetarLidaNaoLida(usuario,view);
      Retorno.Dados = view;
      Retorno.Mensagem = "Mensagem salva.";

      return Ok(Retorno);
    }
    catch (Exception erro)
    {
      Retorno.Mensagem = erro.Message;
      Retorno.Sucesso = false;
      return BadRequest(Retorno);
    }
  }

  [Route("Mensagens")]
  [HttpGet]
  public IActionResult Mensagens()
  {
    AjaxResponse<List<SistemaMensagemViewModel>> Retorno = new();
    SistemaMensagemBusiness Business = new();

    try
    {
      SistemaUsuarioViewModel usuario = Funcoes.GetUsuarioSistema(HttpContext);
      Retorno.Dados = Business.Mensagens(usuario);

      return Ok(Retorno);
    }
    catch (Exception erro)
    {
      Retorno.Mensagem = erro.Message;
      Retorno.Sucesso = false;
      return BadRequest(Retorno);
    }
  }

  [Route("Delete")]
  [HttpDelete]
  public IActionResult Delete(SistemaMensagemViewModel view)
  {
    AjaxResponse<SistemaMensagemViewModel> Retorno = new();
    SistemaMensagemBusiness Business = new();

    try
    {
      SistemaUsuarioViewModel usuario = Funcoes.GetUsuarioSistema(HttpContext);
      Business.Delete(usuario, view);
      Retorno.Dados = view;
      Retorno.Mensagem = "Mensagem deletada.";

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