using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns;
using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.GridCss;
using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Infrastructure;
using System.Linq.Expressions;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection
{
    //Ce code utilise les propriétés et les méthodes de la classe Grid<TGridItem> pour afficher les données dans une grille HTML.
    //Le code commence par définir un CascadingValue pour passer l’objet _internalGridContext aux composants enfants.Ensuite, il appelle les méthodes StartCollectingColumns et FinishCollectingColumns pour gérer la collecte des colonnes définies dans le contenu enfant.    
    //Le code principal du composant est contenu dans un élément Defer qui retarde son rendu jusqu’à ce que les colonnes soient collectées.Cet élément contient une div qui représente la grille et qui contient une table HTML avec des en-têtes de colonne et des cellules de données.
    //Les en-têtes de colonne sont générés par la méthode RenderColumnHeaders, qui crée une ligne d’en-tête pour chaque colonne de la grille.Chaque en-tête contient une icône de tri qui appelle la méthode ApplySort lorsqu’elle est cliquée, ainsi que le contenu d’en-tête défini par la colonne.
    //Les cellules de données sont générées par la méthode RenderCellText, qui crée une ligne pour chaque élément de données dans la collection _currentItems.Chaque ligne est générée par la méthode RenderRow, qui crée une cellule pour chaque colonne de la grille. Chaque cellule contient le contenu de cellule défini par la colonne pour l’élément de données correspondant.
    /// <summary>
    /// Composant Blazor représentant une grille de données générique.
    /// Permet d'afficher, de filtrer et de trier des données de type <see cref="TGridItem"/>.
    /// </summary>
    /// <typeparam name="TGridItem">Le type des éléments de données affichés dans la grille.</typeparam>
    [CascadingTypeParameter(nameof(TGridItem))]
    public partial class QuickGridC<TGridItem> : ComponentBase
    {
        /// <summary>
        /// Collection d'éléments actuellement affichés dans la grille.
        /// </summary>
        private ICollection<TGridItem> _currentItems = Array.Empty<TGridItem>();
        /// <summary>
        /// Dernière valeur assignée à la propriété <see cref="QuickGridC{TGridItem}.Items"/>.
        /// </summary>
        private ICollection<TGridItem>? _lastAssignedItems;
        private bool _collectingColumns;


        /// Le champ <see cref="QuickGridC{TGridItem}.ClassAndStyle"/> est une instance de la classe <see cref="GridHtmlCssManager"/> qui permet de gérer les classes CSS et les styles 
        /// des éléments HTML de la grille. Cette classe contient des dictionnaires associant chaque élément HTML à sa classe CSS ou à son style. 
        /// Ces dictionnaires sont initialisés dans le constructeur de la classe avec les valeurs par défaut.
        /// <summary>
        /// Objet permettant de gérer les classes CSS et les styles des éléments HTML de la grille.
        /// </summary>
        private GridHtmlCssManager classAndStyle = new();

        /// Ces champs sont utilisés pour gérer le tri des colonnes de la grille. 
        /// Lorsqu’une colonne est ajoutée à la grille via la méthode <see cref="QuickGridC{TGridItem}.AddColumn(ColumnCBase{TGridItem})"/>, 
        /// si elle est triable (c’est-à-dire si elle a une expression de propriété et que sa propriété IsSort est vraie), 
        /// elle est ajoutée au dictionnaire <see cref="QuickGridC{TGridItem}.columnSortDirections"/> avec une direction de tri par défaut.
        /// Lorsqu’un utilisateur clique sur l’icône de tri d’une colonne, la méthode Sort <see cref="QuickGridC{TGridItem}.ApplySort(ColumnCBase{TGridItem})"/> est appelée. 
        /// Cette méthode vérifie si la colonne est triable et met à jour les listes <see cref="QuickGridC{TGridItem}.columnsSortedAscending"/> et <see cref="QuickGridC{TGridItem}.columnsSortedDescending"/> en fonction de la nouvelle direction de tri.
        /// Elle met également à jour la direction de tri dans le dictionnaire <see cref="QuickGridC{TGridItem}.columnSortDirections"/>.
        /// La méthode <see cref="QuickGridC{TGridItem}.GetSortClass(ColumnCBase{TGridItem})"/>  est utilisée pour obtenir la classe CSS correspondant à la direction de tri d’une colonne. 
        /// Elle utilise le dictionnaire <see cref="QuickGridC{TGridItem}.sortDirectionCssClasses"/> pour associer chaque direction de tri à sa classe CSS correspondante.

        /// <summary>
        /// Dictionnaire associant chaque colonne triable à sa direction de tri actuelle.
        /// </summary>
        private readonly Dictionary<ColumnCBase<TGridItem>, SortDirection> columnSortDirections = new();

        /// <summary>
        /// Liste des colonnes à trier
        /// </summary>
        private readonly List<KeyValuePair<ColumnCBase<TGridItem>, (SortDirection, Expression<Func<TGridItem, object>>)>> columnsSorted = new();
        /// <summary>
        /// Dictionnaire associant chaque direction de tri à la classe CSS correspondante.
        /// </summary>
        private readonly Dictionary<SortDirection, string> sortDirectionCssClasses;

        /// <summary>
        /// Liste des filtres appliqués aux données de la grille.
        /// </summary>
        private readonly List<KeyValuePair<ColumnCBase<TGridItem>, Expression<Func<TGridItem, bool>>>> columnFilters = new();

        /// <summary>
        /// Objet contenant les expressions de filtre et les expressions de tri pour chaque colonne.
        /// </summary>
        private GridFilteringAndSorting<TGridItem> _gridFilteringSorting = new();

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

            sortDirectionCssClasses = new()
            {
                { SortDirection.Ascending, ClassAndStyle[ClassHtml.Grid_div_table_thead_tr_th_i_i_SortAsc] },
                { SortDirection.Descending, ClassAndStyle[ClassHtml.Grid_div_table_thead_tr_th_i_i_SortDesc] },
                { SortDirection.Default, ClassAndStyle[ClassHtml.Grid_div_table_thead_tr_th_i_i_Sortdefault] }
            };
        }

        [Parameter] public RenderFragment? ChildContent { get; set; }
        /// <summary>
        /// Collection d'éléments à afficher dans la grille.
        /// </summary>
        [Parameter, EditorRequired] public ICollection<TGridItem> Items { get; set; } = null!;
        /// <summary>
        /// Callback appelé lorsqu'une ligne de la grille est sélectionnée.
        /// </summary>
        [Parameter] public EventCallback<TGridItem> RowSelected { get; set; }
        /// La propriété <see cref="QuickGridC{TGridItem}.FilterSortChanged"/> est un callback de type
        /// EventCallback<GridFilteringAndSorting<TGridItem>> <see cref="EventCallback{GridFilteringAndSorting{TGridItem}}"/> 
        /// qui est appelé lorsqu’un filtre ou un tri est modifié. Lorsque ce callback est invoqué, il reçoit en paramètre 
        /// l’objet <see cref="QuickGridC{TGridItem}._gridFilteringSorting"/> contenant les informations de filtrage et de tri mises à jour.
        //
        /// <summary>
        /// Callback appelé lorsqu'un filtre ou un tri est modifié.        
        /// </summary>
        [Parameter] public EventCallback<GridFilteringAndSorting<TGridItem>> FilterSortChanged { get; set; }
        [Parameter] public GridHtmlCssManager ClassAndStyle { get => classAndStyle; set => classAndStyle = value; }

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
        /// Ajoute une colonne à la grille.
        /// Si la colonne est triable, elle est ajoutée au dictionnaire <see cref="QuickGridC{TGridItem}.columnSortDirections"/> avec une direction de tri par défaut.
        /// </summary>
        internal void AddColumn(ColumnCBase<TGridItem> column)
        {
            if (_collectingColumns)
            {
                if (column.PropertyExpression != null && column.IsSortable)
                    columnSortDirections.Add(column, SortDirection.Default);
                _columns.Add(column);
            }
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

        // Le champ filter est une liste de paires clé-valeur associant chaque colonne filtrable à son expression de filtre. La méthode AddOrMoveFilter est utilisée pour ajouter ou mettre à jour un filtre pour une colonne spécifiée. Si l’expression de filtre passée en paramètre est nulle, le filtre existant pour cette colonne est supprimé de la liste filter. Sinon, le filtre existant pour cette colonne est mis à jour ou ajouté à la liste filter.

        /// <summary>
        /// Ajoute ou met à jour un filtre pour une colonne spécifiée.
        /// Si l'expression de filtre est nulle, le filtre existant pour cette colonne est supprimé.
        /// Sinon, le filtre existant pour cette colonne est mis à jour ou ajouté à la liste des filtres.
        /// </summary>
        internal async void ApplyColumnFilter(Expression<Func<TGridItem, bool>>? expression, ColumnCBase<TGridItem> column)
        {
            columnFilters.RemoveAll(e => e.Key == column);
            if (expression != null)
                columnFilters.Add(new(column, expression));
            _gridFilteringSorting.Where = columnFilters.Select(e => e.Value).ToArray();
            await FilterSortChanged.InvokeAsync(_gridFilteringSorting);
        }
        /// <summary>
        /// Trie les données de la grille en fonction de la colonne spécifiée.
        /// Met à jour les listes orderBy <see cref="QuickGridC{TGridItem}.columnsSortedAscending"/> et orderByDesc <see cref="QuickGridC{TGridItem}.columnsSortedDescending"/> en fonction de la nouvelle direction de tri.
        /// Met également à jour la direction de tri dans le dictionnaire <see cref="QuickGridC{TGridItem}.columnSortDirections"/>.
        /// </summary>
        private async void ApplySort(ColumnCBase<TGridItem> column)
        {
            var hasValue = columnSortDirections.TryGetValue(column, out SortDirection sortDirection);
            if (hasValue)
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    columnsSorted.RemoveAll(e => e.Key == column);
                }
                else if (sortDirection == SortDirection.Descending)
                {
                    columnsSorted.RemoveAll(e => e.Key == column);
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
            else throw new Exception();
            _gridFilteringSorting.Values = columnsSorted.Select((e, index) =>
            {
                (var sort, Expression<Func<TGridItem, object>> exp) = e.Value;
                if (index == 0)
                {
                    if (sort == SortDirection.Ascending)
                        return (SortedLinq.OrderBy, exp);
                    else
                        return (SortedLinq.OrderByDescending, exp);
                }
                else
                {
                    if (sort == SortDirection.Ascending)
                        return (SortedLinq.ThenBy, exp);
                    else
                        return (SortedLinq.ThenByDescending, exp);
                }
            }).ToArray();

            await FilterSortChanged.InvokeAsync(_gridFilteringSorting);

        }
        /// <summary>
        /// Obtient la classe CSS correspondant à la direction de tri d'une colonne.
        /// Utilise le dictionnaire sortClassValues <see cref="QuickGridC{TGridItem}.sortDirectionCssClasses"/> pour associer chaque direction de tri à sa classe CSS correspondante.
        /// </summary>
        private string GetSortClass(ColumnCBase<TGridItem> column)
        {
            if (column.PropertyExpression != null && column.IsSortable)
            {
                return sortDirectionCssClasses[columnSortDirections[column]];
            }
            return ClassAndStyle[ClassHtml.Grid_div_table_thead_tr_th_i_i_SortNot];
        }
        /// <summary>
        /// Obtient la Style CSS correspondant à la direction de tri d'une colonne.
        /// Utilise le dictionnaire sortClassValues <see cref="QuickGridC{TGridItem}.sortDirectionCssClasses"/> pour associer chaque direction de tri à sa classe CSS correspondante.
        /// </summary>
        private string GetSortStyle(ColumnCBase<TGridItem> column)
        {
            if (column.PropertyExpression != null && column.IsSortable)
            {
                var style = columnSortDirections[column] switch
                {
                    SortDirection.Ascending => ClassAndStyle[StyleCss.Grid_div_table_thead_tr_th_i_i_SortAsc],
                    SortDirection.Descending => ClassAndStyle[StyleCss.Grid_div_table_thead_tr_th_i_i_SortDesc],
                    SortDirection.Default => ClassAndStyle[StyleCss.Grid_div_table_thead_tr_th_i_i_Sortdefault],
                    _ => throw new NotImplementedException(),
                };
                return style;
            }
            return ClassAndStyle[StyleCss.Grid_div_table_thead_tr_th_i_i_SortNot];
        }
        /// <summary>
        /// Obtenir la valeur pour élément html aria-sort
        /// </summary>
        /// <param name="column">instance de la colonne</param>
        /// <returns>La valeur en <c>string</c> selon étal de la colonne</returns>
        private string AriaSortValue(ColumnCBase<TGridItem> column)
        {
            var hasValue = columnSortDirections.TryGetValue(column, out SortDirection sortDirection);
            if (hasValue)
            {
                return sortDirection switch
                {
                    SortDirection.Default => "other",
                    _ => sortDirection.ToString().ToLower()
                };
            }
            else return "none";
        }
        /// <summary>
        /// Invoque le callback <see cref="QuickGridC{TGridItem}.RowSelected"/> avec l'élément de la ligne sélectionnée.
        /// </summary>
        private void HandleRowSelection(TGridItem item)
        {
            RowSelected.InvokeAsync(item);
        }
        /// <summary>
        /// Met à jour les éléments affichés dans la grille.
        /// Si la dernière valeur assignée à la propriété <see cref="QuickGridC{TGridItem}.Items"/> n'est pas nulle,
        /// les éléments actuellement affichés sont mis à jour avec cette valeur.
        /// Sinon, les éléments actuellement affichés sont définis sur un tableau vide.
        /// </summary>
        private Task RefreshDataAsync()
        {
            if (_lastAssignedItems != null)
                _currentItems = _lastAssignedItems;
            else
                _currentItems = Array.Empty<TGridItem>();
            return Task.CompletedTask;
        }
    }
}
