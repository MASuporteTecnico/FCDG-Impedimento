using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSistemas.ViewModel
{
  public partial class ImpedimentoViewModel : BaseViewModel
  {
    public override int Id { get; set; }
    public DateOnly DataImpedimento { get; set; }
    public int AdvogadoId { get; set; }
    public string Objeto { get; set; }
    public string ParteA { get; set; }
    public string ParteB { get; set; }

    // Navegação: impedimentos do qual é responsável
    public AdvogadoViewModel AdvogadoResponsavel { get; set; }

    // Navegação: verificações que este advogado realizou
    // public virtual ICollection<ImpedimentoVerificacaoViewModel> Verificacoes { get; set; }
  }
}
