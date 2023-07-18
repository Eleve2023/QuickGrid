// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

using Microsoft.AspNetCore.Components;
using Components.QuickGrid.Columns;
using Components.QuickGrid.Infrastructure;
using Components.QuickGrid.QuickGridCollection;
using System.Linq.Expressions;

namespace Components.QuickGrid;

/// This code uses the properties and methods of the Grid<TGridItem> class to display data in an HTML grid.
/// The code starts by defining a CascadingValue to pass the _internalGridContext object to child components. Then, it calls the StartCollectingColumns and FinishCollectingColumns methods to handle collecting columns defined in the child content.
/// The main component code is contained in a Defer element that delays its rendering until the columns are collected. This element contains a div that represents the grid and contains an HTML table with column headers and data cells.
/// The column headers are generated by the RenderColumnHeaders method, which creates a header row for each column in the grid. Each header contains a sorting icon that calls the ApplySort method when clicked, as well as the header content defined by the column.
/// The data cells are generated by the RenderCellText method, which creates a row for each data item in the _currentItems collection. Each row is generated by the RenderRow method, which creates a cell for each column in the grid. Each cell contains the cell content defined by the column for the corresponding data item.

/// Ce code utilise les propriétés et les méthodes de la classe Grid<TGridItem> pour afficher les données dans une grille HTML.
/// Le code commence par définir un CascadingValue pour passer l’objet _internalGridContext aux composants enfants.Ensuite, il appelle les méthodes StartCollectingColumns et FinishCollectingColumns pour gérer la collecte des colonnes définies dans le contenu enfant.    
/// Le code principal du composant est contenu dans un élément Defer qui retarde son rendu jusqu’à ce que les colonnes soient collectées.Cet élément contient une div qui représente la grille et qui contient une table HTML avec des en-têtes de colonne et des cellules de données.
/// Les en-têtes de colonne sont générés par la méthode RenderColumnHeaders, qui crée une ligne d’en-tête pour chaque colonne de la grille.Chaque en-tête contient une icône de tri qui appelle la méthode ApplySort lorsqu’elle est cliquée, ainsi que le contenu d’en-tête défini par la colonne.
/// Les cellules de données sont générées par la méthode RenderCellText, qui crée une ligne pour chaque élément de données dans la collection _currentItems.Chaque ligne est générée par la méthode RenderRow, qui crée une cellule pour chaque colonne de la grille. Chaque cellule contient le contenu de cellule défini par la colonne pour l’élément de données correspondant.

