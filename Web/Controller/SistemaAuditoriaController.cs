using MaSistemas.Business;
using MaSistemas.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaSistemas.Web.Controller;

[ApiController]
[Authorize]
[VeriricaPermissao]
[Route("/api/Sistema/Auditoria")]
public class SistemaAuditoriaController : ControllerBase
{

  [Route("Index")]
  [HttpPost]
  public IActionResult Index(PaginacaoViewModel paginacao)
  {
    AjaxResponse<List<SistemaAuditoriaViewModel>> Retorno = new();
    SistemaAuditoriaBusiness Business = new();

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

}