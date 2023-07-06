using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Infrastructure;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns
{
    /// <summary xml:lang="fr">
    /// Représente une colonne dans une grille de données.
    /// Elle contient des propriétés et des méthodes pour gérer les options de colonne,
    /// telles que la visibilité des options, la possibilité de trier et de filtrer les données,
    /// ainsi que les expressions de propriété et les types de propriété pour la colonne.    
    /// </summary>
    /// <typeparam name="TGridItem" xml:lang="fr">Le type des éléments de données affichés dans la grille.</typeparam>
    [CascadingTypeParameter(nameof(TGridItem))]
    public abstract partial class ColumnCBase<TGridItem> : ComponentBase
    {
        /// <summary xml:lang="fr">
        /// Référence à la  dernière instance de <see cref="ColumnCBase{TGridItem}"/> assignée à cette variable.
        /// </summary>
        private ColumnCBase<TGridItem>? _lastAssignedColumn;
        /// <summary xml:lang="fr">
        /// Indique si les options de colonne sont affichées ou masquées.
        /// </summary>
        private bool isOptionVisible = false;
        /// <summary xml:lang="fr">
        /// Indique si la colonne est triable. 
        /// </summary>
        /// <remarks xml:lang="fr">
        /// si <see cref="isSortable"/> est définir sur <c>true</c> la <see cref="ColumnCBase{TGridItem}.PropertyExpression"/> ne doit pas être <c>null</c>
        /// </remarks>
        protected bool isSortable = false;
        /// <summary xml:lang="fr">
        /// Obtient ou définit une valeur indiquant si cette colonne peut être triée avec d'autres colonnes triables.
        /// </summary>
        /// <remarks xml:lang="fr">
        /// Si cette propriété est définie sur <c>true</c> et que la propriété <see cref="IsSortable"/> est également définie sur <c>true</c>, cette colonne peut être triée avec d'autres colonnes triables.
        /// </remarks>
        protected bool multipleSortingAllowed;
        /// <summary xml:lang="fr">
        /// Nombre maximum de filtres à applique pour cette colonne. La valeur par défaut est 5.
        /// </summary>
        protected int maxFilters = 5;
        /// <summary xml:lang="fr">
        /// Indique si la colonne a des options de filtre avancé.
        /// </summary>
        /// <remarks xml:lang="fr">
        /// si <see cref="hasAdvancedFilterOptions"/> est définir sur <c>true</c> la <see cref="ColumnCBase{TGridItem}.PropertyExpression"/> ne doit pas être <c>null</c>
        /// </remarks>
        protected bool hasAdvancedFilterOptions;
        /// <summary xml:lang="fr">
        /// Indique si la colonne a des options de filtre.        
        /// </summary>
        /// /// <remarks xml:lang="fr">
        /// si <see cref="hasFilterOptions"/> est définir sur <c>true</c> la <see cref="ColumnCBase{TGridItem}.PropertyExpression"/> ne doit pas être <c>null</c>
        /// </remarks>
        protected bool hasFilterOptions;
        /// <summary xml:lang="fr">
        /// Indique si les options de la colonne sont applique 
        /// </summary>
        private bool optionApplied;
        /// <summary xml:lang="fr">
        /// Type de la propriété de la colonne.
        /// </summary>
        protected Type? typeOfProperty;
        /// <summary xml:lang="fr">
        /// Expression de propriété pour la colonne.
        /// </summary>
        protected Expression<Func<TGridItem, object?>>? propertyExpression;

        protected ColumnCBase()
        {
            HeaderContent = RenderDefaultHeaderContent;
            SortContent = RenderSortContent;
            OptionsContent = RenderOptionsContent;
        }

        /// <summary xml:lang="fr">
        /// Contexte interne de la grille.
        /// </summary>
        [CascadingParameter] internal InternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;
        /// <summary xml:lang="fr">
        /// Titre de la colonne.
        /// </summary>
        [Parameter] public string? Title { get; set; }
        /// <summary xml:lang="fr">
        /// Modèle d'en-tête personnalisé pour la colonne.
        /// </summary>
        [Parameter] public RenderFragment<HeaderTemplateContext<TGridItem>>? HeaderTemplate { get; set; }
        /// <summary xml:lang="fr">
        /// Options de colonne personnalisées.
        /// </summary>
        [Parameter] public RenderFragment<ColumnOptionsContext<TGridItem>>? ColumnOptions { get; set; }
        /// <summary xml:lang="fr">
        /// Contenu d'en-tête de la colonne.
        /// </summary>
        protected internal RenderFragment HeaderContent { get; protected set; }
        /// <summary xml:lang="fr">
        /// Contenu sort d'en-tête de la colonne.
        /// </summary>
        protected internal RenderFragment SortContent { get; protected set; }
        /// <summary xml:lang="fr">
        /// Contenu menu option d'en-tête de la colonne.
        /// </summary>
        protected internal RenderFragment OptionsContent { get; protected set; }

        /// <summary xml:lang="fr">
        /// Expression de propriété pour la colonne.
        /// </summary>
        internal Expression<Func<TGridItem, object?>>? PropertyExpression => propertyExpression;
        /// <summary xml:lang="fr">
        /// Type de la propriété de la colonne.
        /// </summary>
        internal Type? TypeOfProperty => typeOfProperty;

        /// <summary xml:lang="fr">
        /// Indique si la colonne est triable.        
        /// </summary>
        /// /// <remarks xml:lang="fr">
        /// si <see cref="IsSortable"/> est définir sur <c>true</c> la <see cref="ColumnCBase{TGridItem}.PropertyExpression"/> ne doit pas être <c>null</c>
        /// </remarks>
        internal bool IsSortable { get => isSortable; set => isSortable = value; }
        /// <summary xml:lang="fr">
        /// Obtient ou définit une valeur indiquant si cette colonne peut être triée avec d'autres colonnes triables.
        /// </summary>
        /// <remarks xml:lang="fr">
        /// Si cette propriété est définie sur <c>true</c> et que la propriété <see cref="IsSortable"/> est également définie sur <c>true</c>, cette colonne peut être triée avec d'autres colonnes triables.
        /// </remarks>
        internal bool MultipleSortingAllowed { get => multipleSortingAllowed; set => multipleSortingAllowed = value; }
        /// <summary xml:lang="fr">
        /// instance de <see cref="QuickGridC{TGridItem}"/>
        /// </summary>
        internal QuickGridC<TGridItem> Grid => InternalGridContext.Grid;
        /// <summary xml:lang="fr">
        /// Objet permettant de gérer les classes CSS et les styles des éléments HTML de la grille.
        /// </summary>
        internal GridHtmlCssManager ClassAndStyle => Grid.ClassAndStyle;
        /// <summary xml:lang="fr">
        /// Indique si les options de la colonne sont applique. 
        /// </summary>
        internal bool OptionApplied { get => optionApplied; set => optionApplied = value; }
        /// <summary xml:lang="fr">
        /// Nombre maximum de filtres à applique pour cette colonne. La valeur par défaut est 5, La valeur minimal est 2
        /// </summary>
        /// <remarks xml:lang="fr"> Cette propriété est utilise par <see cref="MenuAdvancedFilter{TGridItem}"/> </remarks>
        internal int MaxFilters
        {
            get => maxFilters; set
            {
                if (value < 1)
                    maxFilters = 2;
                else
                    maxFilters = value;
            }
        }
        /// <summary xml:lang="fr">
        /// Indique si les options de colonne sont affichées ou masquées.
        /// </summary>
        internal bool IsOptionVisible { get => isOptionVisible; set => isOptionVisible = value; }

        protected internal abstract void CellContent(RenderTreeBuilder builder, TGridItem item);

        /// <summary xml:lang="fr">
        /// Ajoute une colonne à la grille.
        /// </summary>
        protected void AddColumn()
        {
            Grid.AddColumn(_lastAssignedColumn = this);
        }

        /// <summary xml:lang="fr">
        /// Trie les données de la grille.
        /// Met à jour la liste <see cref="QuickGridC{TGridItem}.columnsSorted"/> en fonction de la nouvelle direction de tri.
        /// Met également à jour la direction de tri dans le dictionnaire <see cref="QuickGridC{TGridItem}.columnSortDirections"/>.
        /// </summary>
        internal void ApplySort()
        {
            Grid.ApplySort(_lastAssignedColumn!);
        }

        /// <summary xml:lang="fr">
        /// Définit l'expression de propriété et le type de propriété pour la colonne.
        /// </summary>
        /// <param name="memberExp"  xml:lang="fr">Expression de membre à utiliser.</param>
        internal void SetPropertyExpressionAndTypet(MemberExpression memberExp)
        {
            var parameterExp = memberExp.Expression as ParameterExpression;            
            propertyExpression = Expression.Lambda<Func<TGridItem, object?>>(Expression.Convert(memberExp, typeof(object)), parameterExp!);
            typeOfProperty = Nullable.GetUnderlyingType(memberExp.Type) ?? memberExp.Type;            
        }

        /// <summary xml:lang="fr">
        /// Définit l'expression de propriété et le type de propriété pour la colonne en utilisant une expression lambda.
        /// </summary>
        /// <typeparam name="TPro" xml:lang="fr">Le type de la propriété à utiliser.</typeparam>
        /// <param name="expression"  xml:lang="fr">\1L'expression lambda représentant la propriété à utiliser.</param>
        internal void SetPropertyExpressionAndTypet<TPro>(Expression<Func<TGridItem, TPro>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
                SetPropertyExpressionAndTypet(memberExpression);
        }

        /// <summary xml:lang="fr">
        /// Affiche ou masque les options de colonne.
        /// </summary>
        private void ToggleColumnOptionsVisibility()
        {
            isOptionVisible = !isOptionVisible;
            Grid.StateChanged();
        }

        /// <summary xml:lang="fr">
        /// Résout la classe CSS pour les options de colonne.
        /// </summary>
        private string GetColumnOptionCssClass()
        {
            if (ColumnOptions != null)
            {
                return optionApplied ? ClassAndStyle[ClassHtml.Column_Options_i_i_ColumnOptionActive] :
                    ClassAndStyle[ClassHtml.Column_Options_i_i_ColumnOptionNotActive];
            }
            else if (hasAdvancedFilterOptions)
            {
                return optionApplied ? ClassAndStyle[ClassHtml.Column_Options_i_i_MenuAdvancedFilterActive] :
                    ClassAndStyle[ClassHtml.Column_Options_i_i_MenuAdvancedFilterNotActive];
            }
            else if (hasFilterOptions)
            {
                return optionApplied ? ClassAndStyle[ClassHtml.Column_Options_i_i_MenuFiltreActive] :
                    ClassAndStyle[ClassHtml.Column_Options_i_i_MenuFiltreNotActive];
            }
            else
                return "";
        }

        /// <summary xml:lang="fr">
        /// Résout le Style CSS pour les options de colonne.
        /// </summary>
        private string GetColumnOptionCssStyle()
        {
            if (ColumnOptions != null)
            {
                return optionApplied ? ClassAndStyle[StyleCss.Column_Options_i_i_ColumnOptionActive] :
                    ClassAndStyle[StyleCss.Column_Options_i_i_ColumnOptionNotActive];
            }
            else if (hasAdvancedFilterOptions)
            {
                return optionApplied ? ClassAndStyle[StyleCss.Column_Options_i_i_MenuAdvancedFilterActive] :
                    ClassAndStyle[StyleCss.Column_Options_i_i_MenuAdvancedFilterNotActive];
            }
            else if (hasFilterOptions)
            {
                return optionApplied ? ClassAndStyle[StyleCss.Column_Options_i_i_MenuFiltreActive] :
                    ClassAndStyle[StyleCss.Column_Options_i_i_MenuFiltreNotActive];
            }
            else
                return "";
        }

        /// <summary xml:lang="fr">
        /// Obtient la classe CSS correspondant à la direction de tri du cette colonne.
        /// Utilise le dictionnaire <see cref="QuickGridC{TGridItem}.sortDirectionCssClasses"/> pour associer chaque direction de tri à sa classe CSS correspondante.
        /// </summary>
        private string GetSortClass()
        {
            return Grid.GetSortClass(_lastAssignedColumn!);
        }

        /// <summary xml:lang="fr">
        /// Obtient la Style CSS correspondant à la direction de tri du cette colonne.
        /// Utilise le dictionnaire <see cref="QuickGridC{TGridItem}.sortDirectionCssClasses"/> pour associer chaque direction de tri à sa classe CSS correspondante.
        /// </summary>
        private string GetSortStyle()
        {
            return Grid.GetSortStyle(_lastAssignedColumn!);
        }
    }
}
