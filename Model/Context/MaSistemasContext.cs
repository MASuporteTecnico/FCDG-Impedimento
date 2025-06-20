using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;

namespace MaSistemas.Model;

public class MaSistemasContext : DbContext
{
  //Utilizado para auditoria
  public SistemaUsuarioModel Operador { get; set; } = default!;

  //Tabelas do DB
  public DbSet<AdvogadoModel> AdvogadosModel   { get; set; } = default!;
  public DbSet<ImpedimentoModel> ImpedimentosModel { get; set; } = default!;
  public DbSet<ImpedimentoVerificacaoModel> ImpedimentoVerificacaoModel { get; set; } = default!;
  public DbSet<EmpresaModel> EmpresasModel { get; set; } = default!;

  public DbSet<SistemaMenuModel> SistemaMenusModel { get; set; } = default!;
  public DbSet<SistemaMensagemModel> SistemaMensagensModel { get; set; } = default!;
  public DbSet<SistemaMensagemParaModel> SistemaMensagensParaModel { get; set; } = default!;
  public DbSet<SistemaMensagemCaixaModel> SistemaMensagensCaixaModel { get; set; } = default!;
  public DbSet<SistemaPermissaoModel> SistemaPermissoesModel { get; set; } = default!;
  public DbSet<SistemaUsuarioModel> SistemaUsuariosModel { get; set; } = default!;
  public DbSet<SistemaParametroModel> SistemaParametrosModel { get; set; } = default!;
  public DbSet<SistemaGrupoModel> SistemaGruposModel { get; set; } = default!;
  public DbSet<SistemaGrupoUsuarioModel> SistemaGrupoUsuariosModel { get; set; } = default!;
  public DbSet<SistemaGrupoMenuModel> SistemaGrupoMenusModel { get; set; } = default!;
  public DbSet<SistemaAuditoriaModel> SistemaAuditoriasModel { get; set; } = default!;


  public MaSistemasContext(DbContextOptions<MaSistemasContext> options) : base(options)
  {
    //
  }

  public MaSistemasContext()
  {
    //
  }

