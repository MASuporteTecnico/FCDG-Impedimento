using MaSistemas.Model;
using MaSistemas.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace MaSistemas.Business
{
  class SistemaMensagemValidator
  {

    private MaSistemasContext _context;

    public SistemaMensagemValidator()
    {
      _context = new MaSistemasContext();
    }

    public void ValidaAlteracao(SistemaMensagemViewModel entity)
    {
      ValidaComum(entity);
    }

    public void ValidaExclusao(SistemaMensagemViewModel entity)
    {

    }

    public void ValidaInclusao(SistemaMensagemViewModel entity)
    {
      ValidaComum(entity);

      if(entity.Para.Count == 0)
      {
        throw new System.InvalidOperationException("Nenhum destinatário (Para) informado.");
      }

      if (entity.Caixa.Count == 0)
      {
        throw new System.InvalidOperationException("Nenhum destinatário (Caixa) informado.");
      }
    }

    public void ValidaComum(SistemaMensagemViewModel entity)
    {
      if(entity.De == null)
      {
        throw new System.InvalidOperationException("Remetente não informado.");
      }

      if(string.IsNullOrEmpty(entity.Titulo))
      {
        throw new System.InvalidOperationException("Título não informado.");
      }
      
      if(string.IsNullOrEmpty(entity.Texto))
      {
        throw new System.InvalidOperationException("Conteúdo da mensagem não informado.");
      }
      
    }
  }
}