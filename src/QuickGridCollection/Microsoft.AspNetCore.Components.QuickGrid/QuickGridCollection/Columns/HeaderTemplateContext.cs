using System.Linq.Expressions;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns
{
    /// <summary>
    /// Fournit un contexte pour l'en-tête personnalisé du la colonne
    /// définies dans la propriété <see cref="ColumnCBase{TGridItem}.HeaderTemplate"/>.    
    /// </summary>
    /// <typeparam name="TGridItem">Le type des éléments de données affichés dans la grille.</typeparam>
    public class HeaderTemplateContext<TGridItem>
    {
        private readonly ColumnCBase<TGridItem> column;

        public HeaderTemplateContext(ColumnCBase<TGridItem> column)
        {
            this.column = column;
        }

        /// <summary>
        /// Indique si la colonne est triable.        
        /// </summary>
        /// /// <remarks>
        /// si <see cref="IsSortable"/> est définir sur <c>true</c> vous devait utilise la méthode <see cref="SetPropertyExpressionAndType{TPro}(Expression{Func{TGridItem, TPro}})"/>.
        /// </remarks>
        public bool IsSortable { get => column.IsSortable; set => column.IsSortable = value; }
        /// <summary>
        /// Obtient ou définit une valeur indiquant si cette colonne peut être triée avec d'autres colonnes triables.
        /// </summary>
        /// <remarks>
        /// Si cette propriété est définie sur <c>true</c> et que la propriété <see cref="IsSortable"/> est également définie sur <c>true</c>, cette colonne peut être triée avec d'autres colonnes triables.
        /// </remarks>
        internal bool MultipleSortingAllowed { get => column.MultipleSortingAllowed; set => column.MultipleSortingAllowed = value; }
        /// <summary>
        /// Définit l'expression de propriété et le type de propriété pour la colonne en utilisant une expression lambda.
        /// </summary>
        /// <typeparam name="TPro">Le type de la propriété à utiliser.</typeparam>
        /// <param name="expression">L'expression lambda représentant la propriété à utiliser.</param>
        public void SetPropertyExpressionAndType<TPro>(Expression<Func<TGridItem, TPro>> expression)
        {
            column.SetPropertyExpressionAndTypet(expression);
        }

        /// <summary>
        /// Ajoute ou met à jour un filtre pour cette colonne.
        /// Si l'expression de filtre est nulle, le filtre existant pour cette colonne est supprimé.
        /// Sinon, le filtre existant pour cette colonne est mis à jour ou ajouté à la liste des filtres.
        /// </summary>
        public void ApplyColumnFilter(Expression<Func<TGridItem, bool>>? expression)
        {
            column.Grid.ApplyColumnFilter(expression, column);
        }
        public void ApplySort()
        {
            if (column.PropertyExpression != null)
                column.ApplySort();
            else throw new Exception();
        }
    }
}
