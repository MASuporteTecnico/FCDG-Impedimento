using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using MaSistemas.Model;
using MaSistemas.ViewModel;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;

namespace MaSistemas.Business
{
  public class ImpedimentoBusiness : BaseBusiness<ImpedimentoViewModel, ImpedimentoModel, PaginacaoViewModel>
  {
    private readonly MaSistemasContext _context = new();

    public ImpedimentoBusiness()
    {
      _context = new MaSistemasContext();
    }

    public override void Dispose()
    {
      _context.Dispose();
    }

    public override List<ImpedimentoViewModel> Index(ref PaginacaoViewModel paginacao)
    {

      //Ajuste de Filtro e Ordenação
      MontaOrderBylist<ImpedimentoModel> odbList = new();
      AplicaOrderBy<ImpedimentoModel> appOdb = new();
      OrderByExpression<ImpedimentoModel>[] oderByExp = odbList.Montar(paginacao).ToArray();

      JsonNode jsonFiltro = JsonSerializer.Deserialize<JsonNode>(paginacao.Filtro.GetRawText());
      string Nome = jsonFiltro["Busca"].GetValue<String>() ?? "";
      bool flInativos = jsonFiltro["Inativos"].GetValue<bool?>() == null || jsonFiltro["Inativos"].GetValue<bool>();


      //Expression<Func<ImpedimentoModel, bool>> flATivo = a => a.Ativo;
      // Expression<Func<ImpedimentoModel, bool>> filtroNome = a => a.Nome.Contains(Nome);
      // Fim do Ajuste de  Filtro e Ordenação

      List<ImpedimentoViewModel> view = new();

      IQueryable<ImpedimentoModel> model = _context.ImpedimentosModel;

      // if (Nome != "")
      //   model = model.Where(filtroNome);

      if (flInativos == false)
      {
        //model = model.Where(flATivo);
      }

      if (oderByExp.Length > 0)
        model = appOdb.Ordenar(model, oderByExp);

      // view = (from u in model
      //         select (ImpedimentoViewModel)new ImpedimentoViewModel().InjectFrom(u)
      //         ).ToList();

      view = (from u in model
              select new ImpedimentoViewModel
              {
                Id = u.Id,
                DataImpedimento = u.DataImpedimento,
                AdvogadoId = u.AdvogadoId,
                Objeto = u.Objeto,
                ParteA = u.ParteA,
                ParteB = u.ParteB
                // ==> Aqui vai entrar o Nome do Advogado Responsável
                // ==> Aqui vai entrar as Respostas dos Advogados
              }).ToList();


      paginacao.itemsLength = view.Count;
      paginacao.pageCount = Convert.ToInt32(Math.Ceiling((Decimal)paginacao.itemsLength / paginacao.itemsPerPage));
      paginacao.page = (paginacao.page > paginacao.pageCount) ? 1 : paginacao.page;

      return view.Skip((paginacao.page - 1) * paginacao.itemsPerPage).Take(paginacao.itemsPerPage).ToList();

    }

    public override void Save(SistemaUsuarioViewModel colaborador, ImpedimentoViewModel view)
    {
      ImpedimentoModel model;
      ImpedimentoValidator validador = new();

      //Para Auditoria
      _context.Operador = (SistemaUsuarioModel)(new SistemaUsuarioModel()).InjectFrom(colaborador);

      if (view.Id == 0)
      {
        validador.ValidaInclusao(view);
        model = ViewToEntity(view, EnumOperacao.Incluir);
        _context.ImpedimentosModel.Add(model);
      }
      else
      {
        validador.ValidaAlteracao(view);
        model = ViewToEntity(view, EnumOperacao.Alterar);
        _context.ImpedimentosModel.Attach(model);
      }

      _context.SaveChanges();
    }

    public override void Delete(SistemaUsuarioViewModel usuario, ImpedimentoViewModel view)
    {
      ImpedimentoValidator validador = new();
      validador.ValidaExclusao(view);
      ImpedimentoModel model = ViewToEntity(view, EnumOperacao.Excluir);

      //Para Auditoria
      _context.Operador = (SistemaUsuarioModel)(new SistemaUsuarioModel()).InjectFrom(usuario);

      _context.ImpedimentosModel.Remove(model);
      _context.SaveChanges();
    }
    public override ImpedimentoViewModel SelectOne(Expression<Func<ImpedimentoModel, bool>> pCondicao)
    {
      ImpedimentoModel model = _context.ImpedimentosModel
                            .Where(pCondicao)
                            .FirstOrDefault();

      ImpedimentoViewModel view = EntityToView(model);
      return view;
    }

    public override ImpedimentoModel ViewToEntity(ImpedimentoViewModel view, EnumOperacao operacao)
    {
      if (view == null)
      {
        return null;
      }

      ImpedimentoModel model = _context.ImpedimentosModel.Where(x => x.Id == view.Id).FirstOrDefault() ?? new ImpedimentoModel();
      model.InjectFrom(view);
      return model;

    }

    public override ImpedimentoViewModel EntityToView(ImpedimentoModel entity)
    {
      if (entity == null)
      {
        return new ImpedimentoViewModel();
      }

      ImpedimentoViewModel view = (ImpedimentoViewModel)(new ImpedimentoViewModel()).InjectFrom(entity);
      view.AdvogadoResponsavel = (AdvogadoViewModel)new AdvogadoViewModel().InjectFrom(view.AdvogadoId);
      return view;
    }

  }
}