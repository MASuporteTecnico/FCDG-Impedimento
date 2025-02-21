namespace MaSistemas.ViewModel
{
  public class SistemaUsuarioViewModel : BaseViewModel
  {
    public override int Id { get; set; }
    public int? ClienteId { get; set; }
    public bool Ativo { get; set; } = false;
    public bool Admin { get; set; } = false;
    public string Senha { get; set; } = "";
    public string Telefone { get; set; } = "";
    public string EMail { get; set; } = "";
    public string Nome { get; set; } = "";
    public bool MenuLateral { get; set; }

    public string NovaSenha { get; set; } = "";
    public string ConfirmaSenha { get; set; } = "";
    public string ChaveResetSenha { get; set; } = "";


    public virtual EmpresaViewModel? Empresa { get; set; }
    public ICollection<SistemaGrupoUsuarioViewModel> Grupos { get; set; } = [];
  }
}