using MaSistemas.Model;
using MaSistemas.ViewModel;

namespace MaSistemas.Business
{
  class ImpedimentoVerificacaoValidator 
  {

    private MaSistemasContext _context;

    public ImpedimentoVerificacaoValidator()
    {
      _context = new MaSistemasContext();
    }

    public void ValidaAlteracao(ImpedimentoVerificacaoViewModel entity)
    {
      // if (string.IsNullOrEmpty(entity.Nome))
      // {
      //   throw new System.InvalidOperationException("Nome do ImpedimentoVerificacao não pode estar em branco.");
      // }

      // IQueryable<ImpedimentoVerificacaoModel> validainc = _context.ImpedimentoVerificacaoModel.Where(x => x.Nome.Trim() == entity.Nome.Trim() && x.Id != entity.Id);
      // if (validainc.Any())
      // {
      //   throw new System.InvalidOperationException("Já existe ImpedimentoVerificacao com este nome.");
      // }
    }

    public void ValidaExclusao(ImpedimentoVerificacaoViewModel entity)
    {
            
    }

    public void ValidaInclusao(ImpedimentoVerificacaoViewModel entity)
    {
      // if (string.IsNullOrEmpty(entity.Nome))
      // {
      //   throw new System.InvalidOperationException("Nome do ImpedimentoVerificacao não pode estar em branco.");
      // }

      // IQueryable<ImpedimentoVerificacaoModel> validainc = _context.ImpedimentoVerificacaoModel.Where(x => x.Nome.Trim() == entity.Nome.Trim());
      // if (validainc.Any())
      // {
      //   throw new System.InvalidOperationException("Já existe ImpedimentoVerificacao com este nome.");
      // }
    }
  }
}