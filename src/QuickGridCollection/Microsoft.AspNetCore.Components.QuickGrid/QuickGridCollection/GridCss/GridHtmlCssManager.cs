namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.GridCss
{
    /// <summary>
    /// Classe permettant de gérer les classes CSS et les styles des éléments HTML de la grille <see cref="QuickGridC{TGridItem}"/>.
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
                { StyleCss.Grid_div_table_tbody, "" },
                { StyleCss.Grid_div_table_tbody_tr, "" },
                { StyleCss.Grid_div_table_tbody_tr_td, "" },
                { StyleCss.Column_iSort, "float:right" },
                { StyleCss.Column_iSort_i_SortAsc, "" },
                { StyleCss.Column_iSort_i_SortDesc, "" },
                { StyleCss.Column_iSort_i_Sortdefault, "" },
                { StyleCss.Column_iSort_i_SortNot, "" },
                { StyleCss.Column_i, "" },
                { StyleCss.Column_i_div_NoShow, "" },
                { StyleCss.Column_i_div_Show, "" },
                { StyleCss.Column_i_i_ColumnOptionActive, "" },
                { StyleCss.Column_i_i_ColumnOptionNotActive, "" },
                { StyleCss.Column_i_i_MenuFiltreActive, "" },
                { StyleCss.Column_i_i_MenuFiltreNotActive, "" },
                { StyleCss.Column_i_i_MenuAdvancedFilterActive, "" },
                { StyleCss.Column_i_i_MenuAdvancedFilterNotActive, "" },
                { StyleCss.MenuFiltre_from, "" },
                { StyleCss.MenuFiltre_from_divInput, "width: max-content;" },
                { StyleCss.MenuFiltre_from_divInput_selectInputOption, "" },
                { StyleCss.MenuFiltre_from_divInput_selectInputEnumValue, "" },
                { StyleCss.MenuFiltre_from_divInput_selectInputBoolValue, "" },
                { StyleCss.MenuFiltre_from_divInput_inputInputValue, "" },
                { StyleCss.MenuFiltre_from_divAction, "" },
                { StyleCss.MenuFiltre_from_divAction_buttonOk, "" },
                { StyleCss.MenuFiltre_from_divAction_buttonReset, "" },
                { StyleCss.MenuAdvancedFilter_div, "" },
                { StyleCss.MenuAdvancedFilter_div_button_Operator, "" },
                { StyleCss.MenuAdvancedFilter_div_button_Add, "" },
            };
        }
    }
}
