using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace MaSistemas.Model;

public class MaSistemasContext : DbContext
{
  public MaSistemasContext(DbContextOptions<MaSistemasContext> options)
      : base(options)
  {
  }

  public MaSistemasContext()
  {

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

      default:
        break;
    }
  }

  public DbSet<SistemaMenuModel> SistemaMenusModel { get; set; } = default!;
  public DbSet<SistemaPermissaoModel> SistemaPermissoesModel { get; set; } = default!;
  public DbSet<SistemaUsuarioModel> SistemaUsuariosModel { get; set; } = default!;
  public DbSet<SistemaParametroModel> SistemaParametrosModel { get; set; } = default!;
  public DbSet<SistemaGrupoModel> SistemaGruposModel { get; set; } = default!;
  public DbSet<SistemaGrupoUsuarioModel> SistemaGrupoUsuariosModel { get; set; } = default!;
  public DbSet<SistemaGrupoMenuModel> SistemaGrupoMenusModel { get; set; } = default!;
  public DbSet<EmpresaModel> EmpresasModel { get; set; } = default!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    
    modelBuilder.Seed();
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
      Ativo = true
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
  }
}
