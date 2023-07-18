// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

namespace Components.QuickGrid;

/// <summary>
/// Represents the pagination state of a grid.
/// </summary>
/// <summary xml:lang="fr">
/// Représente l'état de la pagination d'une grille.
/// </summary>
public class GridPagingState
{
    private int pageIndex = 1;
    private int itemsPerPage = 20;

    public GridPagingState(int itemsPerPage = 20)
    {
        this.itemsPerPage = itemsPerPage;
    }

    /// <summary>
    /// Gets or sets the total number of items.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Obtient ou définit le nombre total d'éléments.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Obtient ou définit le nombre d'éléments par page.
    /// </summary>
    public int ItemsPerPage { get => itemsPerPage; set => itemsPerPage = value; }

    /// <summary>
    /// Gets or sets the index of the current page.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Obtient ou définit l'index de la page actuelle.
    /// </summary>
    public int CurrentPage { get => pageIndex; set { pageIndex = value; OnPageChanged(); } }

    /// <summary>
    /// Gets the number of items to skip for the current page.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Obtient le nombre d'éléments à ignorer pour la page actuelle.
    /// </summary>
    public int Skip => (CurrentPage - 1) * ItemsPerPage;

    /// <summary>
    /// Gets or sets the link to the next page.
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Obtient ou définit le lien vers la page suivante.
    /// </summary>
    public string? NextLink { get; set; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Obtient le nombre total de pages.
    /// </summary>
    public int PageCount { get => (int)Math.Ceiling(TotalItems / (decimal)ItemsPerPage); }

    /// <summary>
    /// Occurs when the index of the current page changes.
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Se produit lorsque l'index de la page actuelle change.
    /// </summary>
    public event EventHandler<GridPageChangedEventArgs> PageChanged = default!;

    /// <summary>
    /// Raises the <see cref="PageChanged"/> event.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Déclenche l'événement <see cref="PageChanged"/>.
    /// </summary>
    private void OnPageChanged()
    {
        PageChanged.Invoke(this, new(this));
    }

}
