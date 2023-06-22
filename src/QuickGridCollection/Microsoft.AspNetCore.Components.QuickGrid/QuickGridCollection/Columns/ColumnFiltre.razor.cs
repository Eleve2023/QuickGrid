using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection;
using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Infrastructure;
using System.Linq.Expressions;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns
{
    public partial class ColumnFiltre<TGridItem> : ComponentBase
    {
        /// <summary>
        /// Cette chaîne de caractères contient le type d'entrée HTML à utiliser pour les champs de saisie du formulaire de recherche.
        /// Elle est déterminée en fonction du type de propriété associé au champ de saisie, spécifié par la propriété <see cref="TypeOfProperty"/>.
        /// Par exemple, pour les propriétés de type chaîne de caractères, elle prend la valeur "text", pour les propriétés de type numérique, elle prend la valeur "number", etc.
        /// </summary>
        private string htmlInputType = string.Empty;
        /// <summary>
        /// Obtient ou définit une valeur indiquant si le filtre est appliqué.
        /// Cette propriété est utilisée pour empêcher le déclenchement de l'événement <see cref="Grid{TGridItem}.FilterSortChanged"/> dans la méthode <see cref="ResetColumnFilters"/> si la méthode <see cref="ApplyFilters"/> n'a pas été appelée auparavant.
        /// </summary>
        private bool filterApplied;
        /// <summary>
        /// Ce champ est utilisé par la classe <see cref="ColumnFilterAdvenced{TGridItem}"/> pour indiquer le nombre de filtres ajoutés dans la colonne.
        /// </summary>
        protected int columnFilterAdditions;
        /// <summary>
        /// Cette liste contient les objets qui correspondent aux champs de saisie du formulaire de recherche.
        /// Elle est utilisée par la classe <see cref="ColumnFilterAdvenced{TGridItem}"/> pour gérer le nombre d'ajouts de filtres dans la colonne.
        /// </summary>
        protected List<object> filterValues = new() { string.Empty };
        /// <summary>
        /// Cette liste contient les options de filtre dans le formulaire de recherche.
        /// La première liste gère le nombre d'ajouts de filtres dans la colonne, tandis que la seconde liste permet de choisir le type de recherche à effectuer.
        /// </summary>
        protected List<List<Enum>> filterOptions = default!;
        /// <summary>
        /// Cette liste contient les options de filtre sélectionnées pour les champs de type énumération dans le formulaire de recherche.
        /// La liste gère le nombre d'ajouts de filtres dans la colonne
        /// </summary>
        protected List<Enum> selectedFilterOptions = default!;
        /// <summary>
        /// la valeur par défaut de <see cref="selectedFilterOptions"/>. 
        /// </summary>
        protected List<Enum> selectedFilterOptionsDefault = default!;
        /// <summary>
        /// le Type Enum a utilisée pour l'option de rechercher.
        /// </summary>
        protected Type optionsType = default!;
        /// <summary>
        /// Cette liste contient les expressions lambda générées à partir des options de filtre sélectionnées dans le formulaire de recherche.
        /// Les expressions sont créées en utilisant les valeurs contenues dans la propriété <see cref="filterValues"/> et les options de filtre sélectionnées.
        /// la List est utilisée par la classe <see cref="ColumnFilterAdvenced{TGridItem}"/> pour gérer le nombre d'ajouts de filtres dans la colonne.
        /// </summary>
        protected List<Expression<Func<TGridItem, bool>>>? columnFilterExpressions;

        [CascadingParameter] public Column<TGridItem> Column { get; set; } = default!;

        protected RenderFragment RenderFragment => FromRender;
        /// <summary>
        /// Une instance de la grille <see cref="Grid{TGridItem}"/>
        /// </summary>
        protected Grid<TGridItem> Grid => Column.Grid;
        /// <summary>
        /// Type de la propriété de la colonne.
        /// </summary>        
        protected Type TypeOfProperty => Column.TypeOfProperty ?? throw new NullReferenceException("Column.TypeOfProperty is null");
        /// <summary>
        /// Expression de propriété pour la colonne.
        /// </summary>
        protected Expression<Func<TGridItem, object>> PropertyExpression => Column.PropertyExpression ?? throw new NullReferenceException("Column.PropertyExpression is null");
        protected GridHtmlCssManager ClassAndStyle => Column.ClassAndStyle;
        /// <summary>
        /// Obtient ou définit une valeur indiquant si le filtre est appliqué.
        /// Cette propriété est utilisée pour empêcher le déclenchement de l'événement <see cref="Grid{TGridItem}.FilterSortChanged"/> dans la méthode <see cref="ResetColumnFilters"/> si la méthode <see cref="ApplyFilters"/> n'a pas été appelée auparavant.
        /// </summary>
        private bool FilterApplied { get => filterApplied; set => Column.OptionApplied = filterApplied = value; }


        protected override void OnParametersSet()
        {
            (optionsType, selectedFilterOptions, htmlInputType) = TypeOfProperty switch
            {
                Type t when t == typeof(string) =>
                            (typeof(OptionFiltreString), new List<Enum>() { OptionFiltreString.Contains }, "text"),
                Type t when t == typeof(DateTime) || t == typeof(DateTimeOffset) || t == typeof(DateOnly) =>
                            (typeof(OptionFiltreData), new() { OptionFiltreData.Equal }, "datetime-local"),
                Type t when t == typeof(TimeOnly) || t == typeof(TimeSpan) =>
                            (typeof(OptionFiltreData), new() { OptionFiltreData.Equal }, "time"),
                Type t when t == typeof(bool) =>
                            (typeof(OptionFiltreData), new() { OptionFiltreData.Equal }, "checkbox"),
                Type t when IsNumber(t) =>
                            (typeof(OptionFiltreData), new() { OptionFiltreData.Equal }, "number"),
                Type t when t.IsEnum =>
                            (typeof(OptionFiltreEnum), new() { OptionFiltreEnum.Equal }, string.Empty),
                _ => throw new NotSupportedException($"type {TypeOfProperty} not supported")
            };
            selectedFilterOptionsDefault = selectedFilterOptions.ToList();
        }
        private bool IsNumber(Type value)
        {
            return value == typeof(sbyte)
                || value == typeof(byte)
                || value == typeof(short)
                || value == typeof(ushort)
                || value == typeof(int)
                || value == typeof(uint)
                || value == typeof(long)
                || value == typeof(ulong)
                || value == typeof(float)
                || value == typeof(double)
                || value == typeof(decimal);
        }
        /// <summary>
        /// Cette méthode est appelée lorsque l'utilisateur clique sur le bouton "OK" du formulaire de recherche.
        /// Elle crée des expressions lambda à partir des options de filtre sélectionnées dans le formulaire et les ajoute à la liste des expressions de filtre de la colonne.
        /// Les expressions sont créées en utilisant les valeurs contenues dans la propriété <see cref="filterValues"/> et les options de filtre sélectionnées.
        /// Si des expressions valides sont créées, elles sont ajoutées à la liste <see cref="columnFilterExpressions"/> et le filtre est appliqué en appelant la méthode <see cref="ApplyColumnFilterFromGrid"/>.
        /// </summary>
        private void ApplyFilters()
        {
            for (int i = 0; i < columnFilterAdditions + 1; i++)
            {
                Func<Expression<Func<TGridItem, bool>>> action;

                if (selectedFilterOptions[i] is OptionFiltreString optionFiltreString)
                {
                    action = optionFiltreString switch
                    {
                        OptionFiltreString.Contains => new(() => CreateStringFilterExpression("Contains", filterValues[i])),
                        OptionFiltreString.StartsWith => new(() => CreateStringFilterExpression("StartsWith", filterValues[i])),
                        OptionFiltreString.EndsWith => new(() => CreateStringFilterExpression("EndsWith", filterValues[i])),
                        OptionFiltreString.Equal => new(() => CreateDataFilterExpression(ExpressionType.Equal, filterValues[i])),
                        OptionFiltreString.NotEqual => new(() => CreateDataFilterExpression(ExpressionType.NotEqual, filterValues[i])),
                        _ => throw new NotSupportedException()
                    };
                }
                else if (selectedFilterOptions[i] is OptionFiltreData optionFiltreData)
                {
                    action = optionFiltreData switch
                    {
                        OptionFiltreData.Equal => new(() => CreateDataFilterExpression(ExpressionType.Equal, filterValues[i])),
                        OptionFiltreData.GreaterThan => new(() => CreateDataFilterExpression(ExpressionType.GreaterThan, filterValues[i])),
                        OptionFiltreData.GreaterThanOrEqual => new(() => CreateDataFilterExpression(ExpressionType.GreaterThanOrEqual, filterValues[i])),
                        OptionFiltreData.LessThan => new(() => CreateDataFilterExpression(ExpressionType.LessThan, filterValues[i])),
                        OptionFiltreData.LessThanOrEqual => new(() => CreateDataFilterExpression(ExpressionType.LessThanOrEqual, filterValues[i])),
                        OptionFiltreData.NotEqual => new(() => CreateDataFilterExpression(ExpressionType.NotEqual, filterValues[i])),
                        _ => throw new NotSupportedException()
                    };
                }
                else if (selectedFilterOptions[i] is OptionFiltreEnum optionFiltreEnum)
                {
                    action = optionFiltreEnum switch
                    {
                        OptionFiltreEnum.Equal => new(() => CreateDataFilterExpression(ExpressionType.Equal, filterValues[i])),
                        OptionFiltreEnum.NotEqual => new(() => CreateDataFilterExpression(ExpressionType.NotEqual, filterValues[i])),
                        _ => throw new NotSupportedException()
                    };
                }
                else
                {
                    throw new NotSupportedException();
                }

                Expression<Func<TGridItem, bool>> lambda = action.Invoke();

                if (columnFilterExpressions == null)
                    columnFilterExpressions = new() { lambda };
                else
                {
                    if (columnFilterExpressions.Count < columnFilterAdditions + 1)
                    {
                        columnFilterExpressions.Add(lambda);
                    }
                    else
                    {
                        columnFilterExpressions[i] = lambda;
                    }
                }

            }
            if (columnFilterExpressions != null && columnFilterExpressions.Any())
            {
                ApplyColumnFilterFromGrid();
                FilterApplied = true;
                Column.IsOptionVisible = false;
            }
        }
        /// <summary>
        /// Cette méthode ajoute le filtre de la colonne dans la grille.
        /// </summary>
        protected virtual void ApplyColumnFilterFromGrid()
        {
            if (columnFilterExpressions != null && columnFilterExpressions.Any())
                Grid.ApplyColumnFilter(columnFilterExpressions.First(), Column);
        }
        /// <summary>
        /// Cette méthode réinitialise les options de filtre sélectionnées dans le formulaire de recherche et supprime les expressions de filtre de la colonne.
        /// Si un filtre a été appliqué précédemment, il est supprimé de la grille <see cref="Grid{TGridItem}.columnFilters"/>.
        /// </summary>
        protected void ResetColumnFilters()
        {
            filterValues = new() { string.Empty };
            selectedFilterOptions = selectedFilterOptionsDefault.ToList();
            filterOptions = default!;
            columnFilterExpressions = null!;
            columnFilterAdditions = 0;
            if (FilterApplied)
                Grid.ApplyColumnFilter(null, Column);
            FilterApplied = false;
        }

        //todo if objValue is null

        /// <summary>
        /// Cette méthode crée une expression lambda pour les champs de type chaîne de caractères en utilisant la méthode spécifiée et la valeur fournie.
        /// </summary>
        /// <param name="methode">La méthode à utiliser pour créer l'expression lambda.</param>
        /// <param name="objValue">La valeur à utiliser dans l'expression lambda.</param>
        /// <returns>Une expression lambda représentant le filtre pour un champ de type chaîne de caractères.</returns>
        private Expression<Func<TGridItem, bool>> CreateStringFilterExpression(string methode, object objValue)
        {
            MemberExpression memberExp = null!;
            if (PropertyExpression.Body is MemberExpression memberExpression)
            {
                memberExp = memberExpression;
            }
            else if (PropertyExpression.Body is UnaryExpression unaryExp && unaryExp.Operand is MemberExpression memberExpression1)
            {
                memberExp = memberExpression1;
            }
            if (memberExp != null)
            {
                var parameter = Expression.Parameter(typeof(TGridItem), "x");
                var property = Expression.Property(parameter, memberExp.Member.Name);

                var methodCallExpression = Expression.Call(
                    property,
                    methode,
                    Type.EmptyTypes,
                    Expression.Constant(objValue)
                );

                return Expression.Lambda<Func<TGridItem, bool>>(methodCallExpression, parameter);
            }
            else
            {
                throw new Exception("Not MemberExpression");
            }
        }
        /// <summary>
        /// Cette méthode crée une expression lambda pour les champs de type numérique, date, enum ou bool en utilisant le type de comparaison et la valeur fournie.
        /// </summary>
        /// <param name="comparisonType">Le type de comparaison à utiliser pour créer l'expression lambda.</param>
        /// <param name="objValue">La valeur à utiliser dans l'expression lambda.</param>
        /// <returns>Une expression lambda représentant le filtre pour un champ de type numérique, date, enum ou bool.</returns>
        private Expression<Func<TGridItem, bool>> CreateDataFilterExpression(ExpressionType comparisonType, object objValue)
        {
            MemberExpression memberExp = null!;
            if (PropertyExpression.Body is MemberExpression memberExpression)
            {
                memberExp = memberExpression;
            }
            else if (PropertyExpression.Body is UnaryExpression unaryExp && unaryExp.Operand is MemberExpression memberExpression1)
            {
                memberExp = memberExpression1;
            }

            if (memberExp != null)
            {
                object? objectConverted;
                if (TypeOfProperty.IsEnum)
                    objectConverted = Enum.Parse(TypeOfProperty, (string)objValue);
                else
                    objectConverted = Convert.ChangeType(objValue, TypeOfProperty);
                var parameter = Expression.Parameter(typeof(TGridItem), "x");
                var property = Expression.Property(parameter, memberExp.Member.Name);

                var unaryExpression = Expression.Convert(property, TypeOfProperty);

                var constant = Expression.Constant(objectConverted);
                var constantConverted = Expression.Convert(constant, TypeOfProperty);
                var comparison = Expression.MakeBinary(comparisonType, unaryExpression, constantConverted);
                return Expression.Lambda<Func<TGridItem, bool>>(comparison, parameter);
            }
            else
            {
                throw new Exception("Not MemberExpression");
            }

        }

        /// <summary>
        /// Cette méthode renvoie la liste des options de filtre pour les différents types de champs dans le formulaire de recherche.
        /// Elle est utilisée pour générer les menus déroulants permettant à l’utilisateur de sélectionner les options de filtre dans le formulaire.
        /// </summary>
        /// <param name="index">L'index de la liste des options de filtre à renvoyer.</param>
        /// <returns>La liste des options de filtre <see cref="filterOptions"/> à l'index spécifié.</returns>
        protected virtual List<Enum> GetListOptionFiltre(int index)
        {
            filterOptions ??= new() { Enum.GetValues(optionsType).Cast<Enum>().ToList() };
            return filterOptions[index];
        }
    }
}
