using MaSistemas.Model;
using MaSistemas.ViewModel;
using Microsoft.AspNetCore.Http;

namespace MaSistemas.Business
{
  public class ArquivoBusiness
  {
    private readonly MaSistemasContext _context = new();

    public ArquivoBusiness()
    {
      _context = new MaSistemasContext();
    }

    public List<ArquivoViewModel> Upload(ArquivoViewModel entity)
    {

      SistemaParametroModel config = _context.SistemaParametrosModel.FirstOrDefault();
      List<ArquivoViewModel> lista = new();

      string pastaDoArquivo = config.PastaTemporarios;

      // Faço a exclusão dos arquivos com mais de 2 horas de criação
      Directory.GetFiles(config.PastaTemporarios, "*.*", SearchOption.AllDirectories)
               .Select(f => new FileInfo(f))
               .Where(f => f.CreationTime < DateTime.Now.AddHours(-2))
               .ToList()
               .ForEach(f => f.Delete());

      foreach (IFormFile file in entity.Arquivos)
      {
        int indiceArquivo = 1;
        //string novoNome = String.Join("", SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(Path.GetFileNameWithoutExtension(file.FileName)))) + Path.GetExtension(file.FileName);
        // Utilizando GUID
        string novoNome = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        string caminhoCompleto = Path.Combine(pastaDoArquivo, novoNome);
        while (File.Exists(caminhoCompleto))
        {
          novoNome = Path.GetFileNameWithoutExtension(novoNome) + " (" + indiceArquivo.ToString() + ")" + Path.GetExtension(novoNome);
          caminhoCompleto = Path.Combine(pastaDoArquivo, novoNome);
          ++indiceArquivo;
        }

        Directory.CreateDirectory(pastaDoArquivo);

        using (var fileStream = new FileStream(caminhoCompleto, FileMode.Create))
          file.CopyTo(fileStream);

        lista.Add(new ArquivoViewModel() { Id = 0, Nome = novoNome, Caminho = caminhoCompleto, ArquivoTemporario = true });

      }

      return lista;
    }
  }
}