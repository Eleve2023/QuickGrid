using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Infrastructure
{
    internal class InternalGridContext<TGridItem>
    {
        internal bool IsFirstRender { get; set; } = true;

        public InternalGridContext(QuickGridC<TGridItem> grid)
        {
            Grid = grid;
        }

        public QuickGridC<TGridItem> Grid { get; }
    }
}