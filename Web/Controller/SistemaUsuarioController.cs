using System.Security.Claims;
using System.Text.Json;
using MaSistemas.Business;
using MaSistemas.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaSistemas.Web.Controller;

[ApiController]
[Authorize]
[VeriricaPermissao]
[Route("/api/Sistema/Usuario")]
public class SistemaUsuarioController : ControllerBase
{

  [AllowAnonymous]
  [PularVeriricaPermissao]
  [Route("Login")]
  [HttpPost]
  public IActionResult Login(SistemaUsuarioViewModel view)
  {
    AjaxResponse<SistemaUsuarioViewModel> Retorno = new();
    SistemaUsuarioBusiness Business = new();

    try
    {
      SistemaUsuarioViewModel ViewModel = Business.Login(view);
      Retorno.Dados = ViewModel;

      var claims = new List<Claim>() {
            new Claim(ClaimTypes.Name, ViewModel.Nome),
            new Claim("SistemaUsuario", JsonSerializer.Serialize(ViewModel)),
          };

      var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

      ClaimsPrincipal principal = new(claimsIdentity);

      var authProperties = new AuthenticationProperties
      {
        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
        IsPersistent = true
      };

      HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

      Thread.CurrentPrincipal = principal;
      HttpContext.User = principal;

      return Ok(Retorno);
    }
    catch (Exception erro)
    {
      Retorno.Mensagem = erro.Message;
      Retorno.Sucesso = false;
      return BadRequest(Retorno);
    }
  }

  [AllowAnonymous]
  [PularVeriricaPermissao]
  [Route("Logout")]
  [HttpGet]
  public IActionResult Logout()
  {
    AjaxResponse<SistemaUsuarioViewModel> Retorno = new();
    SistemaUsuarioBusiness Business = new();

    try
    {
      Retorno.Dados = Business.Logout();
      HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      HttpContext.User = null!;
      Thread.CurrentPrincipal = null;
      return Ok(Retorno);
    }
    catch (Exception erro)
    {
      Retorno.Mensagem = erro.Message;
      Retorno.Sucesso = false;
      return BadRequest(Retorno);
    }
  }

  [Route("Index")]
  [HttpPost]
  public IActionResult Index(PaginacaoViewModel paginacao)
  {
    AjaxResponse<List<SistemaUsuarioViewModel>> Retorno = new();
    SistemaUsuarioBusiness Business = new();

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
    AjaxResponse<SistemaUsuarioViewModel> Retorno = new();
    SistemaUsuarioBusiness Business = new();

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
  public IActionResult Save(SistemaUsuarioViewModel view)
  {
    AjaxResponse<SistemaUsuarioViewModel> Retorno = new();
    SistemaUsuarioBusiness Business = new();

    try
    {
      SistemaUsuarioViewModel usuario = Funcoes.GetUsuarioSistema(HttpContext);
      Business.Save(usuario, view);
      Retorno.Dados = view;
      Retorno.Mensagem = "Usuário salvo.";

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
  public IActionResult Delete(SistemaUsuarioViewModel view)
  {
    AjaxResponse<SistemaUsuarioViewModel> Retorno = new();
    SistemaUsuarioBusiness Business = new();

    try
    {
      SistemaUsuarioViewModel usuario = Funcoes.GetUsuarioSistema(HttpContext);
      Business.Delete(usuario, view);
      Retorno.Dados = view;
      Retorno.Mensagem = "Usuário deletado.";

      return Ok(Retorno);
    }
    catch (Exception erro)
    {
      Retorno.Mensagem = erro.Message;
      Retorno.Sucesso = false;
      return BadRequest(Retorno);
    }
  }

  [PularVeriricaPermissao]
  [Route("TrocarSenha")]
  [HttpPut]
  public ActionResult<SistemaUsuarioViewModel> TrocarSenha(SistemaUsuarioViewModel usuarioTrocaDeSenha)
  {

    SistemaUsuarioViewModel usuario = Funcoes.GetUsuarioSistema(HttpContext);
    AjaxResponse<SistemaUsuarioViewModel> Retorno = new();
    SistemaUsuarioBusiness Business = new(usuario);

    try
    {
      SistemaUsuarioViewModel ViewModel = Business.TrocarSenha(usuarioTrocaDeSenha);
      Retorno.Dados = ViewModel;
      Retorno.Mensagem = "Senha alterada.";
    }
    catch (Exception erro)
    {
      Retorno.Sucesso = false;
      Retorno.Mensagem = erro.Message;
      return BadRequest(Retorno);
    }

    return Ok(Retorno);
  }

  [PularVeriricaPermissao]
  [Route("MenuUsuario")]
  [HttpGet]
  public async Task<SistemaMenuViewModel> MenuUsuario()
  {
    SistemaUsuarioViewModel usuario = Funcoes.GetUsuarioSistema(HttpContext);
    SistemaUsuarioBusiness Business = new();
    SistemaMenuViewModel View = new();

    try
    {
      View = await Task.Run(() => Business.MenuUsuario(usuario));
    }
    catch (Exception erro)
    {

    }

    return View;
  }

  [PularVeriricaPermissao]
  [Route("MenuLateral/{ativo:bool}")]
  [HttpGet]
  public async Task<bool> MenuLateral(bool ativo)
  {
    SistemaUsuarioViewModel usuario = Funcoes.GetUsuarioSistema(HttpContext);
    SistemaUsuarioBusiness Business = new();
    await Task.Run(() => Business.MenuLateral(usuario, ativo));

    return ativo;
  }

  [AllowAnonymous]
  [PularVeriricaPermissao]
  [Route("Negado")]
  public IActionResult Negado()
  {
    return Forbid();
  }

  [AllowAnonymous]
  [PularVeriricaPermissao]
  [Route("NaoLogado")]
  public IActionResult NaoLogado()
  {
    return Unauthorized();
  }
}