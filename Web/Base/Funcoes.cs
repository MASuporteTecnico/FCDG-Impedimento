using System.Text.Json;
using MaSistemas.ViewModel;

namespace MaSistemas.Web
{
  public static class Funcoes
  {
    public static SistemaUsuarioViewModel GetUsuarioSistema(HttpContext context)
    {
      try
      {
        SistemaUsuarioViewModel colaborador = JsonSerializer.Deserialize<SistemaUsuarioViewModel>(context.User.Claims.Where(x => x.Type == "SistemaUsuario").Select(x => x.Value).FirstOrDefault());
        return colaborador;
      }
      catch
      {
        return new SistemaUsuarioViewModel();
      }
    }

  }
}