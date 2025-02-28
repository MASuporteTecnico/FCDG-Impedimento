using System.Security.Cryptography.X509Certificates;
using MaSistemas.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace MaSistemas.Business
{
  public static class DatabaseInitializer
  {
    public static void InitializeDatabase()
    {
      IServiceCollection serviceCollection = new ServiceCollection();
      serviceCollection.AddDbContext<MaSistemasContext>(ServiceLifetime.Scoped);
      ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
      var context = serviceProvider.GetRequiredService<MaSistemasContext>();

      // Aplica migrations e executa o seed automaticamente
      context.Database.EnsureCreated();

    }
  }
}