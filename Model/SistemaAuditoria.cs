#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MaSistemas.Model
{
  [Table("SistemaAuditorias")]
  public partial class SistemaAuditoriaModel : BaseModel
  {
    [Key]
    public override int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Classe { get; set; } = "";
    public int ClasseId { get; set; }
    public string ValorAnterior { get; set; } = "";
    public string ValorNovo { get; set; } = "";
    public EnumOperacao Operacao { get; set; }
    public DateTime DataAlteracao { get; set; }

    [ForeignKey("UsuarioId")]
    public SistemaUsuarioModel Usuario { get; set; } = default!;
  }
}
