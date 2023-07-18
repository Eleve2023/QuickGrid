// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Components.QuickGrid.Columns;

namespace Components.QuickGrid;

public class TemplateColumnC<TGridItem> : ColumnCBase<TGridItem>
{
    private static readonly RenderFragment<TGridItem> EmptyChildContent = _ => builder => { };

    [Parameter] public RenderFragment<TGridItem> ChildContent { get; set; } = EmptyChildContent;

    protected override void OnParametersSet()
    {
        var IsNewProperty = InternalGridContext.Grid.IsFirstRender;
        if (IsNewProperty)
        {
            AddColumn();
        }
    }
    protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        builder.AddContent(0, ChildContent(item));
    }
}
