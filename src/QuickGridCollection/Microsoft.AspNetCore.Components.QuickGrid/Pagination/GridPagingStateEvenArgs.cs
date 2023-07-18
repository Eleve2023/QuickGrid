// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

namespace Components.QuickGrid;

/// <summary>
/// Provides data for the <see cref="GridPagingState.PageChanged"/> event.
/// </summary> 
/// <summary xml:lang="fr">
/// Fournit des données pour l'événement <see cref="GridPagingState.PageChanged"/>.
/// </summary>
public class GridPageChangedEventArgs : EventArgs
{
    private readonly GridPagingState pagingState;

    public GridPageChangedEventArgs(GridPagingState pagingState)
    {
        this.pagingState = pagingState;
    }

    /// <summary>
    /// Gets or sets the link to the next page.
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Obtient ou définit le lien vers la page suivante.
    /// </summary>
    public string? NextLink { get => pagingState.NextLink; set => pagingState.NextLink = value; }

    /// <summary>
    /// Gets the number of items to skip for the current page.
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Obtient le nombre d'éléments à ignorer pour la page actuelle.
    /// </summary>
    public int Skip => pagingState.Skip;

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Obtient le nombre d'éléments par page.
    /// </summary>
    public int ItemsPerPage => pagingState.ItemsPerPage;

    /// <summary>
    /// Gets or sets the total number of items.
    /// </summary> 
    /// <summary xml:lang="fr">
    /// Obtient ou définit le nombre total d'éléments.
    /// </summary>
    public int TotalItems { get => pagingState.TotalItems; set => pagingState.TotalItems = value; }
}
