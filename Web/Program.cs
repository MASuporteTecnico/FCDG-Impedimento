using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
var vueServer = "http://localhost:9083";

int TempoDeExpiracaoDeSessao = 20;

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


// Configurar CORS
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

app.UseCors("AllowVueDevServer");
app.UseSession();
app.UseAuthentication();
app.MapControllers();
app.UseStaticFiles();
app.UseSpaStaticFiles();
app.UseMvc();
app.UseRouting();
app.UseAuthorization();

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
