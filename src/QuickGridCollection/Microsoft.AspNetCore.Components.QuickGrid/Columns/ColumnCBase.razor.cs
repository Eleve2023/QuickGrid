// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

using Components.QuickGrid.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;

namespace Components.QuickGrid.Columns;

/// <summary>
/// Represents a column in a data grid.
/// It contains properties and methods to manage column options,
/// such as visibility of options, ability to sort and filter data,
/// as well as property expressions and property types for the column.
/// </summary>
/// <typeparam name="TGridItem">The type of data items displayed in the grid.</typeparam>
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
    /// <summary>
    /// Reference to the last instance of <see cref="ColumnCBase{TGridItem}"/> assigned to this variable.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Référence à la  dernière instance de <see cref="ColumnCBase{TGridItem}"/> assignée à cette variable.
    /// </summary>
    private ColumnCBase<TGridItem>? _lastAssignedColumn;

    /// <summary>
    /// Indicates whether column options are shown or hidden.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Indique si les options de colonne sont affichées ou masquées.
    /// </summary>
    private bool isOptionVisible;

    /// <summary>
    /// Indicates whether the column is sortable.
    /// </summary>
    /// <remarks>
    /// if <see cref="isSortable"/> is set to <c>true</c> then <see cref="ColumnCBase{TGridItem}.PropertyExpression"/> must not be <c>null</c>
    /// </remarks>
    /// <summary xml:lang="fr">
    /// Indique si la colonne est triable. 
    /// </summary>
    /// <remarks xml:lang="fr">
    /// si <see cref="isSortable"/> est définir sur <c>true</c> la <see cref="ColumnCBase{TGridItem}.PropertyExpression"/> ne doit pas être <c>null</c>
    /// </remarks>
    protected bool isSortable;

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
    protected bool multipleSortingAllowed;

    /// <summary>
    /// Maximum number of filters to apply for this column. The default value is 5.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Nombre maximum de filtres à applique pour cette colonne. La valeur par défaut est 5.
    /// </summary>
    protected int maxFilters = 5;

    /// <summary>
    /// Indicates whether the column has advanced filter options.
    /// </summary>
    /// <remarks>
    /// if <see cref="hasAdvancedFilterOptions"/> is set to <c>true</c> then <see cref="ColumnCBase{TGridItem}.PropertyExpression"/> must not be <c>null</c>
    /// </remarks>
    /// <summary xml:lang="fr">
    /// Indique si la colonne a des options de filtre avancé.
    /// </summary>
    /// <remarks xml:lang="fr">
    /// si <see cref="hasAdvancedFilterOptions"/> est définir sur <c>true</c> la <see cref="ColumnCBase{TGridItem}.PropertyExpression"/> ne doit pas être <c>null</c>
    /// </remarks>
    protected bool hasAdvancedFilterOptions;

    /// <summary>
    /// Indicates whether the column has filter options.
    /// </summary>
    /// /// <remarks>
    /// if <see cref="hasFilterOptions"/> is set to <c>true</c> then <see cref="ColumnCBase{TGridItem}.PropertyExpression"/> must not be <c>null</c>
    /// </remarks>
    /// <summary xml:lang="fr">
    /// Indique si la colonne a des options de filtre.        
    /// </summary>
    /// /// <remarks xml:lang="fr">
    /// si <see cref="hasFilterOptions"/> est définir sur <c>true</c> la <see cref="ColumnCBase{TGridItem}.PropertyExpression"/> ne doit pas être <c>null</c>
    /// </remarks>
    protected bool hasFilterOptions;

    /// <summary>
    /// Indicates whether the column options are applied.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Indique si les options de la colonne sont applique 
    /// </summary>
    private bool optionApplied;

    /// <summary>
    /// Type of the column property.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Type de la propriété de la colonne.
    /// </summary>
    protected Type? typeOfProperty;

    /// <summary>
    /// Property expression for the column.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Expression de propriété pour la colonne.
    /// </summary>
    protected Expression<Func<TGridItem, object?>>? propertyExpression;

    protected ColumnCBase()
    {
        HeaderContent = RenderDefaultHeaderContent;
        SortContent = RenderSortContent;
        OptionsContent = RenderOptionsContent;
        AlignCell = Align.Default;
        AlignHeader = Align.Default;
    }

    /// <summary>
    /// Internal grid context.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Contexte interne de la grille.
    /// </summary>
    [CascadingParameter] internal InternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;

    /// <summary>
    /// Column title.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Titre de la colonne.
    /// </summary>
    [Parameter] public string? Title { get; set; }

    /// <summary>
    /// Custom header template for the column.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Modèle d'en-tête personnalisé pour la colonne.
    /// </summary>
    [Parameter] public RenderFragment<HeaderTemplateContext<TGridItem>>? HeaderTemplate { get; set; }

    /// <summary>
    /// Custom column options.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Options de colonne personnalisées.
    /// </summary>
    [Parameter] public RenderFragment<ColumnOptionsContext<TGridItem>>? ColumnOptions { get; set; }

    /// <summary>
    /// If specified, controls the justification of body cells for this column.
    /// </summary>
    [Parameter] public Align AlignCell { get; set; }

    /// <summary>
    /// If specified, controls the justification of table header for this column.
    /// </summary>
    [Parameter] public Align AlignHeader { get; set; }

    /// <summary>
    /// Column header content.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Contenu d'en-tête de la colonne.
    /// </summary>
    protected internal RenderFragment HeaderContent { get; protected set; }

    /// <summary>
    /// Column header sort content.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Contenu sort d'en-tête de la colonne.
    /// </summary>
    protected internal RenderFragment SortContent { get; protected set; }

    /// <summary>
    /// Column header options menu content.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Contenu menu option d'en-tête de la colonne.
    /// </summary>
    protected internal RenderFragment OptionsContent { get; protected set; }

    /// <summary>
    /// Property expression for the column.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Expression de propriété pour la colonne.
    /// </summary>
    internal Expression<Func<TGridItem, object?>>? PropertyExpression => propertyExpression;

    /// <summary>
    /// Type of the column property.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Type de la propriété de la colonne.
    /// </summary>
    internal Type? TypeOfProperty => typeOfProperty;

    /// <summary>
    /// Indicates whether the column is sortable.
    /// </summary>
    /// /// <remarks>
    /// if <see cref="IsSortable"/> is set to <c>true</c> then <see cref="ColumnCBase{TGridItem}.PropertyExpression"/> must not be <c>null</c>
    /// </remarks>
    /// <summary xml:lang="fr">
    /// Indique si la colonne est triable.        
    /// </summary>
    /// /// <remarks xml:lang="fr">
    /// si <see cref="IsSortable"/> est définir sur <c>true</c> la <see cref="ColumnCBase{TGridItem}.PropertyExpression"/> ne doit pas être <c>null</c>
    /// </remarks>
    internal bool IsSortable { get => isSortable; set => isSortable = value; }

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
    internal bool MultipleSortingAllowed { get => multipleSortingAllowed; set => multipleSortingAllowed = value; }

    /// <summary>
    /// instance of <see cref="QuickGridC{TGridItem}"/>
    /// </summary>
    /// <summary xml:lang="fr">
    /// instance de <see cref="QuickGridC{TGridItem}"/>
    /// </summary>
    internal QuickGridC<TGridItem> Grid => InternalGridContext.Grid;

    /// <summary>
    /// Object for managing CSS classes and styles of HTML elements in the grid.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Objet permettant de gérer les classes CSS et les styles des éléments HTML de la grille.
    /// </summary>
    internal GridHtmlCssManager CssClassAndStyle => Grid.CssClassAndStyle;

    /// <summary>
    /// Indicates whether the column options are applied.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Indique si les options de la colonne sont applique. 
    /// </summary>
    internal bool OptionApplied { get => optionApplied; set => optionApplied = value; }

    /// <summary>
    /// Maximum number of filters to apply for this column. The default value is 5, The minimum value is 2
    /// </summary>
    /// <remarks> This property is used by <see cref="MenuAdvancedFilter{TGridItem}"/> </remarks>
    /// <summary xml:lang="fr">
    /// Nombre maximum de filtres à applique pour cette colonne. La valeur par défaut est 5, La valeur minimal est 2
    /// </summary>
    /// <remarks xml:lang="fr"> Cette propriété est utilise par <see cref="MenuAdvancedFilter{TGridItem}"/> </remarks>
    internal int MaxFilters
    {
        get => maxFilters; set
        {
            if (value < 1)
            {
                maxFilters = 2;
            }
            else
            {
                maxFilters = value;
            }
        }
    }

    /// <summary>
    /// Indicates whether column options are shown or hidden.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Indique si les options de colonne sont affichées ou masquées.
    /// </summary>
    internal bool IsOptionVisible { get => isOptionVisible; set => isOptionVisible = value; }

    protected internal abstract void CellContent(RenderTreeBuilder builder, TGridItem item);

    internal string GetCssClassOfHeader()
    {
        return AlignHeader switch
        {
            Align.Default => CssClassAndStyle[CssClass.Grid_div_table_thead_tr_th_Default],
            Align.Start => CssClassAndStyle[CssClass.Grid_div_table_thead_tr_td_Start],
            Align.Center => CssClassAndStyle[CssClass.Grid_div_table_thead_tr_td_Center],
            Align.End => CssClassAndStyle[CssClass.Grid_div_table_thead_tr_td_End],
            Align.Left => CssClassAndStyle[CssClass.Grid_div_table_thead_tr_td_Left],
            Align.Right => CssClassAndStyle[CssClass.Grid_div_table_thead_tr_td_Right],
            _ => throw new NotImplementedException(),
        };
    }

    internal string GetCssStyleOfHeader()
    {
        return AlignHeader switch
        {
            Align.Default => CssClassAndStyle[CssStyle.Grid_div_table_thead_tr_th_Default],
            Align.Start => CssClassAndStyle[CssStyle.Grid_div_table_thead_tr_td_Start],
            Align.Center => CssClassAndStyle[CssStyle.Grid_div_table_thead_tr_td_Center],
            Align.End => CssClassAndStyle[CssStyle.Grid_div_table_thead_tr_td_End],
            Align.Left => CssClassAndStyle[CssStyle.Grid_div_table_thead_tr_td_Left],
            Align.Right => CssClassAndStyle[CssStyle.Grid_div_table_thead_tr_td_Right],
            _ => throw new NotImplementedException(),
        };
    }

    internal string GetCssClassOfCell()
    {
        return  AlignCell switch
        {
            Align.Default => CssClassAndStyle[CssClass.Grid_div_table_tbody_tr_td_Default],
            Align.Start => CssClassAndStyle[CssClass.Grid_div_table_tbody_tr_td_Start],
            Align.Center => CssClassAndStyle[CssClass.Grid_div_table_tbody_tr_td_Center],
            Align.End => CssClassAndStyle[CssClass.Grid_div_table_tbody_tr_td_End],
            Align.Left => CssClassAndStyle[CssClass.Grid_div_table_tbody_tr_td_Left],
            Align.Right => CssClassAndStyle[CssClass.Grid_div_table_tbody_tr_td_Right],
            _ => throw new NotImplementedException(),
        };        
    }

    internal string GetCssStyleOfCell()
    {
        return AlignCell switch
        {
            Align.Default => CssClassAndStyle[CssStyle.Grid_div_table_tbody_tr_td_Default],
            Align.Start => CssClassAndStyle[CssStyle.Grid_div_table_tbody_tr_td_Start],
            Align.Center => CssClassAndStyle[CssStyle.Grid_div_table_tbody_tr_td_Center],
            Align.End => CssClassAndStyle[CssStyle.Grid_div_table_tbody_tr_td_End],
            Align.Left => CssClassAndStyle[CssStyle.Grid_div_table_tbody_tr_td_Left],
            Align.Right => CssClassAndStyle[CssStyle.Grid_div_table_tbody_tr_td_Right],
            _ => throw new NotImplementedException(),
        };
    }

    /// <summary>
    /// Adds a column to the grid.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Ajoute une colonne à la grille.
    /// </summary>
    protected void AddColumn()
    {
        Grid.AddColumn(_lastAssignedColumn = this);
    }

    protected void AddSortingInColumn()
    {
        if (_lastAssignedColumn != null && isSortable && propertyExpression != null)
        {
            Grid.ColumnSortDirectionsAdding(_lastAssignedColumn);
        }
    }
    protected void RemoveSortingInColumn()
    {
        Grid.ColumnSortDirectionsRemove(_lastAssignedColumn!);
    }

    /// <summary>
    /// Sorts the grid data.
    /// Updates the list of sorted columns in the grid based on the new sort direction.
    /// Also updates the sort direction in the dictionary of sort directions for each column in the grid.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Trie les données de la grille.
    /// Met à jour la liste <see cref="QuickGridC{TGridItem}.columnsSorted"/> en fonction de la nouvelle direction de tri.
    /// Met également à jour la direction de tri dans le dictionnaire <see cref="QuickGridC{TGridItem}.columnSortDirections"/>.
    /// </summary>
    internal void ApplySort()
    {
        Grid.ApplySort(_lastAssignedColumn!);
    }

    /// <summary>
    /// Sets the property expression and property type for the column.
    /// </summary>
    /// <param name="memberExp">Member expression to use.</param>
    /// <summary xml:lang="fr">
    /// Définit l'expression de propriété et le type de propriété pour la colonne.
    /// </summary>
    /// <param name="memberExp"  xml:lang="fr">Expression de membre à utiliser.</param>
    internal void SetPropertyExpressionAndTypet(MemberExpression memberExp)
    {
        var parameterExp = memberExp.Expression as ParameterExpression;
        propertyExpression = Expression.Lambda<Func<TGridItem, object?>>(Expression.Convert(memberExp, typeof(object)), parameterExp!);
        typeOfProperty = memberExp.Type;
    }

    /// <summary>
    /// Sets the property expression and property type for the column using a lambda expression.
    /// </summary>
    /// <typeparam name="TPro">The type of the property to use.</typeparam>
    /// <param name="expression">The lambda expression representing the property to use.</param>
    /// <summary xml:lang="fr">
    /// Définit l'expression de propriété et le type de propriété pour la colonne en utilisant une expression lambda.
    /// </summary>
    /// <typeparam name="TPro" xml:lang="fr">Le type de la propriété à utiliser.</typeparam>
    /// <param name="expression"  xml:lang="fr">\1L'expression lambda représentant la propriété à utiliser.</param>
    internal void SetPropertyExpressionAndTypet<TPro>(Expression<Func<TGridItem, TPro>> expression)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            SetPropertyExpressionAndTypet(memberExpression);
        }
    }

    /// <summary>
    /// Shows or hides column options.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Affiche ou masque les options de colonne.
    /// </summary>
    private void ToggleColumnOptionsVisibility()
    {
        isOptionVisible = !isOptionVisible;
        Grid.StateChanged();
    }

    /// <summary>
    /// Resolves the CSS class for column options.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Résout la classe CSS pour les options de colonne.
    /// </summary>
    private string GetColumnOptionCssClass()
    {
        if (ColumnOptions != null)
        {
            return optionApplied ? CssClassAndStyle[CssClass.Column_Options_i_i_ColumnOptionActive] :
                CssClassAndStyle[CssClass.Column_Options_i_i_ColumnOptionNotActive];
        }
        else if (hasAdvancedFilterOptions)
        {
            return optionApplied ? CssClassAndStyle[CssClass.Column_Options_i_i_MenuAdvancedFilterActive] :
                CssClassAndStyle[CssClass.Column_Options_i_i_MenuAdvancedFilterNotActive];
        }
        else if (hasFilterOptions)
        {
            return optionApplied ? CssClassAndStyle[CssClass.Column_Options_i_i_MenuFiltreActive] :
                CssClassAndStyle[CssClass.Column_Options_i_i_MenuFiltreNotActive];
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// Resolves the CSS style for column options.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Résout le Style CSS pour les options de colonne.
    /// </summary>
    private string GetColumnOptionCssStyle()
    {
        if (ColumnOptions != null)
        {
            return optionApplied ? CssClassAndStyle[CssStyle.Column_Options_i_i_ColumnOptionActive] :
                CssClassAndStyle[CssStyle.Column_Options_i_i_ColumnOptionNotActive];
        }
        else if (hasAdvancedFilterOptions)
        {
            return optionApplied ? CssClassAndStyle[CssStyle.Column_Options_i_i_MenuAdvancedFilterActive] :
                CssClassAndStyle[CssStyle.Column_Options_i_i_MenuAdvancedFilterNotActive];
        }
        else if (hasFilterOptions)
        {
            return optionApplied ? CssClassAndStyle[CssStyle.Column_Options_i_i_MenuFiltreActive] :
                CssClassAndStyle[CssStyle.Column_Options_i_i_MenuFiltreNotActive];
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// Gets the CSS class corresponding to the sort direction of this column.
    /// Uses the dictionary of sort direction CSS classes to associate each sort direction with its corresponding CSS class.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Obtient la classe CSS correspondant à la direction de tri du cette colonne.
    /// Utilise le dictionnaire <see cref="QuickGridC{TGridItem}.sortDirectionCssClasses"/> pour associer chaque direction de tri à sa classe CSS correspondante.
    /// </summary>
    private string GetSortClass()
    {
        return Grid.GetSortCssClass(_lastAssignedColumn!);
    }

    /// <summary>
    /// Gets the CSS style corresponding to the sort direction of this column.
    /// Uses the dictionary of sort direction CSS classes to associate each sort direction with its corresponding CSS class.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Obtient la Style CSS correspondant à la direction de tri du cette colonne.
    /// Utilise le dictionnaire <see cref="QuickGridC{TGridItem}.sortDirectionCssClasses"/> pour associer chaque direction de tri à sa classe CSS correspondante.
    /// </summary>
    private string GetSortStyle()
    {
        return Grid.GetSortCssStyle(_lastAssignedColumn!);
    }
}