/// <summary>
/// Blazor component representing a generic data grid.
/// Allows displaying, filtering and sorting data of type <see cref="TGridItem"/>.
/// </summary>
/// <typeparam name="TGridItem">The type of data items displayed in the grid.</typeparam>    
/// <summary xml:lang="fr">
/// Composant Blazor représentant une grille de données générique.
/// Permet d'afficher, de filtrer et de trier des données de type <see cref="TGridItem"/>.
/// </summary>
/// <typeparam name="TGridItem" xml:lang="fr">Le type des éléments de données affichés dans la grille.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public partial class QuickGridC<TGridItem> : ComponentBase
{
    /// <summary>
    /// Collection of items currently displayed in the grid.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Collection d'éléments actuellement affichés dans la grille.
    /// </summary>
    private ICollection<TGridItem> _currentItems = Array.Empty<TGridItem>();

    /// <summary>
    /// Last value assigned to the <see cref="QuickGridC{TGridItem}.Items"/> property.
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Dernière valeur assignée à la propriété <see cref="QuickGridC{TGridItem}.Items"/>.
    /// </summary>
    private ICollection<TGridItem>? _lastAssignedItems;
    private bool _collectingColumns;

    /// The <see cref="QuickGridC{TGridItem}.CssClassAndStyle"/> field is an instance of the <see cref="GridHtmlCssManager"/> class that allows managing the CSS classes and styles of the grid's HTML elements. This class contains dictionaries associating each HTML element with its CSS class or style. These dictionaries are initialized in the class constructor with default values.

    /// Le champ <see cref="QuickGridC{TGridItem}.CssClassAndStyle"/> est une instance de la classe <see cref="GridHtmlCssManager"/> qui permet de gérer les classes CSS et les styles 
    /// des éléments HTML de la grille. Cette classe contient des dictionnaires associant chaque élément HTML à sa classe CSS ou à son style. 
    /// Ces dictionnaires sont initialisés dans le constructeur de la classe avec les valeurs par défaut.

    /// <summary>
    /// Object for managing the CSS classes and styles of the grid's HTML elements.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Objet permettant de gérer les classes CSS et les styles des éléments HTML de la grille.
    /// </summary>
    private GridHtmlCssManager cssClassAndStyle = new();

    /// Ces champs sont utilisés pour gérer le tri des colonnes de la grille. 
    /// Lorsqu’une colonne est ajoutée à la grille via la méthode <see cref="QuickGridC{TGridItem}.AddColumn(ColumnCBase{TGridItem})"/>, 
    /// si elle est triable (c’est-à-dire si elle a une expression de propriété et que sa propriété IsSort est vraie), 
    /// elle est ajoutée au dictionnaire <see cref="QuickGridC{TGridItem}.columnSortDirections"/> avec une direction de tri par défaut.
    /// Lorsqu’un utilisateur clique sur l’icône de tri d’une colonne, la méthode Sort <see cref="QuickGridC{TGridItem}.ApplySort(ColumnCBase{TGridItem})"/> est appelée. 
    /// Cette méthode vérifie si la colonne est triable et met à jour les listes <see cref="QuickGridC{TGridItem}.columnsSortedAscending"/> et <see cref="QuickGridC{TGridItem}.columnsSortedDescending"/> en fonction de la nouvelle direction de tri.
    /// Elle met également à jour la direction de tri dans le dictionnaire <see cref="QuickGridC{TGridItem}.columnSortDirections"/>.
    /// La méthode <see cref="QuickGridC{TGridItem}.GetSortCssClass(ColumnCBase{TGridItem})"/>  est utilisée pour obtenir la classe CSS correspondant à la direction de tri d’une colonne. 
    /// Elle utilise le dictionnaire <see cref="QuickGridC{TGridItem}.sortDirectionCssClasses"/> pour associer chaque direction de tri à sa classe CSS correspondante.

    /// These fields are used to manage the sorting of the grid columns.
    /// When a column is added to the grid via the <see cref="QuickGridC{TGridItem}.AddColumn(ColumnCBase{TGridItem})"/> method,
    /// if it is sortable (i.e. it has a property expression and its IsSort property is true),
    /// it is added to the <see cref="QuickGridC{TGridItem}.columnSortDirections"/> dictionary with a default sort direction.
    /// When a user clicks on a column's sort icon, the Sort <see cref="QuickGridC{TGridItem}.ApplySort(ColumnCBase{TGridItem})"/> method is called.
    /// This method checks if the column is sortable and updates the <see cref="QuickGridC{TGridItem}.columnsSortedAscending"/> and <see cref="QuickGridC{TGridItem}.columnsSortedDescending"/> lists based on the new sort direction.
    /// It also updates the sort direction in the <see cref="QuickGridC{TGridItem}.columnSortDirections"/> dictionary.
    /// The <see cref="QuickGridC{TGridItem}.GetSortCssClass(ColumnCBase{TGridItem})"/> method is used to get the CSS class corresponding to a column's sort direction.
    /// It uses the <see cref="QuickGridC{TGridItem}.sortDirectionCssClasses"/> dictionary to associate each sort direction with its corresponding CSS class.

    /// <summary>
    /// Dictionary associating each sortable column with its current sort direction.
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Dictionnaire associant chaque colonne triable à sa direction de tri actuelle.
    /// </summary>
    private readonly Dictionary<ColumnCBase<TGridItem>, SortDirection> columnSortDirections = new();

    /// <summary>
    /// List of columns to sort
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Liste des colonnes à trier
    /// </summary>
    private readonly List<KeyValuePair<ColumnCBase<TGridItem>, (SortDirection, Expression<Func<TGridItem, object?>>)>> columnsSorted = new();

    /// <summary>
    /// Dictionary associating each sort direction with its corresponding CSS class.
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Dictionnaire associant chaque direction de tri à la classe CSS correspondante.
    /// </summary>
    private readonly Dictionary<SortDirection, string> sortDirectionCssClasses;
    /// <summary>
    /// Dictionary associating each sort direction with its corresponding CSS style.
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Dictionnaire associant chaque direction de tri à la style CSS correspondante.
    /// </summary>
    private readonly Dictionary<SortDirection, string> sortDirectionCssStyles;

    /// <summary>
    /// List of filters applied to the grid data.
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Liste des filtres appliqués aux données de la grille.
    /// </summary>
    private readonly List<KeyValuePair<ColumnCBase<TGridItem>, Expression<Func<TGridItem, bool>>>> columnFilters = new();

    /// <summary>
    /// Object containing filter expressions and sort expressions for each column.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Objet contenant les expressions de filtre et les expressions de tri pour chaque colonne.
    /// </summary>
    private GridFilteringAndSorting<TGridItem> _gridFilteringSorting;

    private readonly List<ColumnCBase<TGridItem>> _columns;
    private readonly RenderFragment _ColumnHeaders;
    private readonly RenderFragment _CellText;
    private readonly InternalGridContext<TGridItem> _internalGridContext;

    public QuickGridC()
    {
        _internalGridContext = new(this);
        _columns = new();
        _ColumnHeaders = RenderColumnHeaders;
        _CellText = RenderCellText;

        _gridFilteringSorting = new();

        sortDirectionCssClasses = new()
        {
            { SortDirection.Ascending, CssClassAndStyle[CssClass.Column_Sort_i_i_SortAsc] },
            { SortDirection.Descending, CssClassAndStyle[CssClass.Column_Sort_i_i_SortDesc] },
            { SortDirection.Default, CssClassAndStyle[CssClass.Column_Sort_i_i_Sortdefault] }
        };
        sortDirectionCssStyles = new()
        {
            { SortDirection.Ascending, CssClassAndStyle[CssStyle.Column_Sort_i_i_SortAsc] },
            { SortDirection.Descending, CssClassAndStyle[CssStyle.Column_Sort_i_i_SortDesc] },
            { SortDirection.Default, CssClassAndStyle[CssStyle.Column_Sort_i_i_Sortdefault] }
        };
    }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Collection of items to display in the grid.
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Collection d'éléments à afficher dans la grille.
    /// </summary>
    [Parameter, EditorRequired] public ICollection<TGridItem> Items { get; set; } = null!;

    /// <summary>
    /// Callback called when a row in the grid is selected.
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Callback appelé lorsqu'une ligne de la grille est sélectionnée.
    /// </summary>
    [Parameter] public EventCallback<TGridItem> RowSelected { get; set; }

    /// The <see cref="QuickGridC{TGridItem}.FilterSortChanged"/> property is a callback of type
    /// EventCallback<GridFilteringAndSorting<TGridItem>> <see cref = "EventCallback{GridFilteringAndSorting{TGridItem}}" />
    /// that is called when a filter or sort is changed. When this callback is invoked, it receives as a parameter
    /// the <see cref="QuickGridC{TGridItem}._gridFilteringSorting"/> object containing the updated filtering and sorting information.

    /// La propriété<see cref="QuickGridC{TGridItem}.FilterSortChanged"/> est un callback de type
    /// EventCallback<GridFilteringAndSorting<TGridItem>> <see cref = "EventCallback{GridFilteringAndSorting{TGridItem}}" />
    /// qui est appelé lorsqu’un filtre ou un tri est modifié.Lorsque ce callback est invoqué, il reçoit en paramètre 
    /// l’objet<see cref="QuickGridC{TGridItem}._gridFilteringSorting"/> contenant les informations de filtrage et de tri mises à jour.

    /// <summary>
    /// Callback called when a filter or sort is changed.        
    /// </summary>
    /// <summary xml:lang="fr">
    /// Callback appelé lorsqu'un filtre ou un tri est modifié.        
    /// </summary>
    [Parameter] public EventCallback<GridFilteringAndSorting<TGridItem>> FilterSortChanged { get; set; }
    [Parameter] public GridHtmlCssManager CssClassAndStyle { get => cssClassAndStyle; set => cssClassAndStyle = value; }

    internal bool IsFirstRender { get; set; } = true;

    protected override Task OnParametersSetAsync()
    {
        var newAssignedIems = Items;
        var dataSourceHasChanged = newAssignedIems != _lastAssignedItems;
        if (dataSourceHasChanged)
        {
            _lastAssignedItems = newAssignedIems;
        }
        return dataSourceHasChanged ? RefreshDataAsync() : Task.CompletedTask;
    }

    /// <summary>
    /// Adds a column to the grid.
    /// If the column is sortable, it is added to the <see cref="QuickGridC{TGridItem}.columnSortDirections"/> dictionary with a default sort direction.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Ajoute une colonne à la grille.
    /// Si la colonne est triable, elle est ajoutée au dictionnaire <see cref="QuickGridC{TGridItem}.columnSortDirections"/> avec une direction de tri par défaut.
    /// </summary>
    internal void AddColumn(ColumnCBase<TGridItem> column)
    {
        if (_collectingColumns)
        {
            ColumnSortDirectionsAdding(column);
            _columns.Add(column);
        }
    }
    internal void ColumnSortDirectionsAdding(ColumnCBase<TGridItem> column)
    {
        if (column.PropertyExpression != null && column.IsSortable)
        {
            columnSortDirections.TryAdd(column, SortDirection.Default);
        }
    }
    internal void ColumnSortDirectionsRemove(ColumnCBase<TGridItem> column)
    {
        columnSortDirections.Remove(column);
    }
    internal void StateChanged() => StateHasChanged();
    private void StartCollectingColumns()
    {
        _collectingColumns = true;
    }

    private void FinishCollectingColumns()
    {
        _collectingColumns = false;
        IsFirstRender = false;
    }

    /// The filter field is a list of key-value pairs associating each filterable column with its filter expression. The AddOrMoveFilter method is used to add or update a filter for a specified column. If the filter expression passed as a parameter is null, the existing filter for this column is removed from the filter list. Otherwise, the existing filter for this column is updated or added to the filter list.

    /// Le champ filter est une liste de paires clé-valeur associant chaque colonne filtrable à son expression de filtre. La méthode AddOrMoveFilter est utilisée pour ajouter ou mettre à jour un filtre pour une colonne spécifiée. Si l’expression de filtre passée en paramètre est nulle, le filtre existant pour cette colonne est supprimé de la liste filter. Sinon, le filtre existant pour cette colonne est mis à jour ou ajouté à la liste filter.

    /// <summary>
    /// Adds or updates a filter for a specified column.
    /// If the filter expression is null, the existing filter for this column is removed.
    /// Otherwise, the existing filter for this column is updated or added to the list of filters.
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Ajoute ou met à jour un filtre pour une colonne spécifiée.
    /// Si l'expression de filtre est nulle, le filtre existant pour cette colonne est supprimé.
    /// Sinon, le filtre existant pour cette colonne est mis à jour ou ajouté à la liste des filtres.
    /// </summary>
    internal async void ApplyColumnFilter(Expression<Func<TGridItem, bool>>? expression, ColumnCBase<TGridItem> column)
    {
        columnFilters.RemoveAll(e => e.Key == column);
        if (expression != null)
        {
            columnFilters.Add(new(column, expression));
        }

        _gridFilteringSorting.FilterExpressions = columnFilters.Select(e => e.Value).ToArray();
        await FilterSortChanged.InvokeAsync(_gridFilteringSorting);
    }

    internal SortDirection? GetSortDirection(ColumnCBase<TGridItem> column)
    {
        var hasValue = columnSortDirections.TryGetValue(column, out SortDirection sortDirection);
        if (hasValue)
        {
            return sortDirection;
        }

        return null;
    }

    /// <summary>
    /// Sorts the grid data based on the specified column.
    /// Updates the <see cref="QuickGridC{TGridItem}.columnsSorted"/> list based on the new sort direction.
    /// Also updates the sort direction in the <see cref="QuickGridC{TGridItem}.columnSortDirections"/> dictionary.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Trie les données de la grille en fonction de la colonne spécifiée.
    /// Met à jour la liste <see cref="QuickGridC{TGridItem}.columnsSorted"/> en fonction de la nouvelle direction de tri.
    /// Met également à jour la direction de tri dans le dictionnaire <see cref="QuickGridC{TGridItem}.columnSortDirections"/>.
    /// </summary>
    internal async void ApplySort(ColumnCBase<TGridItem> column)
    {
        var hasValue = columnSortDirections.TryGetValue(column, out SortDirection sortDirection);
        if (hasValue)
        {
            if (sortDirection == SortDirection.Ascending || sortDirection == SortDirection.Descending)
            {
                columnsSorted.RemoveAll(e => e.Key == column);
            }
            else if (!column.MultipleSortingAllowed)
            {
                columnsSorted.RemoveAll(e => e.Key == column || e.Key != column);
                foreach (var keyValue in columnSortDirections)
                {
                    if (keyValue.Key != column)
                    {
                        columnSortDirections[keyValue.Key] = SortDirection.Default;
                    }
                }
            }
            var newSortDirection = sortDirection switch
            {
                SortDirection.Default => SortDirection.Ascending,
                SortDirection.Ascending => SortDirection.Descending,
                SortDirection.Descending => SortDirection.Default,
                _ => throw new NotSupportedException($"Unknown sort direction {sortDirection}"),
            };
            if (newSortDirection == SortDirection.Ascending)
            {
                columnsSorted.Add(new(column, (SortDirection.Ascending, column.PropertyExpression!)));
            }
            else if (newSortDirection == SortDirection.Descending)
            {
                columnsSorted.Add(new(column, (SortDirection.Descending, column.PropertyExpression!)));
            }
            columnSortDirections[column] = newSortDirection;
        }
        else
        {
            throw new QuickGridCException();
        }

        _gridFilteringSorting.SortExpressions = columnsSorted.Select((e, index) =>
        {
            (var sort, Expression<Func<TGridItem, object?>> exp) = e.Value;
            if (index == 0)
            {
                if (sort == SortDirection.Ascending)
                {
                    return (SortedLinq.OrderBy, exp);
                }
                else
                {
                    return (SortedLinq.OrderByDescending, exp);
                }
            }
            else
            {
                if (sort == SortDirection.Ascending)
                {
                    return (SortedLinq.ThenBy, exp);
                }
                else
                {
                    return (SortedLinq.ThenByDescending, exp);
                }
            }
        }).ToArray();

        await FilterSortChanged.InvokeAsync(_gridFilteringSorting);

    }

    /// <summary>
    /// Gets the CSS class corresponding to the sort direction of a column.
    /// Uses the <see cref="QuickGridC{TGridItem}.sortDirectionCssClasses"/> dictionary to associate each sort direction with its corresponding CSS class.
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Obtient la classe CSS correspondant à la direction de tri d'une colonne.
    /// Utilise le dictionnaire <see cref="QuickGridC{TGridItem}.sortDirectionCssClasses"/> pour associer chaque direction de tri à sa classe CSS correspondante.
    /// </summary>
    internal string GetSortCssClass(ColumnCBase<TGridItem> column)
    {
        if (GetSortDirection(column) is SortDirection sortDirection)
        {
            return sortDirectionCssClasses[sortDirection];
        }
        return CssClassAndStyle[CssClass.Column_Sort_i_i_SortNot];
    }

    /// <summary>
    /// Gets the CSS Style corresponding to the sort direction of a column.
    /// Uses the <see cref="QuickGridC{TGridItem}.sortDirectionCssClasses"/> dictionary to associate each sort direction with its corresponding CSS class.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Obtient la Style CSS correspondant à la direction de tri d'une colonne.
    /// Utilise le dictionnaire <see cref="QuickGridC{TGridItem}.sortDirectionCssClasses"/> pour associer chaque direction de tri à sa classe CSS correspondante.
    /// </summary>
    internal string GetSortCssStyle(ColumnCBase<TGridItem> column)
    {
        if (GetSortDirection(column) is SortDirection sortDirection)
        {
            return sortDirectionCssStyles[sortDirection];
        }
        return CssClassAndStyle[CssStyle.Column_Sort_i_i_SortNot];
    }

    /// <summary>
    /// Get aria-sort value for html element
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Obtenir la valeur pour élément html aria-sort
    /// </summary>
    /// <param name="column">instance de la colonne</param>
    /// <returns xml:lang="fr">La valeur en <c>string</c> selon étal de la colonne</returns>
    private string AriaSortValue(ColumnCBase<TGridItem> column)
    {
        var hasValue = columnSortDirections.TryGetValue(column, out SortDirection sortDirection);
        if (hasValue)
        {
            return sortDirection switch
            {
                SortDirection.Default => "other",
                _ => sortDirection.ToString().ToLowerInvariant()
            };
        }
        else
        {
            return "none";
        }
    }

    /// <summary>
    /// Invokes the <see cref="QuickGridC{TGridItem}.RowSelected"/> callback with the selected row item.
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Invoque le callback <see cref="QuickGridC{TGridItem}.RowSelected"/> avec l'élément de la ligne sélectionnée.
    /// </summary>
    private void HandleRowSelection(TGridItem item)
    {
        RowSelected.InvokeAsync(item);
    }

    /// <summary>
    /// Updates the items displayed in the grid.
    /// If the last value assigned to the <see cref="QuickGridC{TGridItem}.Items"/> property is not null,
    /// the currently displayed items are updated with this value.
    /// Otherwise, the currently displayed items are set to an empty array.
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Met à jour les éléments affichés dans la grille.
    /// Si la dernière valeur assignée à la propriété <see cref="QuickGridC{TGridItem}.Items"/> n'est pas nulle,
    /// les éléments actuellement affichés sont mis à jour avec cette valeur.
    /// Sinon, les éléments actuellement affichés sont définis sur un tableau vide.
    /// </summary>
    private Task RefreshDataAsync()
    {
        if (_lastAssignedItems != null)
        {
            _currentItems = _lastAssignedItems;
        }
        else
        {
            _currentItems = Array.Empty<TGridItem>();
        }

        return Task.CompletedTask;
    }
}
