using System.Linq.Expressions;

namespace MaSistemas.Business
{
  public class OrderByExpression<E> where E : class
  {
    public Expression<Func<E, dynamic>> _expression;
    public bool _descending;

    public OrderByExpression(Expression<Func<E, dynamic>> expression, bool descending = false)
    {
      _expression = expression;
      _descending = descending;
    }    
  }

  public class MontaOrderBylist<E> where E : class
  {
    public List<OrderByExpression<E>> Montar(dynamic paginacao)
    {
      var orderBylist = new List<OrderByExpression<E>>();
      if (paginacao != null && paginacao.sortBy != null)
      {

        if(paginacao.sortBy.Count == 1)
        {

          ParameterExpression param = Expression.Parameter(typeof(E), "x");
          Expression property = Expression.PropertyOrField(param, paginacao.sortBy[0].key);
          var convertedBody = Expression.MakeUnary(ExpressionType.Convert, property, typeof(object));
          var expression = Expression.Lambda<Func<E, dynamic>>(convertedBody, new[] { param });
          orderBylist.Add(new OrderByExpression<E>(expression, paginacao.sortBy[0].order == "desc" ? true : false));

        }else if(paginacao.sortBy.Count == 2){

          ParameterExpression param = Expression.Parameter(typeof(E), "x");
          Expression property = Expression.PropertyOrField(param, paginacao.sortBy[0].key);
          Expression property2 = Expression.PropertyOrField(property, paginacao.sortBy[1].key);

          var convertedBody = Expression.MakeUnary(ExpressionType.Convert, property2, typeof(object));
          var expression = Expression.Lambda<Func<E, dynamic>>(convertedBody, new[] { param });
          orderBylist.Add(new OrderByExpression<E>(expression, paginacao.sortBy[0].order == "desc" ? true : false));
        }
      }
      else
      {
        return null;
      }

      return orderBylist;
    }
  }

  public class AplicaOrderBy<E> where E : class
  {
    public IQueryable<E> Ordenar(IQueryable<E> query, OrderByExpression<E>[] orderBy = null)
    {
      if (orderBy == null)
        return query;

      IOrderedQueryable<E> output = null;

      foreach (var orderByExpression in orderBy)
      {
        if (output == null)
          output = ApplyOrderBy(query, orderByExpression._expression, orderByExpression._descending);
        else
          output = ApplyThenBy(output, orderByExpression._expression, orderByExpression._descending);
      }

      return output;
    }

    private IOrderedQueryable<E> ApplyOrderBy(IQueryable<E> query, Expression<Func<E, dynamic>> orderBy, bool _descending)
    {
      if (_descending)
        return query.OrderByDescending(orderBy);
      else
        return query.OrderBy(orderBy);
    }

    private IOrderedQueryable<E> ApplyThenBy(IOrderedQueryable<E> query, Expression<Func<E, dynamic>> orderBy, bool _descending)
    {
      if (_descending)
        return query.ThenByDescending(orderBy);
      else
        return query.ThenBy(orderBy);
    }
  }
}