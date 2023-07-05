using Microsoft.AspNetCore.Components.Rendering;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns
{
    public class TemplateColumnC<TGridItem> : ColumnCBase<TGridItem>
    {
        private static readonly RenderFragment<TGridItem> EmptyChildContent = _ => builder => { };

        [Parameter] public RenderFragment<TGridItem> ChildContent { get; set; } = EmptyChildContent;

        protected override void OnParametersSet()
        {
            var IsNewProperty = InternalGridContext.Grid.IsFirstRender;
            if (IsNewProperty)
                AddColumn();
        }
        protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
        {
            builder.AddContent(0, ChildContent(item));
        }
    }
}
