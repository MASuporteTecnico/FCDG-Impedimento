#nullable enable

namespace MaSistemas.ViewModel
{
  public partial class SistemaAuditoriaViewModel : BaseViewModel
  {
    public override int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Classe { get; set; } = "";
    public int ClasseId { get; set; }
    public string ValorAnterior { get; set; } = "";
    public string ValorNovo { get; set; } = "";
    public EnumOperacao Operacao { get; set; }
    public DateTime DataAlteracao { get; set; }
    public SistemaUsuarioViewModel Usuario { get; set; } = default!;
  }
}
