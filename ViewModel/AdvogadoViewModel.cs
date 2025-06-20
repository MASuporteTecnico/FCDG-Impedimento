using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSistemas.ViewModel
{
  public partial class AdvogadoViewModel : BaseViewModel
  {
    [Key]
    public override int Id { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public string Sigla { get; set; }

    // Navegação: impedimentos do qual é responsável
    public virtual ICollection<ImpedimentoViewModel> ImpedimentosResponsavel { get; set; }

    // Navegação: verificações que este advogado realizou
    public virtual ICollection<ImpedimentoVerificacaoViewModel> Verificacoes { get; set; }
  }
}
