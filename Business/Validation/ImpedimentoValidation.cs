using MaSistemas.Model;
using MaSistemas.ViewModel;

namespace MaSistemas.Business
{
  class ImpedimentoValidator 
  {

    private MaSistemasContext _context;

    public ImpedimentoValidator()
    {
      _context = new MaSistemasContext();
    }

    public void ValidaAlteracao(ImpedimentoViewModel entity)
    {
      // if (string.IsNullOrEmpty(entity.Nome))
      // {
      //   throw new System.InvalidOperationException("Nome do Impedimento não pode estar em branco.");
      // }

      // IQueryable<ImpedimentoModel> validainc = _context.ImpedimentosModel.Where(x => x.Nome.Trim() == entity.Nome.Trim() && x.Id != entity.Id);
      // if (validainc.Any())
      // {
      //   throw new System.InvalidOperationException("Já existe Impedimento com este nome.");
      // }
    }

    public void ValidaExclusao(ImpedimentoViewModel entity)
    {
            
    }

    public void ValidaInclusao(ImpedimentoViewModel entity)
    {
      // if (string.IsNullOrEmpty(entity.Nome))
      // {
      //   throw new System.InvalidOperationException("Nome do Impedimento não pode estar em branco.");
      // }

      // IQueryable<ImpedimentoModel> validainc = _context.ImpedimentosModel.Where(x => x.Nome.Trim() == entity.Nome.Trim());
      // if (validainc.Any())
      // {
      //   throw new System.InvalidOperationException("Já existe Impedimento com este nome.");
      // }
    }
  }
}