namespace MaSistemas.ViewModel
{
  public class SistemaMenuViewModel : BaseViewModel
  {
    public string Nome { get; set; } = "";
    public string Rota { get; set; } = "";
    public string Icone { get; set; } = "";
    public bool Divisor { get; set; } = false;
    public int MenuPaiId { get; set; }
    public int Ordem { get; set; }
    public bool Ativo { get; set; }
    public virtual SistemaMenuViewModel? MenuPai { get; set; }
    public ICollection<SistemaMenuViewModel> SubMenu { get; set; } = [];
    public ICollection<SistemaRotaPermissaoViewModel> Rotas { get; set; } = [];
  }

  public class SistemaRotaPermissaoViewModel : BaseViewModel
  {
    public string Rota { get; set; }
    public int Permissao { get; set; }
  }
}