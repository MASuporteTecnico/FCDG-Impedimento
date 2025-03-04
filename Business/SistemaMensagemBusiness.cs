using System.Linq.Expressions;
using MaSistemas.Model;
using MaSistemas.ViewModel;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;

namespace MaSistemas.Business
{
  public class SistemaMensagemBusiness : BaseBusiness<SistemaMensagemViewModel, SistemaMensagemModel, PaginacaoViewModel>
  {
    private readonly MaSistemasContext _context;

    public SistemaMensagemBusiness()
    {
      _context = new MaSistemasContext();
    }

    public override void Delete(SistemaUsuarioViewModel usuario, SistemaMensagemViewModel entity)
    {
      
      SistemaMensagemCaixaModel Caixa = _context.SistemaMensagensCaixaModel
                                      .Where(x => x.MensagemId == entity.Id && x.UsuarioId == usuario.Id)
                                      .FirstOrDefault();

      if(Caixa != null)
      {
        _context.SistemaMensagensCaixaModel.Remove(Caixa);
        _context.SaveChanges();
      }

      //Se não tiver mais nenhum destinatário da mensagem, deleta a mensagem
      int total = _context.SistemaMensagensCaixaModel
                  .Where(x => x.MensagemId == entity.Id)
                  .Count();
      
      if(total == 0)
      {
        SistemaMensagemModel model = _context.SistemaMensagensModel
                                    .Include(x => x.Para)
                                    .Where(x => x.Id == entity.Id)
                                    .FirstOrDefault();

        if(model != null)
        {
          _context.SistemaMensagensModel.Remove(model);
          _context.SaveChanges();
        }
      }
    }

    public override void Dispose()
    {
      _context.Dispose();
    }

    public override SistemaMensagemViewModel EntityToView(SistemaMensagemModel entity)
    {
      if (entity == null)
      {
        return new SistemaMensagemViewModel();
      }

      SistemaMensagemViewModel view = new();
      view.InjectFrom(entity);
      view.De = (SistemaUsuarioViewModel)(new SistemaUsuarioViewModel()).InjectFrom(entity.De ?? new SistemaUsuarioModel());
      view.Para = SistemaMensagemParaModelToView(entity.Para);
      view.Caixa = SistemaMensagemCaixaModelToView(entity.Caixa);

      return view;
    }

    public override List<SistemaMensagemViewModel> Index(ref PaginacaoViewModel paginacao)
    {
      throw new NotImplementedException();
    }

    public List<SistemaMensagemViewModel> Mensagens(SistemaUsuarioViewModel usuario)
    {

      List<SistemaMensagemViewModel> view = [];

      List<SistemaMensagemModel> model = [.. _context.SistemaMensagensModel
                                              .Include(x => x.De)
                                              .Include(x => x.Caixa).ThenInclude(x => x.Usuario)
                                              .Where(x => x.Caixa.Any(y => y.UsuarioId == usuario.Id))];

      view = [..
              (
                from u in model.ToList()
                select (SistemaMensagemViewModel)(new SistemaMensagemViewModel() {
                  De = (SistemaUsuarioViewModel)(new SistemaUsuarioViewModel() {
                    Id = u.De.Id,
                    Nome = u.De.Nome
                  }),
                  Caixa = SistemaMensagemCaixaModelToView(u.Caixa.Where(y => y.UsuarioId == usuario.Id).ToList())
                }).InjectFrom(u)
              )
            ];

      return [.. view];
    }

    private List<SistemaMensagemCaixaViewModel> SistemaMensagemCaixaModelToView(ICollection<SistemaMensagemCaixaModel> caixa)
    {
      if (caixa == null)
      {
        return [];
      }

      return [..
              (
                from u in caixa
                select (SistemaMensagemCaixaViewModel)(new SistemaMensagemCaixaViewModel() {
                  Usuario = (new SistemaUsuarioViewModel() {
                    Id = u.Usuario.Id,
                    Nome = u.Usuario.Nome
                  })
                }).InjectFrom(u)
              )
            ];
    }

