namespace MaSistemas.ViewModel;

public class SistemaPermissaoViewModel
{
  public SistemaPermissaoViewModel()
  {
    PermissaoDeGrupoUsuario = false;
    PermissaoDeGrupoMenu = false;
    UsoInterno = false;
    Index = false;
    Edit = false;
    Save = false;
  }

  public int Id { get; set; }
  public int? SistemaGrupoUsuarioId { get; set; } // opção única para o grupo user
  public int? SistemaGrupoMenuId { get; set; } // opção única para o grupo user
  public int? SistemaUsuarioId { get; set; } // ou opção única para user nomeado
  public int? SistemaMenuId { get; set; } // ou opção única para user nomeado
  public bool PermissaoDeGrupoUsuario { get; set; }
  public bool PermissaoDeGrupoMenu { get; set; }
  public bool UsoInterno { get; set; }
  public bool Ativo { get; set; }

  public bool Index { get; set; } // Lita inicial da páginas
  public bool Edit { get; set; } // Apenas Leitura 
  public bool Save { get; set; } // Escrita / Delete

  public SistemaGrupoViewModel? GrupoUsuario { get; set; }
  public SistemaGrupoViewModel? GrupoMenu { get; set; }
  public SistemaUsuarioViewModel? Usuario { get; set; }
  public virtual SistemaMenuViewModel? Menu { get; set; }


}