using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MaSistemas.Model
{

  public static class EnumExtension
  {
    public static string GetDescription(Enum value)
    {
      // Get the Description attribute value for the enum value
      FieldInfo fi = value.GetType().GetField(value.ToString());
      DescriptionAttribute[] attributes =
          (DescriptionAttribute[])fi.GetCustomAttributes(
              typeof(DescriptionAttribute), false);

      if (attributes.Length > 0)
      {
        return attributes[0].Description;
      }
      else
      {
        return value.ToString();
      }
    }
  }

  public class ListaMenuAcesso
  {

    public int IdMenu { get; set; }
    //  public int? IdGrupo {get;set;}
    //  public int? IdUsuario {get;set;}

  }

  public class SistemaLogControllerAction
  {

    [Key]
    public int Id { get; set; }
    public int IdUsuario { get; set; }
    public string Usuario { get; set; }
    public string Log { get; set; }
    public string HashLog { get; set; }

  }

}