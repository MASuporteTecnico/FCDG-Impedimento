using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSistemas.Model
{
  [Table("ImpedimentoVerificacao")]
  public partial class ImpedimentoVerificacaoModel : BaseModel
  {
    [Key]
    public override int Id { get; set; }
    public int ImpedimentoId { get; set; }
    public int AdvogadoId { get; set; }
    public string FlagImpedimento { get; set; }
    public DateTime DataVerificacao { get; set; }
    public string Observacao { get; set; }

    [ForeignKey("AdvogadoId")]
    public virtual AdvogadoModel Advogado { get; set; }

    [ForeignKey("ImpedimentoId")]
    public virtual ImpedimentoModel Impedimento { get; set; }

  }
}
