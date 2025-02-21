using MaSistemas.Model;
using MaSistemas.ViewModel;
using Mono.Unix;
using Mono.Unix.Native;

namespace MaSistemas.Business
{
  class SistemaParametroValidator
  {

    private MaSistemasContext _context;

    public SistemaParametroValidator()
    {
      _context = new MaSistemasContext();
    }

    public void ValidaAlteracao(SistemaParametroViewModel entity)
    {
      Directory.CreateDirectory(entity.PastaArquivos);
      var unixPastaArquivos = new UnixDirectoryInfo(entity.PastaArquivos);
      unixPastaArquivos.FileAccessPermissions =  FileAccessPermissions.AllPermissions;
      //unixPastaArquivos.SetOwner("www-data");
      unixPastaArquivos.Refresh();

      Directory.CreateDirectory(entity.PastaTemporarios);
      var unixPastaTemporarios = new UnixDirectoryInfo(entity.PastaTemporarios);
      unixPastaTemporarios.FileAccessPermissions = FileAccessPermissions.AllPermissions;
      //unixPastaTemporarios.SetOwner();
      unixPastaTemporarios.Refresh();
    }

    public void ValidaExclusao(SistemaParametroViewModel entity)
    {


    }

    public void ValidaInclusao(SistemaParametroViewModel entity)
    {
      Directory.CreateDirectory(entity.PastaArquivos);
      var unixPastaArquivos = new UnixDirectoryInfo(entity.PastaArquivos);
      unixPastaArquivos.FileAccessPermissions = FileAccessPermissions.UserReadWriteExecute | FileAccessPermissions.GroupReadWriteExecute | FileAccessPermissions.OtherReadWriteExecute;
      //unixPastaArquivos.SetOwner("www-data");
      unixPastaArquivos.Refresh();

      Directory.CreateDirectory(entity.PastaTemporarios);
      var unixPastaTemporarios = new UnixDirectoryInfo(entity.PastaTemporarios);
      unixPastaTemporarios.FileAccessPermissions = FileAccessPermissions.UserReadWriteExecute | FileAccessPermissions.GroupReadWriteExecute | FileAccessPermissions.OtherReadWriteExecute;
      //unixPastaTemporarios.SetOwner();
      unixPastaTemporarios.Refresh();
    }

  }
}