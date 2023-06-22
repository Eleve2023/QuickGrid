namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection
{
    /// <summary>
    /// Représente l'état de la pagination d'une grille.
    /// </summary>
    public class GridPagingState
    {
        private int pageIndex = 1;
        private int itemsPerPage = 20;

        public GridPagingState()
        {

        }
        /// <summary>
        /// Obtient ou définit le nombre total d'éléments.
        /// </summary>
        public int TotalItems { get; set; }
        /// <summary>
        /// Obtient ou définit le nombre d'éléments par page.
        /// </summary>
        public int ItemsPerPage { get => itemsPerPage; set => itemsPerPage = value; }
        /// <summary>
        /// Obtient ou définit l'index de la page actuelle.
        /// </summary>
        public int CurrentPage { get => pageIndex; set { pageIndex = value; OnPageChanged(); } }
        /// <summary>
        /// Obtient le nombre d'éléments à ignorer pour la page actuelle.
        /// </summary>
        public int Skip => (CurrentPage - 1) * ItemsPerPage;
        /// <summary>
        /// Obtient ou définit le lien vers la page suivante.
        /// </summary>
        public string? NextLink { get; set; }
        /// <summary>
        /// Obtient le nombre total de pages.
        /// </summary>
        public int PageCount { get => (int)Math.Ceiling(TotalItems / (decimal)ItemsPerPage); }

        /// <summary>
        /// Se produit lorsque l'index de la page actuelle change.
        /// </summary>
        public event EventHandler<GridPageChangedEventArgs> PageChanged = default!;

        /// <summary>
        /// Déclenche l'événement <see cref="PageChanged"/>.
        /// </summary>
        private void OnPageChanged()
        {
            PageChanged.Invoke(this, new(this));
        }

    }
}
