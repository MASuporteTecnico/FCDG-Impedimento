using MaSistemas.Model;
using MaSistemas.ViewModel;

namespace MaSistemas.Business
{
  class EmpresaValidator 
  {

    private MaSistemasContext _context;

    public EmpresaValidator()
    {
      _context = new MaSistemasContext();
    }

    public void ValidaAlteracao(EmpresaViewModel entity)
    {
      if (string.IsNullOrEmpty(entity.Nome))
      {
        throw new System.InvalidOperationException("Nome do Empresa não pode estar em branco.");
      }

      IQueryable<EmpresaModel> validainc = _context.EmpresasModel.Where(x => x.Nome.Trim() == entity.Nome.Trim() && x.Id != entity.Id);
      if (validainc.Any())
      {
        throw new System.InvalidOperationException("Já existe Empresa com este nome.");
      }
    }

    public void ValidaExclusao(EmpresaViewModel entity)
    {
            
    }

    public void ValidaInclusao(EmpresaViewModel entity)
    {
      if (string.IsNullOrEmpty(entity.Nome))
      {
        throw new System.InvalidOperationException("Nome do Empresa não pode estar em branco.");
      }

      IQueryable<EmpresaModel> validainc = _context.EmpresasModel.Where(x => x.Nome.Trim() == entity.Nome.Trim());
      if (validainc.Any())
      {
        throw new System.InvalidOperationException("Já existe Empresa com este nome.");
      }
    }
  }
}