  public static MaSistemasContext Create()
  {
    return new MaSistemasContext();
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {

    IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    string ServerType = configuration["ConnectionStringsType"];

    string connectionStrings;


    switch (ServerType)
    {
      case "MYSQL":
        connectionStrings = configuration["ConnectionStrings:MYSQL"];
        optionsBuilder.UseMySql(connectionStrings, ServerVersion.AutoDetect(connectionStrings));

        //Mostrar o SL Executado no console
        //Desativar quandao  em produção
        // optionsBuilder.UseMySql(connectionStrings, ServerVersion.AutoDetect(connectionStrings))
        //               .LogTo(s => System.Diagnostics.Debug.WriteLine(s));
        break;

      case "MSSQL":
        connectionStrings = configuration["ConnectionStrings:MSSQL"];
        optionsBuilder.UseSqlServer(connectionStrings);

        //Mostrar o SQL Executado no console - PARA MSSQL SERVER
        //Desativar quandao  em produção
        /* optionsBuilder.UseSqlServer(connectionStrings).LogTo(s => System.Diagnostics.Debug.WriteLine(s)); */
        break;

      case "SQLTE":
        connectionStrings = configuration["ConnectionStrings:SQLTE"];
        optionsBuilder.UseSqlite(connectionStrings);

        //Mostrar o SQL Executado no console - PARA MSSQL SERVER
        //Desativar quandao  em produção
        /* optionsBuilder.UseSqlServer(connectionStrings).LogTo(s => System.Diagnostics.Debug.WriteLine(s)); */
        break;

      default:
        break;
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<SistemaAuditoriaModel>().Property(x => x.Operacao).HasConversion<string>();
    base.OnModelCreating(modelBuilder);
    modelBuilder.Seed();
  }

  public override int SaveChanges()
  {
    List<SistemaAuditoriaModel> changes = GetChanges();
    int result = base.SaveChanges();

    if (changes.Count > 0)
    {
      SistemaAuditoriasModel.AddRange(changes);
      base.SaveChanges(); // Salva os logs primeiro
    }

    return result;
  }

  private List<SistemaAuditoriaModel> GetChanges()
  {
    var changeLogs = new List<SistemaAuditoriaModel>();

    if (changeLogs.Count > 0)
    {
      foreach (var entry in ChangeTracker.Entries())
      {
        if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
        {
          var entityName = entry.Entity.GetType().Name;
          var entityId = entry.OriginalValues["Id"] ?? "0"; // Ajuste para sua PK
          var (oldValues, newValues) = GetEntityChanges(entry);

          EnumOperacao operacao = entry.State switch
          {
            EntityState.Added => EnumOperacao.Incluir,
            EntityState.Modified => EnumOperacao.Alterar,
            EntityState.Deleted => EnumOperacao.Excluir,
            _ => 0
          };

          if (oldValues.Count > 0 || newValues.Count > 0)
          {
            changeLogs.Add(new SistemaAuditoriaModel
            {
              Classe = entityName,
              ClasseId = (operacao == EnumOperacao.Incluir) ? 0 : Convert.ToInt32(entityId),
              ValorAnterior = System.Text.Json.JsonSerializer.Serialize(oldValues),
              ValorNovo = System.Text.Json.JsonSerializer.Serialize(newValues),
              DataAlteracao = DateTime.UtcNow,
              Operacao = operacao,
              UsuarioId = Operador.Id
            });
          }
        }
      }
    }
    

    return changeLogs;
  }

  private (Dictionary<string, object> oldValues, Dictionary<string, object> newValues) GetEntityChanges(EntityEntry entry)
  {
    var oldValues = new Dictionary<string, object>();
    var newValues = new Dictionary<string, object>();

    if (entry.State == EntityState.Added)
    {
      foreach (var prop in entry.CurrentValues.Properties)
      {
        newValues[prop.Name] = entry.CurrentValues[prop];
      }
    }
    else if (entry.State == EntityState.Deleted)
    {
      foreach (var prop in entry.OriginalValues.Properties)
      {
        oldValues[prop.Name] = entry.OriginalValues[prop];
      }
    }
    else if (entry.State == EntityState.Modified)
    {
      foreach (var prop in entry.OriginalValues.Properties)
      {
        var originalValue = entry.OriginalValues[prop];
        var currentValue = entry.CurrentValues[prop];

        if (!Equals(originalValue, currentValue))
        {
          oldValues[prop.Name] = originalValue;
          newValues[prop.Name] = currentValue;
        }
      }
    }

    return (oldValues, newValues);
  }
}

public static class ModelBuilderExtensions
{
  public static void Seed(this ModelBuilder modelBuilder)
  {
    EmpresaModel SeedEmpresa = new()
    {
      Id = 1,
      Nome = "M&A Suporte",
      Ativo = true,
      AdministradoraGlobal = true,
      CpfCnpj = "01033831000122"
    };

    SistemaGrupoModel SeedGrupo = new()
    {
      Id = 1,
      Nome = "Grupo Admin",
      UsoInterno = true,
      AdminSistema = true,
      Ativo = true,
      GrupoDeMenu = false
    };

    SistemaGrupoUsuarioModel SeedGrupoUsuario = new()
    {
      Id = 1,
      SistemaUsuarioId = 1,
      SistemaGrupoUsuarioId = 1
    };

    SistemaUsuarioModel SeedUsuario = new()
    {
      Id = 1,
      Nome = "Administrador",
      EMail = "gerencia@masuporte.com.br",
      Senha = "2K4a9C0ow5Ld5ObzJI60gomyKOd0IlLsRPMlBh3s7UIZ7TQVER0d8YK3wc9YXRWzMIpMX3sVX1YC3ant7cm/7w==",
      Salt = "AYmL/ZbezPlCZaWWhSJBpA==",
      EmpresaId = 1,
      Ativo = true,
      Admin = true
    };

    List<SistemaMenuModel> SeedMenu = new() {
      //Raiz do Menu
      new () { Id = 1, MenuPaiId = null, Nome = "Menu Sistema", Rota = ".", Icone = ".", Divisor = false, Ordem = 0, Ativo = true},

      //Menus principais
      new () { Id = 2, MenuPaiId = 1, Nome = "Home", Rota = "/", Icone = "mdi-home-outline", Divisor = false, Ordem = 1, Ativo = true},
      new () { Id = 3, MenuPaiId = 1, Nome = "Cadastro", Rota = "/#", Icone = "mdi-archive-outline", Divisor = false, Ordem = 2, Ativo = true},
      new () { Id = 4, MenuPaiId = 1, Nome = "Sistema", Rota = "/#", Icone = "mdi-cog-outline", Divisor = false, Ordem = 3, Ativo = true},

      //Submenus Cadastro      
      new () { Id = 5, MenuPaiId = 3, Nome = "Empresas", Rota = "/Cadastro/Empresa", Icone = "mdi-chevron-right", Divisor = false, Ordem = 1, Ativo = true },  

      //Submenus Sistema      
      new () { Id = 6, MenuPaiId = 4, Nome = "Usuários", Rota = "/Sistema/Usuario", Icone = "mdi-chevron-right", Divisor = false, Ordem = 1, Ativo = true },
      new () { Id = 7, MenuPaiId = 4, Nome = "Grupos", Rota = "/Sistema/Grupo", Icone = "mdi-chevron-right", Divisor = false, Ordem = 2, Ativo = true },
      new () { Id = 8, MenuPaiId = 4, Nome = "Permissões", Rota = "/Sistema/Permissao", Icone = "mdi-chevron-right", Divisor = false, Ordem = 3, Ativo = true},
      new () { Id = 9, MenuPaiId = 4, Nome = "Menu", Rota = "/Sistema/Menu", Icone = "mdi-chevron-right", Divisor = false, Ordem = 4, Ativo = true },
      new () { Id =10, MenuPaiId = 4, Nome = "Parâmetros", Rota = "/Sistema/Parametro", Icone = "mdi-chevron-right", Divisor = false, Ordem = 5, Ativo = true },

    };

    modelBuilder.Entity<EmpresaModel>().HasData(
      SeedEmpresa
    );

    modelBuilder.Entity<SistemaGrupoModel>().HasData(
      SeedGrupo
    );

    modelBuilder.Entity<SistemaGrupoUsuarioModel>().HasData(
      SeedGrupoUsuario
    );

    modelBuilder.Entity<SistemaUsuarioModel>().HasData(
      SeedUsuario
    );

    modelBuilder.Entity<SistemaMenuModel>().HasData(
      SeedMenu
    );
  }
}
