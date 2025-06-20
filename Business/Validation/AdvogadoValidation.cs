using MaSistemas.Model;
using MaSistemas.ViewModel;

namespace MaSistemas.Business
{
  class AdvogadoValidator 
  {

    private MaSistemasContext _context;

    public AdvogadoValidator()
    {
      _context = new MaSistemasContext();
    }

    public void ValidaAlteracao(AdvogadoViewModel entity)
    {
      if (string.IsNullOrEmpty(entity.Nome))
      {
        throw new System.InvalidOperationException("Nome do Advogado não pode estar em branco.");
      }

      IQueryable<AdvogadoModel> validainc = _context.AdvogadosModel.Where(x => x.Nome.Trim() == entity.Nome.Trim() && x.Id != entity.Id);
      if (validainc.Any())
      {
        throw new System.InvalidOperationException("Já existe Advogado com este nome.");
      }
    }

    public void ValidaExclusao(AdvogadoViewModel entity)
    {
            
    }

    public void ValidaInclusao(AdvogadoViewModel entity)
    {
      if (string.IsNullOrEmpty(entity.Nome))
      {
        throw new System.InvalidOperationException("Nome do Advogado não pode estar em branco.");
      }

      IQueryable<AdvogadoModel> validainc = _context.AdvogadosModel.Where(x => x.Nome.Trim() == entity.Nome.Trim());
      if (validainc.Any())
      {
        throw new System.InvalidOperationException("Já existe Advogado com este nome.");
      }
    }
  }
}