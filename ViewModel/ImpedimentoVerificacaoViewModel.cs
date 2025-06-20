using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSistemas.ViewModel
{
  public partial class ImpedimentoVerificacaoViewModel : BaseViewModel
  {
    public override int Id { get; set; }
    public int ImpedimentoId { get; set; }
    public int AdvogadoId { get; set; }
    public string FlagImpedimento { get; set; }
    public DateTime DataVerificacao { get; set; }
    public string Observacao { get; set; }

    // Navegação: impedimentos do qual é responsável
    public virtual ICollection<ImpedimentoViewModel> ImpedimentosResponsavel { get; set; }

    // Navegação: verificações que este advogado realizou
    public virtual ICollection<ImpedimentoVerificacaoViewModel> Verificacoes { get; set; }
  }
}
