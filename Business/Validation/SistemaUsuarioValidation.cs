using MaSistemas.Model;
using MaSistemas.ViewModel;

namespace MaSistemas.Business
{
  class SistemaUsuarioValidator
  {

    private MaSistemasContext _context;

    public SistemaUsuarioValidator()
    {
      _context = new MaSistemasContext();
    }

    public void ValidaAlteracao(SistemaUsuarioViewModel entity)
    {
      ValidaComum(entity);

      IQueryable<SistemaUsuarioModel> validainc = _context.SistemaUsuariosModel.Where(x => x.EMail.Trim() == entity.EMail.Trim() && x.Id != entity.Id);
      if (validainc.Any())
      {
        throw new InvalidOperationException("Já existe Usuário com este E-Mail.");
      }
    }

    public void ValidaExclusao(SistemaUsuarioViewModel entity)
    {
      if (entity.Id == 1)
        throw new InvalidOperationException("Operação não permitida!");

      //Verificar Grupo
      List<SistemaGrupoUsuarioModel> TemGrupo = [.. _context.SistemaGrupoUsuariosModel.Where(x => x.SistemaUsuarioId == entity.Id) ];            
      if(TemGrupo.Count != 0)
      {
        throw new InvalidOperationException("Usuario é membro de algum grupo.");
      }

      //Verifica permissao
      List<SistemaPermissaoModel> TemPermissao = [.. _context.SistemaPermissoesModel.Where(x => x.SistemaUsuarioId == entity.Id)];
      if (TemPermissao.Count != 0)
      {
        throw new InvalidOperationException("Usuario possui alguma permissão de acesso cadastrada.");
      }
    }

    public void ValidaInclusao(SistemaUsuarioViewModel entity)
    {
      ValidaComum(entity);

      IQueryable<SistemaUsuarioModel> validainc = _context.SistemaUsuariosModel.Where(x => x.EMail.Trim() == entity.EMail.Trim());
      if (validainc.Any())
      {
        throw new InvalidOperationException("Já existe Usuario com este E-Mail.");
      }
    }

    public void ValidaComum(SistemaUsuarioViewModel entity)
    {
      if (string.IsNullOrEmpty(entity.Nome))
      {
        throw new InvalidOperationException("Nome do Usuário não pode estar em branco.");
      }

      if (string.IsNullOrEmpty(entity.EMail))
      {
        throw new InvalidOperationException("E-Mail do Usuário não pode estar em branco.");
      }
    }
  }
}