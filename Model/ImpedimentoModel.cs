using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSistemas.Model
{
  [Table("Impedimento")]
  public partial class ImpedimentoModel : BaseModel
  {
    [Key]
    public override int Id { get; set; }
    public DateOnly DataImpedimento { get; set; }
    public int AdvogadoId { get; set; }
    public string Objeto { get; set; }

    // Navegação: advogado responsável pelo impedimento
    [ForeignKey("AdvogadoId")]
    public virtual AdvogadoModel Advogado { get; set; }

  }
}
