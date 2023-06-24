namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.GridCss
{
    public enum StyleCss
    {
        /// <summary>
        /// La div qui enveloppe table
        /// </summary>
        Grid_div,
        Grid_div_table,
        Grid_div_table_thead,
        Grid_div_table_thead_tr,
        Grid_div_table_thead_tr_th,
        Grid_div_table_tbody,
        Grid_div_table_tbody_tr,
        Grid_div_table_tbody_tr_td,
        Column_iSort,
        Column_iSort_i_SortAsc,
        Column_iSort_i_SortDesc,
        Column_iSort_i_Sortdefault,
        /// <summary>
        /// La colonne n'est pas triable
        /// </summary>
        Column_iSort_i_SortNot,
        Column_i,
        /// <summary>
        /// Ne pas affiche le menu options
        /// </summary>
        Column_i_div_NoShow,
        /// <summary>
        /// Affiche le menu options
        /// </summary>
        Column_i_div_Show,
        /// <summary>
        /// Si les options de la colonne sont applique.
        /// </summary>
        Column_i_i_ColumnOptionActive,
        /// <summary>
        /// Si les options de la colonne ne sont pas applique.
        /// </summary>
        Column_i_i_ColumnOptionNotActive,
        /// <summary>
        /// Si les options de la colonne sont applique.
        /// </summary>
        Column_i_i_MenuFiltreActive,
        /// <summary>
        /// Si les options de la colonne ne sont pas applique.
        /// </summary>
        Column_i_i_MenuFiltreNotActive,
        /// <summary>
        /// Si les options de la colonne sont applique.
        /// </summary>
        Column_i_i_MenuAdvancedFilterActive,
        /// <summary>
        /// Si les options de la colonne ne sont pas applique.
        /// </summary>
        Column_i_i_MenuAdvancedFilterNotActive,
        MenuFiltre_from,
        MenuFiltre_from_divInput,
        MenuFiltre_from_divInput_selectInputOption,
        MenuFiltre_from_divInput_selectInputEnumValue,
        MenuFiltre_from_divInput_selectInputBoolValue,
        MenuFiltre_from_divInput_inputInputValue,
        MenuFiltre_from_divAction,
        MenuFiltre_from_divAction_buttonOk,
        MenuFiltre_from_divAction_buttonReset,
        MenuAdvancedFilter_div,
        MenuAdvancedFilter_div_button_Operator,
        MenuAdvancedFilter_div_button_Add,
    }
}
