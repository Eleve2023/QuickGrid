namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Infrastructure
{
    internal class InternalGridContext<TGridItem>
    {
        public InternalGridContext(QuickGridC<TGridItem> grid)
        {
            Grid = grid;
        }

        public QuickGridC<TGridItem> Grid { get; }
    }
}