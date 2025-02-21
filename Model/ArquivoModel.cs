using System.ComponentModel.DataAnnotations;

namespace MaSistemas.Model;
public class ArquivoModel : BaseModel
{
  [Key]
  public override int Id { get; set; }
  public string Nome { get; set; }
  public string Descricao {get; set;}
}