    public List<SistemaMensagemParaViewModel> SistemaMensagemParaModelToView(ICollection<SistemaMensagemParaModel> model)
    {
      if (model == null)
      {
        return [];
      }

      return [..
              (
                from u in model
                select (SistemaMensagemParaViewModel)(new SistemaMensagemParaViewModel() {
                  Usuario = (new SistemaUsuarioViewModel() {
                    Id = u.Usuario.Id,
                    Nome = u.Usuario.Nome
                  })
                }).InjectFrom(u)
              )
            ];
    }

    public override void Save(SistemaUsuarioViewModel colaborador, SistemaMensagemViewModel entity)
    {
      //Remetnete é o usuário do sistema
      entity.De = colaborador;
      
      SistemaMensagemModel model;
      SistemaMensagemValidator validator = new();

      if (entity.Id == 0)
      {
        validator.ValidaInclusao(entity);
        model = ViewToEntity(entity, EnumOperacao.Alterar);
        _context.SistemaMensagensModel.Add(model);
      }
      else
      {
        validator.ValidaAlteracao(entity);
        model = ViewToEntity(entity, EnumOperacao.Alterar);
        _context.SistemaMensagensModel.Attach(model);
      }
      
      _context.SaveChanges();
    }

    public void SetarLidaNaoLida(SistemaUsuarioViewModel usuario, SistemaMensagemViewModel entity)
    {
      SistemaMensagemCaixaModel Caixa = _context.SistemaMensagensCaixaModel
                                      .Where(x => x.MensagemId == entity.Id && x.UsuarioId == usuario.Id)
                                      .FirstOrDefault();

      if (Caixa != null)
      {
        Caixa.Lida = !Caixa.Lida;
        _context.SistemaMensagensCaixaModel.Attach(Caixa);
        _context.SaveChanges();
      }
    }

    public override SistemaMensagemViewModel SelectOne(Expression<Func<SistemaMensagemModel, bool>> pCondicao)
    {

      SistemaMensagemModel model = _context.SistemaMensagensModel
                              .Include(x => x.De)
                              .Include(x => x.Para).ThenInclude(x => x.Usuario)
                              .Include(x => x.Caixa).ThenInclude(x => x.Usuario)
                              .Where(pCondicao)
                              .FirstOrDefault();

      SistemaMensagemViewModel view = EntityToView(model);
      return view;
    }

    public override SistemaMensagemModel ViewToEntity(SistemaMensagemViewModel view, EnumOperacao operacao)
    {
      if (view == null)
      {
        return null;
      }

      SistemaMensagemModel model = _context.SistemaMensagensModel
                                  .Include(x => x.De)
                                  .Include(x => x.Para).ThenInclude(x => x.Usuario)
                                  .Include(x => x.Caixa).ThenInclude(x => x.Usuario)                                  
                                  .Where(x => x.Id == view.Id)
                                  .FirstOrDefault() ?? new SistemaMensagemModel();


      model.InjectFrom(view);

      if (operacao == EnumOperacao.Excluir)
        return model;

      model.De = _context.SistemaUsuariosModel.Where(x => x.Id == view.De.Id).FirstOrDefault();
      model.Para = SistemaMensagemParaViewToModel(view.Para);
      model.Caixa = SistemaMensagemCaixaViewToModel(view.Caixa);
      

      return model;
    }

    private ICollection<SistemaMensagemParaModel> SistemaMensagemParaViewToModel(ICollection<SistemaMensagemParaViewModel> para)
    {
      if (para == null)
      {
        return null;
      }

      return [..
              (
                from u in para
                select (SistemaMensagemParaModel)(new SistemaMensagemParaModel() {
                  Usuario = _context.SistemaUsuariosModel.Where(x => x.Id == u.Usuario.Id).FirstOrDefault()
                }).InjectFrom(u)
              )
            ];
    }

    private ICollection<SistemaMensagemCaixaModel> SistemaMensagemCaixaViewToModel(ICollection<SistemaMensagemCaixaViewModel> Caixa)
    {
      if (Caixa == null)
      {
        return null;
      }

      return [..
              (
                from u in Caixa
                select (SistemaMensagemCaixaModel)(new SistemaMensagemCaixaModel() {
                  Usuario = _context.SistemaUsuariosModel.Where(x => x.Id == u.Usuario.Id).FirstOrDefault()
                }).InjectFrom(u)
              )
            ];
    }
  }
}