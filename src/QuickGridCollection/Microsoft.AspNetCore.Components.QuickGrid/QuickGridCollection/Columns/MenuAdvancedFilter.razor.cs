using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Infrastructure;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns
{
    public partial class MenuAdvancedFilter<TGridItem> : MenuFiltre<TGridItem>
    {
        /// <summary>
        /// Représente l'index des filtres de la colonne.
        /// </summary>
        private int filterIndex;
        /// <summary>
        /// Affiche ou masque le bouton d'ajout de filtre pour chaque filtre de la colonne.
        /// </summary>
        private readonly List<bool> showAddFilterButton = new() { false };
        /// <summary>
        /// Opérateur utilisé pour agréger les filtres de la colonne <see cref="MenuFiltre{TGridItem}.columnFilterExpressions"/>.
        /// </summary>
        private FilterOperator filterOperator = FilterOperator.And;
        /// <summary>
        /// Obtient le nombre maximum de filtres à appliquer pour cette colonne.
        /// </summary>
        private int maxColumnFilters => Column.MaxFilters;
        /// <summary>
        /// Ajoute un filtre à la liste des filtres de la colonne.
        /// </summary>
        private void AddColumnFilter()
        {
            if (filterIndex < maxColumnFilters - 1)
            {
                columnFilterAdditions++;
                if (true) showAddFilterButton.Add(true);
                filterValues.Add(string.Empty);
            }
        }
        protected override List<Enum> GetListOptionFiltre(int index)
        {
            filterOptions ??= new() { Enum.GetValues(optionsType).Cast<Enum>().ToList() };

            var enumlist = Enum.GetValues(optionsType).Cast<Enum>().ToList();
            //Ajoute option sélectionne par défaut
            if (selectedFilterOptions.Count != columnFilterAdditions + 1)
            {
                var optionfiltervalue = optionsType.Name switch
                {
                    nameof(DataFilterOptions) => DataFilterOptions.NotEqual,
                    nameof(EnumFilterOptions) => enumlist.FirstOrDefault()!,
                    nameof(StringFilterOptions) => StringFilterOptions.Contains,
                    _ => throw new NotImplementedException(),
                };
                selectedFilterOptions.Add(optionfiltervalue);
            }
            List<Enum> optionSelecteds;
            optionSelecteds = selectedFilterOptions.ToList();
            //
            optionSelecteds!.RemoveAt(index);
            if (filterOptions.Count < columnFilterAdditions + 1)
                filterOptions.Add(enumlist);

            if (filterOperator == FilterOperator.And)
            {
                if (optionsType.Name == typeof(DataFilterOptions).Name)
                    ResolveDataFilterOptions(enumlist, optionSelecteds);
                else if (optionsType.Name == typeof(EnumFilterOptions).Name)
                    ResolveEnumFilterOptions(enumlist, optionSelecteds);
                else if (optionsType.Name == typeof(StringFilterOptions).Name)
                    ResolveStringFilterOptions(enumlist, optionSelecteds);
                else
                    throw new NotImplementedException();

                if (selectedFilterOptions[index].ToString() == "Equal")
                    showAddFilterButton[index] = false;
                else
                    showAddFilterButton[index] = true;
            }
            else
                showAddFilterButton[index] = true;

            return filterOptions[index] = enumlist;
        }
        /// <summary>
        /// Résout la liste des options de filtre pour les champs de type chaîne en fonction des options de filtre sélectionnées.
        /// </summary>
        /// <typeparam name="TOption">Le type des options de filtre.</typeparam>
        /// <param name="enumlist">La liste des options de filtre à résoudre.</param>
        /// <param name="optionSelecteds">La liste des options de filtre sélectionnées.</param>
        private void ResolveStringFilterOptions<TOption>(List<TOption> enumlist, List<Enum> optionSelecteds)
        {
            if (optionSelecteds.Contains(StringFilterOptions.StartsWith))
                enumlist.RemoveAll(x => x is StringFilterOptions.StartsWith or StringFilterOptions.Equal);

            if (optionSelecteds.Contains(StringFilterOptions.EndsWith))
                enumlist.RemoveAll(x => x is StringFilterOptions.EndsWith or StringFilterOptions.Equal);

            if (optionSelecteds.Any(e => e is StringFilterOptions.Contains or StringFilterOptions.NotEqual))
                enumlist.RemoveAll(x => x is StringFilterOptions.Equal);

            if (optionSelecteds.Contains(StringFilterOptions.Equal))
                enumlist.RemoveAll(x =>
                    x is StringFilterOptions.Contains
                    or StringFilterOptions.StartsWith
                    or StringFilterOptions.EndsWith
                    or StringFilterOptions.Equal
                    or StringFilterOptions.NotEqual);
        }
        /// <summary>
        /// Résout la liste des options de filtre pour les champs de type énumération en fonction des options de filtre sélectionnées.
        /// </summary>
        /// <typeparam name="TOption">Le type des options de filtre.</typeparam>
        /// <param name="enumlist">La liste des options de filtre à résoudre.</param>
        /// <param name="optionSelecteds">La liste des options de filtre sélectionnées.</param>
        private void ResolveEnumFilterOptions<TOption>(List<TOption> enumlist, List<Enum> optionSelecteds)
        {
            if (optionSelecteds.Contains(EnumFilterOptions.Equal))
                enumlist.RemoveAll(x => x is EnumFilterOptions.NotEqual);

            if (optionSelecteds.Contains(EnumFilterOptions.NotEqual))
                enumlist.RemoveAll(x => x is EnumFilterOptions.Equal);
        }
        /// <summary>
        /// Résout la liste des options de filtre pour les champs de type données en fonction des options de filtre sélectionnées.
        /// </summary>
        /// <param name="enumlist">La liste des options de filtre à résoudre.</param>
        /// <param name="optionSelecteds">La liste des options de filtre sélectionnées.</param>
        private void ResolveDataFilterOptions(List<Enum> enumlist, List<Enum> optionSelecteds)
        {
            if (optionSelecteds.Contains(DataFilterOptions.Equal))
                enumlist.RemoveAll(x =>
                x is DataFilterOptions.NotEqual
                or DataFilterOptions.GreaterThan
                or DataFilterOptions.GreaterThanOrEqual
                or DataFilterOptions.LessThan
                or DataFilterOptions.LessThanOrEqual
                );
            if (optionSelecteds.Contains(DataFilterOptions.GreaterThan))
                enumlist.RemoveAll(x => x is DataFilterOptions.GreaterThanOrEqual
                                        or DataFilterOptions.Equal
                                        or DataFilterOptions.GreaterThan
                );
            if (optionSelecteds.Contains(DataFilterOptions.GreaterThanOrEqual))
                enumlist.RemoveAll(x => x is DataFilterOptions.GreaterThan
                                        or DataFilterOptions.Equal
                                        or DataFilterOptions.GreaterThanOrEqual
                );
            if (optionSelecteds.Contains(DataFilterOptions.LessThan))
                enumlist.RemoveAll(x => x is DataFilterOptions.LessThanOrEqual
                                        or DataFilterOptions.Equal
                                        or DataFilterOptions.LessThan
                );
            if (optionSelecteds.Contains(DataFilterOptions.LessThanOrEqual))
                enumlist.RemoveAll(x => x is DataFilterOptions.LessThan
                                        or DataFilterOptions.Equal
                                        or DataFilterOptions.LessThanOrEqual
                );
            if (optionSelecteds.Contains(DataFilterOptions.NotEqual))
                enumlist.RemoveAll(x => x is DataFilterOptions.Equal);
        }

        protected override void ApplyColumnFilterFromGrid()
        {
            if (columnFilterExpressions != null)
            {
                var parameter = Expression.Parameter(typeof(TGridItem), "x");
                var replacedExpressions = columnFilterExpressions.Select(e => ((Expression<Func<TGridItem, bool>>)ParameterReplacer.Replace(e, e.Parameters[0], parameter)).Body);
                Expression expression;
                if (filterOperator == FilterOperator.And)
                {
                    expression = replacedExpressions.Aggregate(Expression.AndAlso);
                }
                else if (filterOperator == FilterOperator.Or)
                    expression = replacedExpressions.Aggregate(Expression.Or);
                else throw new Exception();

                var lambda = Expression.Lambda<Func<TGridItem, bool>>(expression, parameter);
                if (lambda != null)
                    Grid.ApplyColumnFilter(lambda, Column);
            }
        }
        // todo : remplace le bouton par select
        /// <summary>
        /// Bascule entre les valeurs de l'opérateur de filtre pour assigner la propriété <see cref="MenuAdvancedFilter{TGridItem}.filterOperator"/>.
        /// </summary>
        /// <exception cref="NotImplementedException">Lancée si la valeur de l'opérateur de filtre n'est pas gérée.</exception>
        private void ToggleFilterOperator()
        {
            bool reset;
            (filterOperator, reset) = filterOperator switch
            {
                FilterOperator.And => (FilterOperator.Or, false),
                FilterOperator.Or => (FilterOperator.And, true),
                _ => throw new NotImplementedException()
            };
            if (reset)
                ResetColumnFilters();
        }
    }
}
