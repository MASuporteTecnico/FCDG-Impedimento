using MaSistemas.Business;
using MaSistemas.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaSistemas.Web.Controller;

[ApiController]
[Authorize]
[VeriricaPermissao]
[Route("/api/Cadastro/ImpedimentoVerificacao")]
public class ImpedimentoVerificacaoController : ControllerBase
{
  [Route("Index")]
  [HttpPost]
  public IActionResult Index(PaginacaoViewModel paginacao)
  {
    AjaxResponse<List<ImpedimentoVerificacaoViewModel>> Retorno = new();
    ImpedimentoVerificacaoBusiness Business = new();

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
    AjaxResponse<ImpedimentoVerificacaoViewModel> Retorno = new();
    ImpedimentoVerificacaoBusiness Business = new();

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
  public IActionResult Save(ImpedimentoVerificacaoViewModel view)
  {
    AjaxResponse<ImpedimentoVerificacaoViewModel> Retorno = new();
    ImpedimentoVerificacaoBusiness Business = new();

    try
    {
      SistemaUsuarioViewModel usuario = Funcoes.GetUsuarioSistema(HttpContext);
      Business.Save(usuario, view);
      Retorno.Dados = view;
      Retorno.Mensagem = "ImpedimentoVerificacao salva.";

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
  public IActionResult Delete(ImpedimentoVerificacaoViewModel view)
  {
    AjaxResponse<ImpedimentoVerificacaoViewModel> Retorno = new();
    ImpedimentoVerificacaoBusiness Business = new();

    try
    {
      SistemaUsuarioViewModel usuario = Funcoes.GetUsuarioSistema(HttpContext);
      Business.Delete(usuario, view);
      Retorno.Dados = view;
      Retorno.Mensagem = "ImpedimentoVerificacao deletada.";

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