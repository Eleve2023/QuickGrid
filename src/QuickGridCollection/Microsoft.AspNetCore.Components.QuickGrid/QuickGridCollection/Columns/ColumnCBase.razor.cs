using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection;
using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.GridCss;
using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Infrastructure;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns
{
    /// <summary>
    /// Représente une colonne dans une grille de données.
    /// Elle contient des propriétés et des méthodes pour gérer les options de colonne,
    /// telles que la visibilité des options, la possibilité de trier et de filtrer les données,
    /// ainsi que les expressions de propriété et les types de propriété pour la colonne.
    /// Cette classe est liée au composant Blazor Column.razor qui utilise des balises Razor
    /// pour afficher une grille avec des colonnes et des lignes.
    /// </summary>
    /// <typeparam name="TGridItem">Le type des éléments de données affichés dans la grille.</typeparam>
    [CascadingTypeParameter(nameof(TGridItem))]
    public abstract partial class ColumnCBase<TGridItem> : ComponentBase
    {
        /// <summary>
        /// Référence à la  dernière instance de <see cref="ColumnCBase{TGridItem}"/> assignée à cette variable.
        /// </summary>
        private ColumnCBase<TGridItem>? _lastAssignedColumn;
        /// <summary>
        /// Indique si les options de colonne sont affichées ou masquées.
        /// </summary>
        private bool isOptionVisible = false;
        /// <summary>
        /// Indique si la colonne est triable. 
        /// </summary>
        /// <remarks>
        /// si <see cref="isSortable"/> est définir sur <c>true</c> la <see cref="ColumnCBase{TGridItem}.PropertyExpression"/> ne doit pas être <c>null</c>
        /// </remarks>
        protected bool isSortable = false;
        /// <summary>
        /// Nombre maximum de filtres à applique pour cette colonne. La valeur par défaut est 5.
        /// </summary>
        protected int maxFilters = 5;
        /// <summary>
        /// Indique si la colonne a des options de filtre avancé.
        /// </summary>
        /// <remarks>
        /// si <see cref="hasAdvancedFilterOptions"/> est définir sur <c>true</c> la <see cref="ColumnCBase{TGridItem}.PropertyExpression"/> ne doit pas être <c>null</c>
        /// </remarks>
        protected bool hasAdvancedFilterOptions;
        /// <summary>
        /// Indique si la colonne a des options de filtre.        
        /// </summary>
        /// /// <remarks>
        /// si <see cref="hasFilterOptions"/> est définir sur <c>true</c> la <see cref="ColumnCBase{TGridItem}.PropertyExpression"/> ne doit pas être <c>null</c>
        /// </remarks>
        protected bool hasFilterOptions;
        /// <summary>
        /// Indique si les options de la colonne sont applique 
        /// </summary>
        private bool optionApplied;
        /// <summary>
        /// Type de la propriété de la colonne.
        /// </summary>
        protected Type? typeOfProperty;
        /// <summary>
        /// Expression de propriété pour la colonne.
        /// </summary>
        protected Expression<Func<TGridItem, object>>? propertyExpression;

        protected ColumnCBase()
        {
            HeaderContent = RenderDefaultHeaderContent;
        }
        /// <summary>
        /// Contexte interne de la grille.
        /// </summary>
        [CascadingParameter] internal InternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;
        /// <summary>
        /// Titre de la colonne.
        /// </summary>
        [Parameter] public string? Title { get; set; }
        /// <summary>
        /// Modèle d'en-tête personnalisé pour la colonne.
        /// </summary>
        [Parameter] public RenderFragment<HeaderTemplateContext<TGridItem>>? HeaderTemplate { get; set; }
        /// <summary>
        /// Options de colonne personnalisées.
        /// </summary>
        [Parameter] public RenderFragment<ColumnOptionsContext<TGridItem>>? ColumnOptions { get; set; }
        /// <summary>
        /// Contenu d'en-tête de la colonne.
        /// </summary>
        protected internal RenderFragment HeaderContent { get; protected set; }
        /// <summary>
        /// Expression de propriété pour la colonne.
        /// </summary>
        internal Expression<Func<TGridItem, object>>? PropertyExpression => propertyExpression;
        /// <summary>
        /// Type de la propriété de la colonne.
        /// </summary>
        internal Type? TypeOfProperty => typeOfProperty;
        /// <summary>
        /// Indique si la colonne est triable.        
        /// </summary>
        /// /// <remarks>
        /// si <see cref="IsSortable"/> est définir sur <c>true</c> la <see cref="ColumnCBase{TGridItem}.PropertyExpression"/> ne doit pas être <c>null</c>
        /// </remarks>
        internal bool IsSortable { get => isSortable; set => isSortable = value; }
        /// <summary>
        /// instance de <see cref="QuickGridC{TGridItem}"/>
        /// </summary>
        internal QuickGridC<TGridItem> Grid => InternalGridContext.Grid;
        /// <summary>
        /// Objet permettant de gérer les classes CSS et les styles des éléments HTML de la grille.
        /// </summary>
        internal GridHtmlCssManager ClassAndStyle => Grid.ClassAndStyle;
        /// <summary>
        /// Indique si les options de la colonne sont applique. 
        /// </summary>
        internal bool OptionApplied { get => optionApplied; set => optionApplied = value; }
        /// <summary>
        /// Nombre maximum de filtres à applique pour cette colonne. La valeur par défaut est 5, La valeur minimal est 2
        /// </summary>
        /// <remarks>
        /// Cette propriété est utilise par <see cref="MenuAdvancedFilter{TGridItem}"/>
        /// </remarks>
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
        /// <summary>
        /// Indique si les options de colonne sont affichées ou masquées.
        /// </summary>
        internal bool IsOptionVisible { get => isOptionVisible; set => isOptionVisible = value; }

        protected internal abstract void CellContent(RenderTreeBuilder builder, TGridItem item);
        /// <summary>
        /// Ajoute une colonne à la grille.
        /// </summary>
        protected void AddColumn()
        {
            Grid.AddColumn(_lastAssignedColumn = this);
        }
        /// <summary>
        /// Définit l'expression de propriété et le type de propriété pour la colonne.
        /// </summary>
        /// <param name="memberExp">Expression de membre à utiliser.</param>
        internal void SetPropertyExpressionAndTypet(MemberExpression memberExp)
        {
            Expression? finalExpression = null;
            Type itemType = typeof(TGridItem);
            PropertyInfo? propertyInfo = itemType.GetProperty(memberExp.Member.Name);

            if (propertyInfo != null)
            {
                typeOfProperty = propertyInfo.PropertyType;
                var parameter = Expression.Parameter(itemType, "x");
                Expression propertyExpression = Expression.Property(parameter, propertyInfo);
                if (propertyInfo.PropertyType.IsValueType)
                    propertyExpression = Expression.Convert(propertyExpression, typeof(object));

                finalExpression = Expression.Lambda<Func<TGridItem, object>>(propertyExpression, parameter);
            }
            propertyExpression = (Expression<Func<TGridItem, object>>?)finalExpression;
        }
        /// <summary>
        /// Définit l'expression de propriété et le type de propriété pour la colonne en utilisant une expression lambda.
        /// </summary>
        /// <typeparam name="TPro">Le type de la propriété à utiliser.</typeparam>
        /// <param name="expression">L'expression lambda représentant la propriété à utiliser.</param>
        internal void SetPropertyExpressionAndTypet<TPro>(Expression<Func<TGridItem, TPro>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
                SetPropertyExpressionAndTypet(memberExpression);
        }
        /// <summary>
        /// Affiche ou masque les options de colonne.
        /// </summary>
        private void ToggleColumnOptionsVisibility()
        {
            isOptionVisible = !isOptionVisible;
            Grid.StateChanged();
        }
        /// <summary>
        /// Résout la classe CSS pour les options de colonne.
        /// </summary>
        private string GetColumnOptionCssClass()
        {
            if (ColumnOptions != null)
            {
                return optionApplied ? ClassAndStyle[ClassHtml.Column_i_i_ColumnOptionActive] :
                    ClassAndStyle[ClassHtml.Column_i_i_ColumnOptionNotActive];
            }
            else if (hasAdvancedFilterOptions)
            {
                return optionApplied ? ClassAndStyle[ClassHtml.Column_i_i_MenuAdvancedFilterActive] :
                    ClassAndStyle[ClassHtml.Column_i_i_MenuAdvancedFilterNotActive];
            }
            else if (hasFilterOptions)
            {
                return optionApplied ? ClassAndStyle[ClassHtml.Column_i_i_MenuFiltreActive] :
                    ClassAndStyle[ClassHtml.Column_i_i_MenuFiltreNotActive];
            }
            else
                return "";
        }
        /// <summary>
        /// Résout la Style CSS pour les options de colonne.
        /// </summary>
        private string GetColumnOptionCssStyle()
        {
            if (ColumnOptions != null)
            {
                return optionApplied ? ClassAndStyle[StyleCss.Column_i_i_ColumnOptionActive] :
                    ClassAndStyle[StyleCss.Column_i_i_ColumnOptionNotActive];
            }
            else if (hasAdvancedFilterOptions)
            {
                return optionApplied ? ClassAndStyle[StyleCss.Column_i_i_MenuAdvancedFilterActive] :
                    ClassAndStyle[StyleCss.Column_i_i_MenuAdvancedFilterNotActive];
            }
            else if (hasFilterOptions)
            {
                return optionApplied ? ClassAndStyle[StyleCss.Column_i_i_MenuFiltreActive] :
                    ClassAndStyle[StyleCss.Column_i_i_MenuFiltreNotActive];
            }
            else
                return "";
        }
        internal void ApplySort()
        {
            Grid.ApplySort(_lastAssignedColumn!);
        }
        internal string GetSortClass()
        {
            return Grid.GetSortClass(_lastAssignedColumn!);
        }
        internal string GetSortStyle()
        {
            return Grid.GetSortStyle(_lastAssignedColumn!);
        }

    }
}
