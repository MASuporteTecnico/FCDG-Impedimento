
namespace MaSistemas.ViewModel
{
  public partial class EmpresaViewModel : BaseViewModel
  {
    public override int Id { get; set; }
    public string CpfCnpj { get; set; } = "";
    public string Nome { get; set; } = "";
    public bool Ativo { get; set; } = false;
    public bool AdministradoraGlobal { get; set; } = false;
    public virtual ICollection<SistemaUsuarioViewModel> Usuarios { get; set; } = [];
  }
}


