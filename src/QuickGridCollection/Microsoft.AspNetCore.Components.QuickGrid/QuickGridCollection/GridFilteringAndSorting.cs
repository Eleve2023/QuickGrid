using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Infrastructure;
using System;
using System.Linq.Expressions;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection
{
    /// <summary>
    /// La structure <see cref="GridFilteringAndSorting{TGridItem}"/> est un type générique qui prend un paramètre de type <c>TGridItem</c> est le même typeparam que <see cref="QuickGridC{TGridItem}"/>.
    /// Elle est utilisée par un composant <see cref="QuickGridC{TGridItem}"/> via la propriété <see cref="QuickGridC{TGridItem}.FilterSortChanged"/>.
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
    ///         Items = filtersAndOrder.ApplyFilterAndSortExpressions(myData.AsQueryable()).ToList();
    ///
    ///         // Utilisation d'Entity Framework Linq
    ///         Items = filtersAndOrder.ApplyFilterAndSortExpressions(dbContext.Set&lt;Item&gt;()).ToList();
    ///
    ///        // Utilisation de Simple.Client.OData
    ///        var client = new ODataClient("https://my-service.com/odata");
    ///        var queryableOdata = client.For&lt;Item&gt;();
    ///        if (filtersAndOrder.HasFilterExpressions())
    ///           queryableOdata.Filter(filtersAndOrder.CombineFilterExpressionsWithAnd()!);
    ///        if(filtersAndOrder.HasSortExpressions())
    ///        {
    ///            foreach (var value in filtersAndOrder.SortExpressions)
    ///            {
    ///                (var sort, var exp) = value;
    ///                if (sort == SortedLinq.OrderBy)
    ///                    queryableOdata.OrderBy(exp);
    ///                if (sort == SortedLinq.OrderByDescending)
    ///                    queryableOdata.OrderByDescending(exp);
    ///                if (sort == SortedLinq.ThenBy)
    ///                    queryableOdata.ThenBy(exp);
    ///                if (sort == SortedLinq.ThenByDescending)
    ///                    queryableOdata.ThenByDescending(exp);
    ///            }
    ///        }
    ///        Items = await queryableOdata.FindEntriesAsync();
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
        /// Ces expressions peuvent être utilisées pour filtrer les éléments de la grille.
        /// Par exemple, 
        /// <code>
        /// var queryable = myData.AsQueryable().Where(gridFilteringAndSorting.FilterExpressions[0])
        /// </code>
        /// </summary>
        public Expression<Func<TGridItem, bool>>[]? FilterExpressions { get; internal set; }
        /// <summary>
        /// Tableau nullable de tuples contenant un élément de type SortedLinq et une expression qui prend un TGridItem et renvoie une valeur de type object.
        /// Ces tuples peuvent être utilisés pour trier les éléments de la grille.
        /// Le premier élément du tuple indique la méthode de tri à utiliser (OrderBy, OrderByDescending, ThenBy ou ThenByDescending) et le deuxième élément est l'expression de tri.
        /// Par exemple, pour trier les éléments dans l'ordre croissant, on peut utiliser le code suivant:
        /// <code>
        /// var queryable = myData.AsQueryable();
        /// if (gridFilteringAndSorting.SortExpressions[0].Item1 == SortedLinq.OrderBy)
        ///    queryable = queryable.OrderBy(gridFilteringAndSorting.SortExpressions[0].Item2);
        /// </code>
        /// </summary>
        public (SortedLinq, Expression<Func<TGridItem, object>>)[]? SortExpressions { get; internal set; }

        /// <summary>
        /// Vérifie si le tableau de tuples de tri <see cref="GridFilteringAndSorting{TGridItem}.SortExpressions"/> est non nul et non vide.
        /// </summary>
        /// <returns>Retourne <c>true</c> si le tableau de tuples de tri est non nul et non vide, sinon <c>false</c>.</returns>
        public readonly bool HasSortExpressions()
        {
            if (SortExpressions != null && SortExpressions.Length != 0)
                return true;
            else return false;
        }
        /// <summary>
        /// Vérifie si le tableau d'expressions de filtrage <see cref="GridFilteringAndSorting{TGridItem}.FilterExpressions"/> est non nul et non vide.
        /// </summary>
        /// <returns>Retourne <c>true</c> si le tableau d'expressions de filtrage est non nul et non vide, sinon <c>false</c>.</returns>
        public readonly bool HasFilterExpressions()
        {
            if (FilterExpressions != null && FilterExpressions.Length != 0)
                return true;
            else return false;
        }
        /// <summary>
        /// Applique les expressions de filtrage <see cref="GridFilteringAndSorting{TGridItem}.FilterExpressions"/> au IQueryable fourni en utilisant l'opérateur AndAlso (ET logique).
        /// </summary>
        /// <param name="queryable">Le <see cref="IQueryable"/> à filtrer.</param>
        /// <returns>Retourne un <see cref="IQueryable"/> filtré ou <c>null</c> si aucune expression de filtrage n'est définie.</returns>
        public readonly IQueryable<TGridItem>? ApplyFilterExpressions(IQueryable<TGridItem> queryable)
        {
            var expression = CombineFilterExpressionsWithAndForLinq();
            if (expression != null)
                return queryable.Where(expression);
            return null;
        }
        /// <summary>
        /// Applique les tuples de tri <see cref="GridFilteringAndSorting{TGridItem}.SortExpressions"/> au <see cref="IQueryable"/> fourni.
        /// </summary>
        /// <param name="queryable">Le <see cref="IQueryable"/> à trier.</param>
        /// <returns>Retourne un <see cref="IOrderedQueryable"/> trié ou  <c>null</c> si aucun tuple de tri n'est défini.</returns>
        public readonly IOrderedQueryable<TGridItem>? ApplySortExpressions(IQueryable<TGridItem> queryable)
        {
            IOrderedQueryable<TGridItem> query = queryable.Order();
            if (HasSortExpressions())
            {
                foreach (var value in SortExpressions!)
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
        /// <summary>
        /// Applique les expressions de filtrage <see cref="GridFilteringAndSorting{TGridItem}.FilterExpressions"/> et les tuples de tri <see cref="GridFilteringAndSorting{TGridItem}.SortExpressions"/> au <see cref="IQueryable"/> fourni.
        /// </summary>
        /// <param name="queryable">Le <see cref="IQueryable"/> à filtrer et trier.</param>
        /// <returns>Retourne un <see cref="IQueryable"/> filtré et trié.</returns>
        public readonly IQueryable<TGridItem> ApplyFilterAndSortExpressions(IQueryable<TGridItem> queryable)
        {
            var q = ApplyFilterExpressions(queryable);
            if (q != null)
                queryable = q;
            var oq = ApplySortExpressions(queryable);
            if (oq != null)
                queryable = oq;
            return queryable;
        }
        /// <summary>
        /// Combine les expressions de filtrage <see cref="GridFilteringAndSorting{TGridItem}.FilterExpressions"/> en utilisant l'opérateur AndAlso (ET logique).
        /// </summary>
        /// <returns>Retourne une expression combinée ou <c>null</c> si aucune expression de filtrage n'est définie.</returns>
        public readonly Expression<Func<TGridItem, bool>>? CombineFilterExpressionsWithAnd()
        {
            if (HasFilterExpressions())
                return CombineExpressions(ExpressionType.AndAlso, FilterExpressions!);
            else return null;
        }
        /// <summary>
        /// Combine les expressions de filtrage <see cref="GridFilteringAndSorting{TGridItem}.FilterExpressions"/> en utilisant l'opérateur AndAlso (ET logique).
        /// Ajoute un contre valeur null pour les type nullable
        /// </summary>
        /// <returns>Retourne une expression combinée ou <c>null</c> si aucune expression de filtrage n'est définie.</returns>
        public readonly Expression<Func<TGridItem, bool>>? CombineFilterExpressionsWithAndForLinq()
        {
            if (HasFilterExpressions())
            {
                var expressions = FilterExpressions!.Select(e => (Expression<Func<TGridItem, bool>>)NullableCheck.AddNullCheck(e, true));
                return CombineExpressions(ExpressionType.AndAlso, expressions!);
            }
            else return null;
        }
        /// <summary>
        /// Combine les expressions de filtrage <see cref="GridFilteringAndSorting{TGridItem}.FilterExpressions"/> en utilisant l'opérateur OrElse (OU logique).
        /// </summary>
        /// <returns>Retourne une expression combinée ou <c>null</c> si aucune expression de filtrage n'est définie.</returns>
        public readonly Expression<Func<TGridItem, bool>>? CombineFilterExpressionsWithOr()
        {
            if (HasFilterExpressions())
                return CombineExpressions(ExpressionType.OrElse, FilterExpressions!);
            else return null;
        }
        /// <summary>
        /// Combine les expressions en utilisant l'opérateur spécifié.
        /// </summary>
        /// <typeparam name="T">Le type de retour des expressions.</typeparam>
        /// <param name="expressionType">L'opérateur à utiliser pour combiner les expressions.</param>
        /// <param name="expressions">Les expressions à combiner.</param>
        /// <returns>Retourne une expression combinée.</returns>
        private readonly Expression<Func<TGridItem, T>> CombineExpressions<T>(ExpressionType expressionType, IEnumerable<Expression<Func<TGridItem, T>>> expressions)
        {
            var parameter = Expression.Parameter(typeof(TGridItem), "x");
            var replacedExpressions = expressions.Select(e => ((Expression<Func<TGridItem, T>>)ParameterReplacer.Replace(e, e.Parameters[0], parameter)).Body);
            Expression expression = expressionType switch
            {
                ExpressionType.AndAlso => replacedExpressions.Aggregate(Expression.AndAlso),
                ExpressionType.OrElse => replacedExpressions.Aggregate(Expression.OrElse),
                ExpressionType.And => replacedExpressions.Aggregate(Expression.And),
                ExpressionType.AndAssign => replacedExpressions.Aggregate(Expression.AndAssign),
                _ => throw new NotImplementedException(),
            };
            return Expression.Lambda<Func<TGridItem, T>>(expression, parameter);
        }
    }
}
