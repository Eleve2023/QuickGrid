using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Infrastructure;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns
{
    public partial class MenuAdvancedFilter<TGridItem> : MenuFiltre<TGridItem>
    {
        /// <summary>
        /// représente l'index des filtres de la colonne
        /// </summary>
        private int index;
        /// <summary>
        /// Affiche ou masque le bouton ajoute des filtre 
        /// </summary>
        private readonly List<bool> showButtonAdd = new() { false };
        /// <summary>
        /// Quelle opérateur a utilise pour aggregation des filtres de la colonne <see cref="MenuFiltre{TGridItem}.columnFilterExpressions"/>.
        /// </summary>
        private OperatorFilter operatorFilter = OperatorFilter.And;
        /// <summary>
        /// Nombre maximum de filtres à applique pour cette colonne.
        /// </summary>
        private int MaxFilters => Column.MaxFilters;

        private void AddOptionFilter()
        {
            if (index < MaxFilters - 1)
            {
                columnFilterAdditions++;
                if (true) showButtonAdd.Add(true);
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
                    nameof(OptionFiltreData) => OptionFiltreData.NotEqual,
                    nameof(OptionFiltreEnum) => enumlist.FirstOrDefault()!,
                    nameof(OptionFiltreString) => OptionFiltreString.Contains,
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

            if (operatorFilter == OperatorFilter.And)
            {
                if (optionsType.Name == typeof(OptionFiltreData).Name)
                    ResouvedListDataoptions(enumlist, optionSelecteds);
                else if (optionsType.Name == typeof(OptionFiltreEnum).Name)
                    ResouvedListEnumoptions(enumlist, optionSelecteds);
                else if (optionsType.Name == typeof(OptionFiltreString).Name)
                    ResouvedListStringoptions(enumlist, optionSelecteds);
                else
                    throw new NotImplementedException();

                if (selectedFilterOptions[index].ToString() == "Equal")
                    showButtonAdd[index] = false;
                else
                    showButtonAdd[index] = true;
            }
            else
                showButtonAdd[index] = true;

            return filterOptions[index] = enumlist;
        }

        private void ResouvedListStringoptions<TOption>(List<TOption> enumlist, List<Enum> optionSelecteds)
        {
            if (optionSelecteds.Contains(OptionFiltreString.StartsWith))
                enumlist.RemoveAll(x => x is OptionFiltreString.StartsWith or OptionFiltreString.Equal);

            if (optionSelecteds.Contains(OptionFiltreString.EndsWith))
                enumlist.RemoveAll(x => x is OptionFiltreString.EndsWith or OptionFiltreString.Equal);

            if (optionSelecteds.Any(e => e is OptionFiltreString.Contains or OptionFiltreString.NotEqual))
                enumlist.RemoveAll(x => x is OptionFiltreString.Equal);

            if (optionSelecteds.Contains(OptionFiltreString.Equal))
                enumlist.RemoveAll(x =>
                    x is OptionFiltreString.Contains
                    or OptionFiltreString.StartsWith
                    or OptionFiltreString.EndsWith
                    or OptionFiltreString.Equal
                    or OptionFiltreString.NotEqual);
        }

        private void ResouvedListEnumoptions<TOption>(List<TOption> enumlist, List<Enum> optionSelecteds)
        {
            if (optionSelecteds.Contains(OptionFiltreEnum.Equal))
                enumlist.RemoveAll(x => x is OptionFiltreEnum.NotEqual);

            if (optionSelecteds.Contains(OptionFiltreEnum.NotEqual))
                enumlist.RemoveAll(x => x is OptionFiltreEnum.Equal);
        }

        private void ResouvedListDataoptions(List<Enum> enumlist, List<Enum> optionSelecteds)
        {
            if (optionSelecteds.Contains(OptionFiltreData.Equal))
                enumlist.RemoveAll(x =>
                x is OptionFiltreData.NotEqual
                or OptionFiltreData.GreaterThan
                or OptionFiltreData.GreaterThanOrEqual
                or OptionFiltreData.LessThan
                or OptionFiltreData.LessThanOrEqual
                );
            if (optionSelecteds.Contains(OptionFiltreData.GreaterThan))
                enumlist.RemoveAll(x => x is OptionFiltreData.GreaterThanOrEqual
                                        or OptionFiltreData.Equal
                                        or OptionFiltreData.GreaterThan
                );
            if (optionSelecteds.Contains(OptionFiltreData.GreaterThanOrEqual))
                enumlist.RemoveAll(x => x is OptionFiltreData.GreaterThan
                                        or OptionFiltreData.Equal
                                        or OptionFiltreData.GreaterThanOrEqual
                );
            if (optionSelecteds.Contains(OptionFiltreData.LessThan))
                enumlist.RemoveAll(x => x is OptionFiltreData.LessThanOrEqual
                                        or OptionFiltreData.Equal
                                        or OptionFiltreData.LessThan
                );
            if (optionSelecteds.Contains(OptionFiltreData.LessThanOrEqual))
                enumlist.RemoveAll(x => x is OptionFiltreData.LessThan
                                        or OptionFiltreData.Equal
                                        or OptionFiltreData.LessThanOrEqual
                );
            if (optionSelecteds.Contains(OptionFiltreData.NotEqual))
                enumlist.RemoveAll(x => x is OptionFiltreData.Equal);
        }

        protected override void ApplyColumnFilterFromGrid()
        {
            if (columnFilterExpressions != null)
            {
                var parameter = Expression.Parameter(typeof(TGridItem), "x");
                var replacedExpressions = columnFilterExpressions.Select(e => ((Expression<Func<TGridItem, bool>>)ParameterReplacer.Replace(e, e.Parameters[0], parameter)).Body);
                Expression expression;
                if (operatorFilter == OperatorFilter.And)
                {
                    expression = replacedExpressions.Aggregate(Expression.AndAlso);
                }
                else if (operatorFilter == OperatorFilter.Or)
                    expression = replacedExpressions.Aggregate(Expression.Or);
                else throw new Exception();

                var lambda = Expression.Lambda<Func<TGridItem, bool>>(expression, parameter);
                if (lambda != null)
                    Grid.ApplyColumnFilter(lambda, Column);
            }
        }
        // todo : remplace le bouton par select
        /// <summary>
        /// Bascule entre les valeurs de <see cref="OperatorFilter"/>  pour assigne <see cref="operatorFilter"/>
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void ToggleOperatorFilter()
        {
            bool reset;
            (operatorFilter, reset) = operatorFilter switch
            {
                OperatorFilter.And => (OperatorFilter.Or, false),
                OperatorFilter.Or => (OperatorFilter.And, true),
                _ => throw new NotImplementedException()
            };
            if (reset)
                ResetColumnFilters();
        }
    }
}
