using System.ComponentModel.DataAnnotations;
using MaSistemas.Business;
using MaSistemas.Model;
using MaSistemas.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaSistemas.Web.Controller;

[ApiController]
[Authorize]
[Route("/api/Sistema/Arquivo")]
public class ArquivoController : ControllerBase
{

  [Route("Upload")]
  [HttpPut]
  public IActionResult Upload(ArquivoViewModel entity)
  {
    var rep = new HttpResponseMessage();

    foreach (IFormFile a in HttpContext.Request.Form.Files)
    {
      entity.Arquivos.Add(a);
    }

    AjaxResponse<ArquivoModel> Retorno = new();
    ArquivoBusiness Business = new();

    Business.Upload(entity);


    return Ok(Retorno);

  }

  [Route("Save")]
  [HttpPut]
  public IActionResult Save(SistemaParametroViewModel view)
  {
    AjaxResponse<SistemaParametroViewModel> Retorno = new();
    SistemaParametroBusiness Business = new();

    //Força ter apenas 1 regisrto
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