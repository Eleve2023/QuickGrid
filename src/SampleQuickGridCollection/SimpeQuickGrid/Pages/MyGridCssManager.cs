using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns;
using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.GridCss;
using Microsoft.AspNetCore.Routing;
using System.Reflection.Metadata;

namespace SimpeQuickGrid.Pages
{
    public class MyGridCssManager : GridHtmlCssManager
    {
        protected override Dictionary<ClassHtml, string> InitializedClassCss()
        {
            return new()
            {
                { ClassHtml.Grid_div, "" },
                { ClassHtml.Grid_div_table, "table table-dark table-hover" },
                { ClassHtml.Grid_div_table_thead, "" },
                { ClassHtml.Grid_div_table_thead_tr, "" },
                { ClassHtml.Grid_div_table_thead_tr_th, "" },
                { ClassHtml.Grid_div_table_tbody, "" },
                { ClassHtml.Grid_div_table_tbody_tr, "" },
                { ClassHtml.Grid_div_table_tbody_tr_td, "" },
                { ClassHtml.Column_iSort, "" },
                { ClassHtml.Column_iSort_i_SortAsc, "sorting_ascending" },
                { ClassHtml.Column_iSort_i_SortDesc, "sorting_descending" },
                { ClassHtml.Column_iSort_i_Sortdefault, "sorting_default" },
                { ClassHtml.Column_iSort_i_SortNot, "" },
                { ClassHtml.Column_i, "dropdown" },
                { ClassHtml.Column_i_div_NoShow, "dropdown-content" },
                { ClassHtml.Column_i_div_Show, "dropdown-content show" },
                { ClassHtml.Column_i_i_ColumnOptionActive, "grid-option-Acteved" },
                { ClassHtml.Column_i_i_ColumnOptionNotActive, "grid-option" },
                { ClassHtml.Column_i_i_MenuFiltreActive, "grid-filter" },
                { ClassHtml.Column_i_i_MenuFiltreNotActive, "grid-filter" },
                { ClassHtml.Column_i_i_MenuAdvancedFilterActive, "grid-filter-advanced-Active" },
                { ClassHtml.Column_i_i_MenuAdvancedFilterNotActive, "grid-filter-advanced" },
                { ClassHtml.MenuFiltre_from, "" },
                { ClassHtml.MenuFiltre_from_divInput, "form-control" },
                { ClassHtml.MenuFiltre_from_divInput_selectInputOption, "form-control" },
                { ClassHtml.MenuFiltre_from_divInput_selectInputEnumValue, "form-control" },
                { ClassHtml.MenuFiltre_from_divInput_selectInputBoolValue, "form-control" },
                { ClassHtml.MenuFiltre_from_divInput_inputInputValue, "form-control" },
                { ClassHtml.MenuFiltre_from_divAction, "form-control d-grid gap-2 d-md-block" },
                { ClassHtml.MenuFiltre_from_divAction_buttonOk, "btn btn-primary" },
                { ClassHtml.MenuFiltre_from_divAction_buttonReset, "btn btn-primary" },
                { ClassHtml.MenuAdvancedFilter_div, "d-grid gap-2 d-md-flex justify-content-between" },
                { ClassHtml.MenuAdvancedFilter_div_button_Operator, "btn btn-primary btn-sm" },
                { ClassHtml.MenuAdvancedFilter_div_button_Add, "btn btn-primary btn-sm" },

            };
        }
    }
}
