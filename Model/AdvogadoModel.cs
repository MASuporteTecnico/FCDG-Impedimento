using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSistemas.Model
{
  [Table("Advogados")]
  public partial class AdvogadoModel : BaseModel
  {
    [Key]
    public override int Id { get; set; }

    [StringLength(100)]
    public string Nome { get; set; }

    [StringLength(100)]
    public string Telefone { get; set; }

    [StringLength(100)]
    public string Email { get; set; }

    [StringLength(3)]
    public string Sigla { get; set; }

  }
}
