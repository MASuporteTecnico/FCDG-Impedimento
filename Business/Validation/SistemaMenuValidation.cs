using MaSistemas.Model;
using MaSistemas.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace MaSistemas.Business
{
  class SistemaMenuValidator
  {

    private MaSistemasContext _context;

    public SistemaMenuValidator()
    {
      _context = new MaSistemasContext();
    }

    public void ValidaAlteracao(SistemaMenuViewModel entity)
    {
      ValidaComum(entity);
    }

    public void ValidaExclusao(SistemaMenuViewModel entity)
    {
      if (entity.Id == 1)
        throw new System.InvalidOperationException("Operação não permitida!");

      //Verificar Grupo
      List<SistemaGrupoMenuModel> TemGrupo = [.. _context.SistemaGrupoMenusModel.Where(x => x.SistemaMenuId == entity.Id).AsNoTracking() ];            
      if(TemGrupo.Count != 0)
      {
        throw new System.InvalidOperationException("Menu é membro de algum grupo.");
      }

      //Verifica permissao
      List<SistemaPermissaoModel> TemPermissao = [.. _context.SistemaPermissoesModel.Where(x => x.SistemaMenuId == entity.Id).AsNoTracking()];
      if (TemPermissao.Count != 0)
      {
        throw new System.InvalidOperationException("Menu possui alguma permissão de acesso cadastrada.");
      }
    }

    public void ValidaInclusao(SistemaMenuViewModel entity)
    {
      ValidaComum(entity);
    }

    public void ValidaComum(SistemaMenuViewModel entity)
    {
      if (string.IsNullOrEmpty(entity.Nome))
      {
        throw new InvalidOperationException("Nome do Grupo não pode estar em branco.");
      }
    }
  }
}