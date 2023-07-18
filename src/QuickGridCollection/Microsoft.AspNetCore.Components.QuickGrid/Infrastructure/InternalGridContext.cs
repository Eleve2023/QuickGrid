// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

namespace Components.QuickGrid.Infrastructure;

internal sealed class InternalGridContext<TGridItem>
{
    public InternalGridContext(QuickGridC<TGridItem> grid)
    {
        Grid = grid;
    }

    public QuickGridC<TGridItem> Grid { get; }
}
