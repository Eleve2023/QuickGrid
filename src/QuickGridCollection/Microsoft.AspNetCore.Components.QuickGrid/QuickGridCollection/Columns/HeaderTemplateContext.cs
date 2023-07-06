using System.Linq.Expressions;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns
{
    /// <summary>
    /// Provides a context for the custom column header
    /// defined in the <see cref="ColumnCBase{TGridItem}.HeaderTemplate"/> property.
    /// </summary>
    /// <typeparam name="TGridItem">The type of data items displayed in the grid.</typeparam>
    /// <summary xml:lang="fr">
    /// Fournit un contexte pour l'en-tête personnalisé du la colonne
    /// définies dans la propriété <see cref="ColumnCBase{TGridItem}.HeaderTemplate"/>.    
    /// </summary>
    /// <typeparam name="TGridItem" xml:lang="fr">Le type des éléments de données affichés dans la grille.</typeparam>
    public class HeaderTemplateContext<TGridItem>
    {
        private readonly ColumnCBase<TGridItem> column;

        public HeaderTemplateContext(ColumnCBase<TGridItem> column)
        {
            this.column = column;
        }

        /// <summary>
        /// Indicates whether the column is sortable.
        /// </summary>
        /// /// <remarks>
        /// if <see cref="IsSortable"/> is set to <c>true</c> you must use the method <see cref="SetPropertyExpressionAndType{TPro}(Expression{Func{TGridItem, TPro}})"/>.
        /// </remarks>
        /// <summary xml:lang="fr">
        /// Indique si la colonne est triable.        
        /// </summary>
        /// /// <remarks xml:lang="fr">
        /// si <see cref="IsSortable"/> est définir sur <c>true</c> vous devait utilise la méthode <see cref="SetPropertyExpressionAndType{TPro}(Expression{Func{TGridItem, TPro}})"/>.
        /// </remarks>
        public bool IsSortable
        {
            get => column.IsSortable; 
            set
            {
                column.IsSortable = value;
                CheckSortability();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this column can be sorted with other sortable columns.
        /// </summary>
        /// <remarks>
        /// If this property is set to <c>true</c> and the <see cref="IsSortable"/> property is also set to <c>true</c>, this column can be sorted with other sortable columns.
        /// </remarks>
        /// <summary xml:lang="fr">
        /// Obtient ou définit une valeur indiquant si cette colonne peut être triée avec d'autres colonnes triables.
        /// </summary>
        /// <remarks xml:lang="fr">
        /// Si cette propriété est définie sur <c>true</c> et que la propriété <see cref="IsSortable"/> est également définie sur <c>true</c>, cette colonne peut être triée avec d'autres colonnes triables.
        /// </remarks>
        public bool MultipleSortingAllowed { get => column.MultipleSortingAllowed; set => column.MultipleSortingAllowed = value; }

        /// <summary>
        /// Sets the property expression and property type for the column using a lambda expression.
        /// </summary>
        /// <typeparam name="TPro">The type of the property to use.</typeparam>
        /// <param name="expression">The lambda expression representing the property to use.</param>
        /// <summary xml:lang="fr">
        /// Définit l'expression de propriété et le type de propriété pour la colonne en utilisant une expression lambda.
        /// </summary>
        /// <typeparam name="TPro" xml:lang="fr">Le type de la propriété à utiliser.</typeparam>
        /// <param name="expression">L'expression lambda représentant la propriété à utiliser.</param>
        public void SetPropertyExpressionAndType<TPro>(Expression<Func<TGridItem, TPro>> expression)
        {
            column.SetPropertyExpressionAndTypet(expression);
        }

        /// <summary>
        /// Adds or updates a filter for this column.
        /// If the filter expression is null, the existing filter for this column is removed.
        /// Otherwise, the existing filter for this column is updated or added to the list of filters.
        /// </summary>
        /// <summary xml:lang="fr">
        /// Ajoute ou met à jour un filtre pour cette colonne.
        /// Si l'expression de filtre est nulle, le filtre existant pour cette colonne est supprimé.
        /// Sinon, le filtre existant pour cette colonne est mis à jour ou ajouté à la liste des filtres.
        /// </summary>
        public void ApplyColumnFilter(Expression<Func<TGridItem, bool>>? expression)
        {
            column.Grid.ApplyColumnFilter(expression, column);
        }

        /// <summary>
        /// Sorts grid data.
        /// </summary>
        /// <summary xml:lang="fr">
        /// Trie les données de la grille.
        /// </summary>
        public void ApplySort()
        {
            if (CheckSortability())
                column.ApplySort();
            else throw new Exception();
        }

        /// <summary> 
        /// Get the sort direction of the column.
        /// </summary>
        /// <returns>The sort direction or null if the column is not sortable</returns>
        /// <summary xml:lang="fr"> 
        /// Obtenir la direction de tri de la colonne.
        /// </summary>
        /// <returns xml:lang="fr">La direction de tri ou null si la colonne n’est pas triable</returns>
        public SortDirection? GetSortDirection()
        {
            return column.Grid.GetSortDirection(column);
        }

        /// <summary> 
        /// Checks if the column is sortable and updates its sort state accordingly.
        /// </summary> 
        /// <returns>Returns true if the column is sortable, false otherwise.</returns>
        /// <summary xml:lang="fr"> 
        /// Vérifie si la colonne est triable et met à jour son état de tri en conséquence.
        /// </summary> 
        /// <returns xml:lang="fr">Retourne true si la colonne est triable, false sinon.</returns>
        public bool CheckSortability()
        {
            if (IsSortable && column.PropertyExpression != null)
            {
                column.Grid.ColumnSortDirectionsAdding(column);
                return true;
            }
            else
                return false;            
        }
    }
}
