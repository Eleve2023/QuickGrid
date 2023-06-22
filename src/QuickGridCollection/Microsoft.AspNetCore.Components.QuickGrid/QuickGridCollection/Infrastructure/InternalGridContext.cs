using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Infrastructure
{
    internal class InternalGridContext<TGridItem>
    {
        internal bool IsFirstRender { get; set; } = true;

        public InternalGridContext(Grid<TGridItem> grid)
        {
            Grid = grid;
        }

        public Grid<TGridItem> Grid { get; }
    }
}