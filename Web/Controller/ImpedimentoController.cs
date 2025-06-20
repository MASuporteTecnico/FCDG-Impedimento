using MaSistemas.Business;
using MaSistemas.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaSistemas.Web.Controller;

[ApiController]
[Authorize]
[VeriricaPermissao]
[Route("/api/Cadastro/Impedimento")]
public class ImpedimentoController : ControllerBase
{
  [Route("Index")]
  [HttpPost]
  public IActionResult Index(PaginacaoViewModel paginacao)
  {
    AjaxResponse<List<ImpedimentoViewModel>> Retorno = new();
    ImpedimentoBusiness Business = new();

    try
    {
      Retorno.Dados = Business.Index(ref paginacao);
      Retorno.Paginacao = paginacao;

      return Ok(Retorno);
    }
    catch (Exception erro)
    {
      Retorno.Mensagem = erro.Message;
      Retorno.Sucesso = false;
      return BadRequest(Retorno);
    }
  }


  [Route("Edit/{id:int}")]
  [HttpGet]
  public IActionResult Edit(int id)
  {
    AjaxResponse<ImpedimentoViewModel> Retorno = new();
    ImpedimentoBusiness Business = new();

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
  public IActionResult Save(ImpedimentoViewModel view)
  {
    AjaxResponse<ImpedimentoViewModel> Retorno = new();
    ImpedimentoBusiness Business = new();

    try
    {
      SistemaUsuarioViewModel usuario = Funcoes.GetUsuarioSistema(HttpContext);
      Business.Save(usuario, view);
      Retorno.Dados = view;
      Retorno.Mensagem = "Impedimento salva.";

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
  public IActionResult Delete(ImpedimentoViewModel view)
  {
    AjaxResponse<ImpedimentoViewModel> Retorno = new();
    ImpedimentoBusiness Business = new();

    try
    {
      SistemaUsuarioViewModel usuario = Funcoes.GetUsuarioSistema(HttpContext);
      Business.Delete(usuario, view);
      Retorno.Dados = view;
      Retorno.Mensagem = "Impedimento deletada.";

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