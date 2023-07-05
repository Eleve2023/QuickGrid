using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns;
using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Infrastructure;
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
    ///         Items = filtersAndOrder.ApplyFilterAndSortExpressionsForLinq(myData.AsQueryable()).ToList();
    ///
    ///         // Utilisation d'Entity Framework Linq
    ///         Items = filtersAndOrder.ApplyFilterAndSortExpressions(dbContext.Set&lt;Item&gt;()).ToList();
    ///
    ///        // Utilisation de Simple.Client.OData
    ///        var client = new ODataClient("https://my-service.com/odata");
    ///        var queryableOdata = client.For&lt;Item&gt;();
    ///        if (filtersAndOrder.HasFilterExpressions())
    ///           queryableOdata.Filter(filtersAndOrder.CombineFilterExpressions()!);
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
        public (SortedLinq, Expression<Func<TGridItem, object?>>)[]? SortExpressions { get; internal set; }

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
        /// <param name="useDefaultValueForNull">
        /// Indique si les objets nullables doivent être traités comme ayant une valeur par défaut lors de la génération d'expressions.
        /// <para>Si cette valeur est <c>true</c>, les expressions générées remplaceront les valeurs <c>null</c> par la valeur par défaut du type nullable.</para>
        /// Si cette valeur est <c>false</c>, les expressions générées traiteront les valeurs <c>null</c> comme des valeurs valides.
        /// </param>
        /// <param name="ignoreCaseInStringComparison">
        /// Indique si les comparaisons de chaînes doivent être insensibles à la casse lors de la génération d'expressions.
        /// <para>Si cette valeur est <c>true</c>, les expressions générées convertiront les chaînes en minuscules avant de les comparer, rendant ainsi la comparaison insensible à la casse.</para>
        /// Si cette valeur est <c>false</c>, les expressions générées effectueront des comparaisons sensibles à la casse.
        /// </param>
        /// <returns>Retourne un <see cref="IQueryable"/> filtré ou <c>null</c> si aucune expression de filtrage n'est définie.</returns>
        public readonly IQueryable<TGridItem>? ApplyFilterExpressionsForLinq(
            IQueryable<TGridItem> queryable,
            bool useDefaultValueForNull = false,
            bool ignoreCaseInStringComparison = true,
            FilterOperator operatorType = FilterOperator.AndAlso)
        {
            var expression = CombineFilterExpressionsForLinq(useDefaultValueForNull, ignoreCaseInStringComparison, operatorType);
            if (expression != null)
                return queryable.Where(expression);
            return null;
        }

        /// <summary>
        /// Applique les expressions de filtrage <see cref="GridFilteringAndSorting{TGridItem}.FilterExpressions"/> au IQueryable fourni en utilisant l'opérateur AndAlso (ET logique).
        /// </summary>
        /// <param name="queryable">Le <see cref="IQueryable"/> à filtrer.</param>  
        /// <param name="operatorType">L'opérateur à utiliser pour combiner les expressions.</param>
        /// <returns>Retourne un <see cref="IQueryable"/> filtré ou <c>null</c> si aucune expression de filtrage n'est définie.</returns>
        public readonly IQueryable<TGridItem>? ApplyFilterExpressions(
            IQueryable<TGridItem> queryable,           
            FilterOperator operatorType = FilterOperator.AndAlso)
        {
            var expression = CombineFilterExpressions(operatorType);
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
                    (var sort, Expression<Func<TGridItem, object?>> exp) = value;
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
        /// <param name="useDefaultValueForNull">
        /// Indique si les objets nullables doivent être traités comme ayant une valeur par défaut lors de la génération d'expressions.
        /// <para>Si cette valeur est <c>true</c>, les expressions générées remplaceront les valeurs <c>null</c> par la valeur par défaut du type nullable.</para>
        /// Si cette valeur est <c>false</c>, les expressions générées traiteront les valeurs <c>null</c> comme des valeurs valides.
        /// </param>
        /// <param name="ignoreCaseInStringComparison">
        /// Indique si les comparaisons de chaînes doivent être insensibles à la casse lors de la génération d'expressions.
        /// <para>Si cette valeur est <c>true</c>, les expressions générées convertiront les chaînes en minuscules avant de les comparer, rendant ainsi la comparaison insensible à la casse.</para>
        /// Si cette valeur est <c>false</c>, les expressions générées effectueront des comparaisons sensibles à la casse.
        /// </param>
        /// <param name="operatorType">L'opérateur à utiliser pour combiner les expressions.</param>
        /// <returns>Retourne un <see cref="IQueryable"/> filtré et trié.</returns>
        public readonly IQueryable<TGridItem> ApplyFilterAndSortExpressionsForLinq(
            IQueryable<TGridItem> queryable,
            bool useDefaultValueForNull = false,
            bool ignoreCaseInStringComparison = true,
            FilterOperator operatorType = FilterOperator.AndAlso)
        {
            var q = ApplyFilterExpressionsForLinq(queryable, useDefaultValueForNull, ignoreCaseInStringComparison, operatorType);
            if (q != null)
                queryable = q;
            var oq = ApplySortExpressions(queryable);
            if (oq != null)
                queryable = oq;
            return queryable;
        }

        /// <summary>
        /// Applique les expressions de filtrage <see cref="GridFilteringAndSorting{TGridItem}.FilterExpressions"/> et les tuples de tri <see cref="GridFilteringAndSorting{TGridItem}.SortExpressions"/> au <see cref="IQueryable"/> fourni.
        /// </summary>
        /// <param name="queryable">Le <see cref="IQueryable"/> à filtrer et trier.</param>        
        /// <param name="operatorType">L'opérateur à utiliser pour combiner les expressions.</param>
        /// <returns>Retourne un <see cref="IQueryable"/> filtré et trié.</returns>
        public readonly IQueryable<TGridItem> ApplyFilterAndSortExpressions(
            IQueryable<TGridItem> queryable,            
            FilterOperator operatorType = FilterOperator.AndAlso)
        {
            var q = ApplyFilterExpressions(queryable, operatorType);
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
        /// <param name="operatorType">L'opérateur à utiliser pour combiner les expressions.</param>
        /// <returns>Retourne une expression combinée ou <c>null</c> si aucune expression de filtrage n'est définie.</returns>
        public readonly Expression<Func<TGridItem, bool>>? CombineFilterExpressions(FilterOperator operatorType = FilterOperator.AndAlso)
        {            
            if (HasFilterExpressions())
            {
                var expressionTypet = Enum.Parse<ExpressionType>(operatorType.ToString(), true);
                return CombineExpressions(expressionTypet, FilterExpressions!);
            }
            else return null;
        }
        /// <summary>
        /// Combine les expressions de filtrage <see cref="GridFilteringAndSorting{TGridItem}.FilterExpressions"/> en utilisant l'opérateur AndAlso (ET logique).
        /// Ajoute un contre valeur null pour les type nullable
        /// </summary>
        /// <param name="useDefaultValueForNull">
        /// Indique si les objets nullables doivent être traités comme ayant une valeur par défaut lors de la génération d'expressions.
        /// <para>Si cette valeur est <c>true</c>, les expressions générées remplaceront les valeurs <c>null</c> par la valeur par défaut du type nullable.</para>
        /// Si cette valeur est <c>false</c>, les expressions générées traiteront les valeurs <c>null</c> comme des valeurs valides.
        /// </param>
        /// <param name="ignoreCaseInStringComparison">
        /// Indique si les comparaisons de chaînes doivent être insensibles à la casse lors de la génération d'expressions.
        /// <para>Si cette valeur est <c>true</c>, les expressions générées convertiront les chaînes en minuscules avant de les comparer, rendant ainsi la comparaison insensible à la casse.</para>
        /// Si cette valeur est <c>false</c>, les expressions générées effectueront des comparaisons sensibles à la casse.
        /// </param>
        /// <param name="operatorType">L'opérateur à utiliser pour combiner les expressions.</param>
        /// <returns>Retourne une expression combinée ou <c>null</c> si aucune expression de filtrage n'est définie.</returns>
        public readonly Expression<Func<TGridItem, bool>>? CombineFilterExpressionsForLinq(
            bool useDefaultValueForNull = false,
            bool ignoreCaseInStringComparison = true,
            FilterOperator operatorType = FilterOperator.AndAlso)
        {
            if (HasFilterExpressions())
            {
                var expressions = FilterExpressions!.Select(e => NullableCheckAndOptionStringCompare(e, useDefaultValueForNull, ignoreCaseInStringComparison));                
                var expressionTypet = Enum.Parse<ExpressionType>(operatorType.ToString(),true);
                return CombineExpressions(expressionTypet, expressions!);
            }
            else return null;
        }
                
        private static Expression<Func<TGridItem, bool>> NullableCheckAndOptionStringCompare(Expression<Func<TGridItem, bool>> e, bool useDefaultValueForNull = false, bool ignoreCaseInStringComparison = true)
        {
            return (Expression<Func<TGridItem, bool>>)(new NullableAndStringComparisonExpressionVisitor(useDefaultValueForNull, ignoreCaseInStringComparison)).Visit(e);            
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
        private static Expression<Func<TGridItem, T>> CombineExpressions<T>(ExpressionType expressionType, IEnumerable<Expression<Func<TGridItem, T>>> expressions)
        {
            var parameter = Expression.Parameter(typeof(TGridItem), "x");
            var replacedExpressions = expressions.Select(e => ((Expression<Func<TGridItem, T>>)ParameterReplacer.Replace(e, e.Parameters[0], parameter)).Body);
            Expression expression = expressionType switch
            {
                ExpressionType.And => replacedExpressions.Aggregate(Expression.And),
                ExpressionType.AndAlso => replacedExpressions.Aggregate(Expression.AndAlso),
                ExpressionType.AndAssign => replacedExpressions.Aggregate(Expression.AndAssign),
                ExpressionType.Or => replacedExpressions.Aggregate(Expression.Or),
                ExpressionType.OrElse => replacedExpressions.Aggregate(Expression.OrElse),
                ExpressionType.OrAssign => replacedExpressions.Aggregate(Expression.OrAssign),                
                _ => throw new NotImplementedException(),
            };
            return Expression.Lambda<Func<TGridItem, T>>(expression, parameter);
        }
    }
}
