using MaSistemas.Business;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
var vueServer = "http://localhost:9083";

int TempoDeExpiracaoDeSessao = 20;

builder.Services.AddDataProtection()
            .SetApplicationName("MaSistemas")
            .AddKeyManagementOptions(options =>
            {
              options.NewKeyLifetime = new TimeSpan(180, 0, 0, 0);
              options.AutoGenerateKeys = true;
            });

//Ajuste de Limite de tamanho para envio de arquivos
builder.Services.Configure<KestrelServerOptions>(option =>
{
  option.Limits.MaxRequestBodySize = int.MaxValue;
});

//Ajuste de Limite de tamanho para envio de arquivos
builder.Services.Configure<FormOptions>(option =>
{
  option.ValueLengthLimit = int.MaxValue;
  option.MultipartBodyLengthLimit = int.MaxValue;
  option.MultipartHeadersLengthLimit = int.MaxValue;
});


//Configurar CORS
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowVueDevServer", policy =>
  {
    policy.WithOrigins(vueServer)
            .AllowAnyHeader()
            .AllowAnyMethod();
  });
});


builder.Services.AddSpaStaticFiles(configuration: options => { options.RootPath = "wwwroot"; });

// Adicione serviços ao container.
//builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddControllers()
.AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.PropertyNamingPolicy = null;
      options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault | System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Configurar o cache distribuído na memória (necessário para usar sessões)
builder.Services.AddDistributedMemoryCache();

//Para controle de permissão de acesso nas controllers
builder.Services.AddAuthorization();

// configuração da session
builder.Services.AddSession(options =>
{
  options.IdleTimeout = TimeSpan.FromMinutes(TempoDeExpiracaoDeSessao); // Tempo de expiração da sessão
  options.Cookie.HttpOnly = true; // Definir o cookie como HTTP only
  options.Cookie.IsEssential = true; // Tornar o cookie essencial para usuários do GDPR
  options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
  options.Cookie.SameSite = SameSiteMode.Strict;

});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(options =>
  {
    options.Events.OnRedirectToLogin = (context) =>
    {
      context.Response.StatusCode = 401;
      return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = (context) =>
    {
      context.Response.StatusCode = 403;
      return Task.CompletedTask;
    };

    options.ExpireTimeSpan = TimeSpan.FromMinutes(TempoDeExpiracaoDeSessao);
    options.SlidingExpiration = true;
    // Teste de expiração de sessão
    options.Cookie.MaxAge = options.ExpireTimeSpan; //TimeSpan.FromMinutes(TempoDeExpiracaoDeSessao);

  });

builder.Services.AddMvc(option => option.EnableEndpointRouting = false);

var app = builder.Build();


//Inicializa a conexçao com o banco de dados.
//Caso não exista, cria e faz o Seed
DatabaseInitializer.InitializeDatabase();

app.UseCors("AllowVueDevServer");
app.UseSession();
app.UseAuthentication();
app.UseRouting();
app.MapControllers();
app.UseStaticFiles();
app.UseSpaStaticFiles();
app.UseMvc();
app.UseAuthorization();

// //Nova organziação usando o Gemini
// app.UseStaticFiles(); // 1. Serve arquivos estáticos (CSS, JavaScript, imagens)
// app.UseSpaStaticFiles(); // 2. Serve arquivos estáticos específicos para Single Page Applications (SPAs)
// app.UseRouting(); // 3. Define o roteamento da aplicação
// app.UseCors("AllowVueDevServer"); // 4. Habilita o CORS para o servidor de desenvolvimento do Vue.js
// app.UseSession(); // 5. Habilita o uso de sessões
// app.UseAuthentication(); // 6. Autentica o usuário
// app.UseAuthorization(); // 7. Autoriza o acesso a recursos
// app.MapControllers(); // 8. Mapeia os controladores da API
// //app.UseMvc(); // 9. (Obsoleto em .NET 6+) Mapeia rotas MVC (se você ainda estiver usando MVC)


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSpa(config => config.UseProxyToSpaDevelopmentServer(vueServer));
}
else
{
  app.MapFallbackToFile("index.html");
}

app.Run();
