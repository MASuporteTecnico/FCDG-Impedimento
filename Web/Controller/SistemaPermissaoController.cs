using MaSistemas.Business;
using MaSistemas.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaSistemas.Web.Controllers;

[Authorize]
[VeriricaPermissao] 
[ApiController]
[Route("/api/Sistema/Permissao")]
public class SistemaPermissaoController : ControllerBase
{
  [Route("Index")]
  [HttpPost]
  public IActionResult Index(PaginacaoViewModel paginacao)
  {
    SistemaPermissaoBusiness Business = new();
    List<SistemaPermissaoViewModel> View;
    AjaxResponse<List<SistemaPermissaoViewModel>> Retorno = new();

    try
    {
      View = Business.Index(ref paginacao);
    }
    catch (Exception erro)
    {
      Retorno.Sucesso = false;
      Retorno.Mensagem = erro.Message;
      return BadRequest(Retorno);
    }

    Retorno.Dados = View;
    Retorno.Paginacao = paginacao;

    return Ok(Retorno);
  }

  [Route("Edit/{id:int}")]
  [HttpGet]
  public IActionResult Edit(int id)
  {
    SistemaPermissaoBusiness Business = new();
    SistemaPermissaoViewModel View = new();
    AjaxResponse<SistemaPermissaoViewModel> Retorno = new();
    try
    {
      View = Business.SelectOne(p => p.Id == id);
      Retorno.Dados = View;
    }
    catch (Exception erro)
    {
      Retorno.Sucesso = false;
      Retorno.Mensagem = erro.Message;
      return BadRequest(Retorno);
    }

    return Ok(Retorno);
  }

  [Route("Save")]
  [HttpPut]
  public IActionResult Save(SistemaPermissaoViewModel view)
  {
    SistemaPermissaoBusiness Business = new();
    AjaxResponse<SistemaPermissaoViewModel> Retorno = new();

    try
    {
      SistemaUsuarioViewModel colaborador = Funcoes.GetUsuarioSistema(HttpContext);
      Business.Save(colaborador, view);
      Retorno.Dados = view;
      Retorno.Mensagem = "Permissão Salva.";
    }
    catch (Exception erro)
    {
      Retorno.Sucesso = false;
      Retorno.Mensagem = erro.Message + (erro.InnerException?.Message ?? "");
      return BadRequest(Retorno);
    }

    return Ok(Retorno);
  }

  [Route("Delete")]
  [HttpDelete]
  public IActionResult Delete(SistemaPermissaoViewModel view)
  {
    SistemaPermissaoBusiness Business = new();
    AjaxResponse<SistemaPermissaoViewModel> Retorno = new();

    try
    {
      SistemaUsuarioViewModel colaborador = Funcoes.GetUsuarioSistema(HttpContext);
      Business.Delete(colaborador, view);
      Retorno.Dados = view;
      Retorno.Mensagem = "Permissão Excluída.";
    }
    catch (Exception erro)
    {
      Retorno.Sucesso = false;
      Retorno.Mensagem = erro.Message + (erro.InnerException?.Message ?? "");
      return BadRequest(Retorno);
    }

    return Ok(Retorno);
  }
}
