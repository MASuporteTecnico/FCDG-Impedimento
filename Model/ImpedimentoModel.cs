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
    public string ParteA { get; set; }
    public string ParteB { get; set; }

    // Navegação: advogado responsável pelo impedimento
    [ForeignKey("AdvogadoId")]
    public AdvogadoModel AdvogadoAdvogadoResponsavel { get; set; }

  }
}
