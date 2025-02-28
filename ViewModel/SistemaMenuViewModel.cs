namespace MaSistemas.ViewModel
{
  public class SistemaMenuViewModel : BaseViewModel
  {

    public SistemaMenuViewModel()
    {
      SubMenu = new HashSet<SistemaMenuViewModel>();
    }
    public string Nome { get; set; } = "";
    public string Rota { get; set; } = "";
    public string Icone { get; set; } = "";
    public bool Divisor { get; set; } = false;
    public int? MenuPaiId { get; set; }
    public int Ordem { get; set; }
    public bool Ativo { get; set; }
    public virtual SistemaMenuViewModel? MenuPai { get; set; }
    public virtual ICollection<SistemaMenuViewModel> SubMenu { get; set; } = [];
    public virtual ICollection<SistemaRotaPermissaoViewModel> Rotas { get; set; } = [];
  }

  public class SistemaRotaPermissaoViewModel : BaseViewModel
  {
    public string Rota { get; set; }
    public int Permissao { get; set; }
  }
}