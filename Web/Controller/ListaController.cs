using MaSistemas.Business;
using MaSistemas.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaSistemas.Web.Controller;

[ApiController]
[Authorize]
[Route("/api/Lista")]
public class ListaController : ControllerBase
{
  [Route("Empresas")]
  [HttpGet]
  public async Task<IEnumerable<ListaViewModel>> Empresas()
  {
    ListaBusiness Business = new();
    List<ListaViewModel> View = new();
    View = await Task.Run(() => Business.Empresas());

    return View;
  }

  [Route("Usuarios")]
  [HttpGet]
  public async Task<IEnumerable<ListaViewModel>> Usuarios()
  {
    ListaBusiness Business = new();
    List<ListaViewModel> View = new();
    View = await Task.Run(() => Business.Usuarios());

    return View;
  }

  [Route("Advogados")]
  [HttpGet]
  public async Task<IEnumerable<AdvogadoViewModel>> Advogados()
  {
    ListaBusiness Business = new();
    List<AdvogadoViewModel> View = new();
    View = await Task.Run(() => Business.Advogados());

    return View;
  }

  [Route("GruposUsuarios")]
  [HttpGet]
  public async Task<IEnumerable<ListaViewModel>> GruposUsuarios()
  {
    ListaBusiness Business = new();
    List<ListaViewModel> View = new();
    View = await Task.Run(() => Business.GruposUsuarios());

    return View;
  }

  [Route("GruposMenus")]
  [HttpGet]
  public async Task<IEnumerable<ListaViewModel>> GruposMenus()
  {
    ListaBusiness Business = new();
    List<ListaViewModel> View = new();
    View = await Task.Run(() => Business.GruposMenus());

    return View;
  }

  [Route("Menus")]
  [HttpGet]
  public async Task<IEnumerable<ListaViewModel>> Menus()
  {
    ListaBusiness Business = new();
    List<ListaViewModel> View = new();
    View = await Task.Run(() => Business.Menus());

    return View;
  }

}