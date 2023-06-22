using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Infrastructure
{
    /// <summary>
    /// Classe permettant de gérer les classes CSS et les styles des éléments HTML de la grille <see cref="Grid{TGridItem}"/>.
    /// Contient des dictionnaires associant chaque élément HTML à sa classe CSS ou à son style.
    /// </summary>
    public class GridHtmlCssManager
    {
        private readonly Dictionary<ClassHtml, string> classHtmls;
        private readonly Dictionary<StyleCss, string> styleCsss;

        public GridHtmlCssManager()
        {
            classHtmls = InitializedClassCss();
            styleCsss = InitializedStyleCss();
        }

        public string this[ClassHtml classHtml] { get => classHtmls[classHtml]; set => classHtmls[classHtml] = value; }
        public string this[StyleCss styleCss] { get => styleCsss[styleCss]; set => styleCsss[styleCss] = value; }

        /// <summary>
        /// Initialized
        /// </summary>
        protected virtual Dictionary<ClassHtml, string> InitializedClassCss()
        {
            return new()
            {
                { ClassHtml.Grid_div, "" },
                { ClassHtml.Grid_div_table, "table" },
                { ClassHtml.Grid_div_table_thead, "" },
                { ClassHtml.Grid_div_table_thead_tr, "" },
                { ClassHtml.Grid_div_table_thead_tr_th, "" },
                { ClassHtml.Grid_div_table_thead_tr_th_i, "" },
                { ClassHtml.Grid_div_table_thead_tr_th_i_i_SortAsc, "sorting_ascending" },
                { ClassHtml.Grid_div_table_thead_tr_th_i_i_SortDesc, "sorting_descending" },
                { ClassHtml.Grid_div_table_thead_tr_th_i_i_Sortdefault, "sorting_default" },
                { ClassHtml.Grid_div_table_thead_tr_th_i_i_SortNot, "" },
                { ClassHtml.Grid_div_table_tbody, "" },
                { ClassHtml.Grid_div_table_tbody_tr, "" },
                { ClassHtml.Grid_div_table_tbody_tr_td, "" },
                { ClassHtml.Column_i, "dropdown" },
                { ClassHtml.Column_i_div_NoShow, "dropdown-content" },
                { ClassHtml.Column_i_div_Show, "dropdown-content show" },
                { ClassHtml.Column_i_i_ColumnOptionActive, "grid-option-Acteved" },
                { ClassHtml.Column_i_i_ColumnOptionNotActive, "grid-option" },
                { ClassHtml.Column_i_i_ColumnFilterActive, "grid-filter" },
                { ClassHtml.Column_i_i_ColumnFilterNotActive, "grid-filter" },
                { ClassHtml.Column_i_i_ColumnFilterAdvenceActive, "grid-filter-advanced-Active" },
                { ClassHtml.Column_i_i_ColumnFilterAdvenceNotActive, "grid-filter-advanced" },
                { ClassHtml.ColumnFiltre_from, "" },
                { ClassHtml.ColumnFiltre_from_divInput, "form-control" },
                { ClassHtml.ColumnFiltre_from_divInput_selectInputOption, "form-control" },
                { ClassHtml.ColumnFiltre_from_divInput_selectInputEnumValue, "form-control" },
                { ClassHtml.ColumnFiltre_from_divInput_inputInputValue, "form-control" },
                { ClassHtml.ColumnFiltre_from_divAction, "form-control d-grid gap-2 d-md-block" },
                { ClassHtml.ColumnFiltre_from_divAction_buttonOk, "btn btn-primary" },
                { ClassHtml.ColumnFiltre_from_divAction_buttonReset, "btn btn-primary" },
                { ClassHtml.ColumnFiltreAdenced_div, "d-grid gap-2 d-md-flex justify-content-between" },
                { ClassHtml.ColumnFiltreAdenced_div_button_Operator, "btn btn-primary btn-sm" },
                { ClassHtml.ColumnFiltreAdenced_div_button_Add, "btn btn-primary btn-sm" },

            };
        }

        /// <summary>
        /// Initialized
        /// </summary>
        protected virtual Dictionary<StyleCss, string> InitializedStyleCss()
        {
            return new()
            {
                { StyleCss.Grid_div, "" },
                { StyleCss.Grid_div_table, "" },
                { StyleCss.Grid_div_table_thead, "" },
                { StyleCss.Grid_div_table_thead_tr, "" },
                { StyleCss.Grid_div_table_thead_tr_th, "" },
                { StyleCss.Grid_div_table_thead_tr_th_i, "float:right" },
                { StyleCss.Grid_div_table_thead_tr_th_i_i_SortAsc, "" },
                { StyleCss.Grid_div_table_thead_tr_th_i_i_SortDesc, "" },
                { StyleCss.Grid_div_table_thead_tr_th_i_i_Sortdefault, "" },
                { StyleCss.Grid_div_table_thead_tr_th_i_i_SortNot, "" },
                { StyleCss.Grid_div_table_tbody, "" },
                { StyleCss.Grid_div_table_tbody_tr, "" },
                { StyleCss.Grid_div_table_tbody_tr_td, "" },
                { StyleCss.Column_i, "" },
                { StyleCss.Column_i_div_NoShow, "" },
                { StyleCss.Column_i_div_Show, "" },
                { StyleCss.Column_i_i_ColumnOptionActive, "" },
                { StyleCss.Column_i_i_ColumnOptionNotActive, "" },
                { StyleCss.Column_i_i_ColumnFilterActive, "" },
                { StyleCss.Column_i_i_ColumnFilterNotActive, "" },
                { StyleCss.Column_i_i_ColumnFilterAdvenceActive, "" },
                { StyleCss.Column_i_i_ColumnFilterAdvenceNotActive, "" },
                { StyleCss.ColumnFiltre_from, "" },
                { StyleCss.ColumnFiltre_from_divInput, "width: max-content;" },
                { StyleCss.ColumnFiltre_from_divInput_selectInputOption, "" },
                { StyleCss.ColumnFiltre_from_divInput_selectInputEnumValue, "" },
                { StyleCss.ColumnFiltre_from_divInput_inputInputValue, "" },
                { StyleCss.ColumnFiltre_from_divAction, "" },
                { StyleCss.ColumnFiltre_from_divAction_buttonOk, "" },
                { StyleCss.ColumnFiltre_from_divAction_buttonReset, "" },
                { StyleCss.ColumnFiltreAdenced_div, "" },
                { StyleCss.ColumnFiltreAdenced_div_button_Operator, "" },
                { StyleCss.ColumnFiltreAdenced_div_button_Add, "" },
            };
        }
    }
}
