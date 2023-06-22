namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Infrastructure
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
        Grid_div_table_thead_tr_th_i,
        Grid_div_table_thead_tr_th_i_i_SortAsc,
        Grid_div_table_thead_tr_th_i_i_SortDesc,
        Grid_div_table_thead_tr_th_i_i_Sortdefault,
        /// <summary>
        /// La colonne n'est pas triable
        /// </summary>
        Grid_div_table_thead_tr_th_i_i_SortNot,
        Grid_div_table_tbody,
        Grid_div_table_tbody_tr,
        Grid_div_table_tbody_tr_td,
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
        Column_i_i_ColumnFilterActive,
        /// <summary>
        /// Si les options de la colonne ne sont pas applique.
        /// </summary>
        Column_i_i_ColumnFilterNotActive,
        /// <summary>
        /// Si les options de la colonne sont applique.
        /// </summary>
        Column_i_i_ColumnFilterAdvenceActive,
        /// <summary>
        /// Si les options de la colonne ne sont pas applique.
        /// </summary>
        Column_i_i_ColumnFilterAdvenceNotActive,
        ColumnFiltre_from,
        ColumnFiltre_from_divInput,
        ColumnFiltre_from_divInput_selectInputOption,
        ColumnFiltre_from_divInput_selectInputEnumValue,
        ColumnFiltre_from_divInput_inputInputValue,
        ColumnFiltre_from_divAction,
        ColumnFiltre_from_divAction_buttonOk,
        ColumnFiltre_from_divAction_buttonReset,
        ColumnFiltreAdenced_div,
        ColumnFiltreAdenced_div_button_Operator,
        ColumnFiltreAdenced_div_button_Add,
    }
}
