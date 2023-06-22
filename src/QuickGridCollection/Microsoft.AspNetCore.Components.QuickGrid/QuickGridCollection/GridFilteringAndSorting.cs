using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection
{
    /// <summary>
    /// La structure <see cref="GridFilteringAndSorting{TGridItem}"/> est un type générique qui prend un paramètre de type <c>TGridItem</c> est le même typeparam que <see cref="Grid{TGridItem}"/>.
    /// Elle est utilisée par un composant <see cref="Grid{TGridItem}"/> via la propriété <see cref="Grid{TGridItem}.FilterSortChanged"/>.
    /// Cette propriété est de type <see cref="EventCallback{GridFilteringAndSorting{TGridItem}}"/>,
    /// ce qui signifie qu'elle peut être utilisée pour déclencher un événement lorsque les filtres ou l'ordre de la grille changent.
    /// <para>
    /// L'utilisateur peut alors utiliser cette instance pour filtrer et trier les données de la grille avec <c>Linq</c>, <c>Entity Framework Linq</c> ou <c>Simple.Client.OData</c>.
    /// </para>
    /// <para>
    /// <example>
    /// Exemple d'utilisation dans un composant Blazor:
    /// <code>
    /// &lt;Grid TGridItem="Item" Items="Items" FilterOrOrderChange="OnFilterOrOrderChange"&gt;
    /// &lt;/Grid&gt;
    /// @code {
    ///     private async Task OnFilterOrOrderChange(GridFilteringAndSorting&lt;Item&gt; filtersAndOrder)
    ///    {
    ///         // Utilisation de Linq
    ///         Items = myData.AsQueryable()
    ///             .Where(filtersAndOrder.Where[0])
    ///             .OrderBy(filtersAndOrder.OrderBy[0]).ToList();
    ///
    ///         // Utilisation d'Entity Framework Linq
    ///         Items = dbContext.Set&lt;Item&gt;()
    ///             .Where(filtersAndOrder.Where[0])
    ///             .OrderBy(filtersAndOrder.OrderBy[0])
    ///
    ///        // Utilisation de Simple.Client.OData
    ///        var client = new ODataClient("https://my-service.com/odata");
    ///        Items = await client.For&lt;Item&gt;()
    ///            .Filter(filtersAndOrder.Where[0])
    ///            .OrderBy(filtersAndOrder.OrderBy[0])
    ///            .FindEntriesAsync();
    ///    }
    /// }    
    /// </code>
    /// </example>
    /// </para>
    /// </summary>
    /// <typeparam name="TGridItem">Le type d'élément de la grille.</typeparam>
    public struct GridFilteringAndSorting<TGridItem>
    {
        /// <summary>
        /// Tableau d'expressions nullable qui prennent un TGridItem et renvoient une valeur booléenne.
        /// </summary>
        public Expression<Func<TGridItem, bool>>[]? Where { get; set; }
        public (SortedLinq, Expression<Func<TGridItem, object>>)[]? Values { get; set; }

        public readonly bool ValuesIsNotNullAndEmpty()
        {
            if (Values != null && Values.Any())
                return true;
            else return false;
        }

        public readonly bool WhereIsNotNullAndEmpty()
        {
            if (Where != null && Where.Any())
                return true;
            else return false;
        }
        public readonly IQueryable<TGridItem>? GetQueryableWhithWere(IQueryable<TGridItem> queryable)
        {
            var expression = GetWhereAggregateAnd();
            if (expression != null)
                return queryable.Where(expression);
            return null;
        }
        public readonly IOrderedQueryable<TGridItem>? GetQueryableSorting(IQueryable<TGridItem> queryable)
        {
            IOrderedQueryable<TGridItem> query = queryable.Order();
            if (ValuesIsNotNullAndEmpty())
            {
                foreach (var value in Values!)
                {
                    (var sort, Expression<Func<TGridItem, object>> exp) = value;
                    if (sort == SortedLinq.OrderBy)
                        query = queryable.OrderBy(exp);
                    if (sort == SortedLinq.OrderByDescending)
                        query = queryable.OrderByDescending(exp);
                    if (sort == SortedLinq.ThenBy)
                        query = query.ThenBy(exp);
                    if (sort == SortedLinq.ThenByDescending)
                        query = query.ThenByDescending(exp);
                }
                return query;
            }
            return null;
        }
        public IQueryable<TGridItem> GetQuerable(IQueryable<TGridItem> queryable)
        {
            var q = GetQueryableWhithWere(queryable);
            if (q != null)
                queryable = q;
            var oq = GetQueryableSorting(queryable);
            if (oq != null)
                queryable = oq;
            return queryable;
        }
        public readonly Expression<Func<TGridItem, bool>>? GetWhereAggregateAnd()
        {
            if (WhereIsNotNullAndEmpty())
                return GetAggregate(ExpressionType.AndAlso, Where!);
            else return null;
        }
        public readonly Expression<Func<TGridItem, bool>>? GetWhereAggregateOr()
        {
            if (WhereIsNotNullAndEmpty())
                return GetAggregate(ExpressionType.OrElse, Where!);
            else return null;
        }

        private readonly Expression<Func<TGridItem, T>> GetAggregate<T>(ExpressionType expressionType, IEnumerable<Expression<Func<TGridItem, T>>> expressions)
        {
            var parameter = Expression.Parameter(typeof(TGridItem), "x");

            Expression expression = expressionType switch
            {
                ExpressionType.AndAlso => expressions.Select(e => e.Body).Aggregate(Expression.AndAlso),
                ExpressionType.OrElse => expressions.Select(e => e.Body).Aggregate(Expression.OrElse),
                ExpressionType.And => expressions.Select(e => e.Body).Aggregate(Expression.And),
                ExpressionType.AndAssign => expressions.Select(e => e.Body).Aggregate(Expression.AndAssign),
                _ => throw new NotImplementedException(),
            };
            return Expression.Lambda<Func<TGridItem, T>>(expression, parameter);
        }
    }
}
