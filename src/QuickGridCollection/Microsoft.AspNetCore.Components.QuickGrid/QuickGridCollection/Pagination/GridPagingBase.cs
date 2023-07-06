namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Pagination
{
    public abstract class GridPagingBase : ComponentBase
    {
        /// <summary xml:lang="fr">
        /// Obtient ou définit l'état de la pagination.
        /// </summary>
        [Parameter, EditorRequired]
        public required GridPagingState PaginationState { get; set; }
        /// <summary xml:lang="fr">
        /// Obtient ou définit le nombre total d'éléments.
        /// </summary>
        public int TotalItems { get => PaginationState.TotalItems; set => PaginationState.TotalItems = value; }
        /// <summary xml:lang="fr">
        /// Obtient ou définit le nombre d'éléments par page.
        /// </summary>
        public int ItemsPerPage { get => PaginationState.ItemsPerPage; set => PaginationState.ItemsPerPage = value; }
        /// <summary xml:lang="fr">
        /// Obtient ou définit l'index de la page actuelle.
        /// </summary>
        public int CurrentPage { get => PaginationState.CurrentPage; set => PaginationState.CurrentPage = value; }
        /// <summary xml:lang="fr">
        /// Obtient le nombre total de pages.
        /// </summary>
        public int PageCount { get => PaginationState.PageCount; }
        /// <summary xml:lang="fr">
        /// Obtient une valeur indiquant si la page actuelle a une page précédente.
        /// </summary>
        public bool HasPreviousPage => CurrentPage > 1;
        /// <summary xml:lang="fr">
        /// Obtient une valeur indiquant si la page actuelle a une page suivante.
        /// </summary>
        public bool HasNextPage => CurrentPage < PageCount;

        /// <summary xml:lang="fr">
        /// Navigue vers la page précédente.
        /// </summary>
        public virtual void PreviousPage()
        {
            CurrentPage--;
        }
        /// <summary xml:lang="fr">
        /// Navigue vers la page suivante.
        /// </summary>
        public virtual void NextPage()
        {
            CurrentPage++;
        }
        /// <summary xml:lang="fr">
        /// Navigue vers une page spécifique.
        /// </summary>
        /// <param name="pageIndex" xml:lang="fr">L'index de la page à laquelle naviguer.</param>
        public virtual void GoToPage(int pageIndex)
        {
            if (pageIndex != CurrentPage + 1)
                PaginationState.NextLink = null;
            CurrentPage = pageIndex;
        }


        /// <summary>
        /// Distributes page numbers into an array.
        /// </summary>
        /// <returns> An array of integers representing the distributed page numbers. 0 indicates that there is no page number at that index.</returns>
        
        /// <summary xml:lang="fr">
        /// Répartit les numéros de page dans un tableau.
        /// </summary>
        /// <returns xml:lang="fr"> Un tableau d'entiers représentant les numéros de page répartis. 0 indique qu'il n'y a pas de numéro de page à cet indice.</returns>
        public int[] DistributePages()
        {
            int selectedPage = CurrentPage;
            int pageCount = PageCount;
            int[] result;

            if (pageCount <= 11)
            {
                result = new int[pageCount];
                for (int i = 0; i < pageCount; i++)
                {
                    result[i] = i + 1;
                }
            }
            else
            {
                result = new int[11];
                if (selectedPage <= 5)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        result[i] = i + 1;
                    }
                    result[9] = pageCount - 1;
                    result[10] = pageCount;
                }
                else if (selectedPage >= pageCount - 4)
                {
                    result[0] = 1;
                    result[1] = 2;
                    for (int j = 3; j < 11; j++)
                    {
                        result[j] = pageCount - (10 - j);
                    }
                    result[9] = pageCount - 1;
                    result[10] = pageCount;
                }
                else
                {
                    result[0] = 1;
                    result[10] = pageCount;
                    for (int j = 2; j < 9; j++)
                    {
                        result[j] = selectedPage + (j - 5);
                    }
                }
            }
            return result;
        }

    }
}
