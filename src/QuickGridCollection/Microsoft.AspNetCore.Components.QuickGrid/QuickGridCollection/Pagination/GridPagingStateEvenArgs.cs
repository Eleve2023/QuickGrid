﻿namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Pagination
{
    /// <summary>
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
        /// Obtient ou définit le lien vers la page suivante.
        /// </summary>
        public string? NextLink { get => pagingState.NextLink; set => pagingState.NextLink = value; }
        /// <summary>
        /// Obtient le nombre d'éléments à ignorer pour la page actuelle.
        /// </summary>
        public int Skip => pagingState.Skip;
        /// <summary>
        /// Obtient le nombre d'éléments par page.
        /// </summary>
        public int ItemsPerPage => pagingState.ItemsPerPage;
        /// <summary>
        /// Obtient ou définit le nombre total d'éléments.
        /// </summary>
        public int TotalItems { get => pagingState.TotalItems; set => pagingState.TotalItems = value; }
    }
}
