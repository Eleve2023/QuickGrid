// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

using Components.QuickGrid.QuickGridCollection.Columns.MenuOptions;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Linq.Expressions;

namespace Components.QuickGrid.Columns.MenuOptions;

public partial class MenuFiltre<TGridItem> : ComponentBase
{
    /// <summary>
    /// This string contains the HTML input type to use for the search form input fields.
    /// It is determined based on the type of property associated with the input field, specified by the <see cref="TypeOfProperty"/> property.
    /// For example, for string type properties, it takes the value "text", for numeric type properties, it takes the value "number", etc.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Cette chaîne de caractères contient le type d'entrée HTML à utiliser pour les champs de saisie du formulaire de recherche.
    /// Elle est déterminée en fonction du type de propriété associé au champ de saisie, spécifié par la propriété <see cref="TypeOfProperty"/>.
    /// Par exemple, pour les propriétés de type chaîne de caractères, elle prend la valeur "text", pour les propriétés de type numérique, elle prend la valeur "number", etc.
    /// </summary>
    private string htmlInputType = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the filter is applied.
    /// This property is used to prevent triggering of the <see cref="QuickGridC{TGridItem}.FilterSortChanged"/> event in the <see cref="ResetColumnFilters"/> method if the <see cref="ApplyFilters"/> method has not been called previously.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Obtient ou définit une valeur indiquant si le filtre est appliqué.
    /// Cette propriété est utilisée pour empêcher le déclenchement de l'événement <see cref="QuickGridC{TGridItem}.FilterSortChanged"/> dans la méthode <see cref="ResetColumnFilters"/> si la méthode <see cref="ApplyFilters"/> n'a pas été appelée auparavant.
    /// </summary>
    private bool filterApplied;

    /// <summary>
    /// This field is used by the <see cref="MenuAdvancedFilter{TGridItem}"/> class to indicate the number of filters added in the column.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Ce champ est utilisé par la classe <see cref="MenuAdvancedFilter{TGridItem}"/> pour indiquer le nombre de filtres ajoutés dans la colonne.
    /// </summary>
    protected int columnFilterAdditions;

    /// <summary>
    /// This list contains the objects that correspond to the search form input fields.
    /// It is used by the <see cref="MenuAdvancedFilter{TGridItem}"/> class to manage the number of filter additions in the column.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Cette liste contient les objets qui correspondent aux champs de saisie du formulaire de recherche.
    /// Elle est utilisée par la classe <see cref="MenuAdvancedFilter{TGridItem}"/> pour gérer le nombre d'ajouts de filtres dans la colonne.
    /// </summary>
    protected List<object?> filterValues = new() { null };

    /// <summary>
    /// This list contains the filter options in the search form.
    /// The first list manages the number of filter additions in the column, while the second list allows you to choose the type of search to perform.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Cette liste contient les options de filtre dans le formulaire de recherche.
    /// La première liste gère le nombre d'ajouts de filtres dans la colonne, tandis que la seconde liste permet de choisir le type de recherche à effectuer.
    /// </summary>
    protected List<List<Enum>> filterOptions = default!;

    /// <summary>
    /// This list contains the selected filter options for enumeration type fields in the search form.
    /// The list manages the number of filter additions in the column
    /// </summary>
    /// <summary xml:lang="fr">
    /// Cette liste contient les options de filtre sélectionnées pour les champs de type énumération dans le formulaire de recherche.
    /// La liste gère le nombre d'ajouts de filtres dans la colonne
    /// </summary>
    protected List<Enum> selectedFilterOptions = default!;

    /// <summary>
    /// The default value of <see cref="selectedFilterOptions"/>.
    /// </summary>
    /// <summary xml:lang="fr">
    /// la valeur par défaut de <see cref="selectedFilterOptions"/>. 
    /// </summary>
    protected List<Enum> selectedFilterOptionsDefault = default!;

    /// <summary>
    /// The Enum Type to use for search options.
    /// </summary>
    /// <summary xml:lang="fr">
    /// le Type Enum a utilisée pour l'option de rechercher.
    /// </summary>
    protected Type optionsType = default!;

    /// <summary>
    /// This list contains lambda expressions generated from selected filter options in the search form.
    /// Expressions are created using values contained in <see cref="filterValues"/> and selected filter options.
    /// The list is used by the <see cref="MenuAdvancedFilter{TGridItem}"/> class to manage the number of filter additions in the column.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Cette liste contient les expressions lambda générées à partir des options de filtre sélectionnées dans le formulaire de recherche.
    /// Les expressions sont créées en utilisant les valeurs contenues dans la propriété <see cref="filterValues"/> et les options de filtre sélectionnées.
    /// la List est utilisée par la classe <see cref="MenuAdvancedFilter{TGridItem}"/> pour gérer le nombre d'ajouts de filtres dans la colonne.
    /// </summary>
    protected List<Expression<Func<TGridItem, bool>>>? columnFilterExpressions;

    [CascadingParameter] public ColumnCBase<TGridItem> Column { get; set; } = default!;

    protected RenderFragment RenderFragment => FromRender;

    /// <summary>
    /// An instance of the <see cref="QuickGridC{TGridItem}"/> grid.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Une instance de la grille <see cref="QuickGridC{TGridItem}"/>
    /// </summary>
    protected QuickGridC<TGridItem> Grid => Column.Grid;

    /// <summary>
    /// Type of the column property.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Type de la propriété de la colonne.
    /// </summary>        
    protected Type TypeOfProperty => Column.TypeOfProperty is not null ? Column.TypeOfProperty : throw new MenuOptionException("Column.TypeOfProperty is null");
    //protected Type TypeOfProperty => Column.TypeOfProperty is not null ? Nullable.GetUnderlyingType(Column.TypeOfProperty) ?? Column.TypeOfProperty : throw new MenuOptionException("Column.TypeOfProperty is null");

    /// <summary>
    /// Property expression for the column.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Expression de propriété pour la colonne.
    /// </summary>
    protected Expression<Func<TGridItem, object?>> PropertyExpression => Column.PropertyExpression ?? throw new MenuOptionException("Column.PropertyExpression is null");
    protected GridHtmlCssManager CssClassAndStyle => Column.CssClassAndStyle;

    /// <summary>
    /// Gets or sets a value indicating whether the filter is applied.
    /// This property is used to prevent triggering of the <see cref="QuickGridC{TGridItem}.FilterSortChanged"/> event in the <see cref="ResetColumnFilters"/> method if the <see cref="ApplyFilters"/> method has not been called previously.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Obtient ou définit une valeur indiquant si le filtre est appliqué.
    /// Cette propriété est utilisée pour empêcher le déclenchement de l'événement <see cref="QuickGridC{TGridItem}.FilterSortChanged"/> dans la méthode <see cref="ResetColumnFilters"/> si la méthode <see cref="ApplyFilters"/> n'a pas été appelée auparavant.
    /// </summary>
    private bool FilterApplied { get => filterApplied; set => Column.OptionApplied = filterApplied = value; }

    protected override void OnParametersSet()
    {
        var typeOfProperty = Nullable.GetUnderlyingType(TypeOfProperty)?? TypeOfProperty;
        (optionsType, selectedFilterOptions, htmlInputType) = typeOfProperty switch
        {
            Type t when t == typeof(string) =>
                        (typeof(StringFilterOptions), new List<Enum>() { StringFilterOptions.Contains }, "text"),
            Type t when t == typeof(Guid) =>
                        (typeof(DataFilterOptions), new() { DataFilterOptions.Equal }, "text"),
            Type t when t == typeof(char) =>
                        (typeof(DataFilterOptions), new() { DataFilterOptions.Equal }, "text"),
            Type t when t == typeof(DateTime) || t == typeof(DateTimeOffset) =>
                        (typeof(DataFilterOptions), new() { DataFilterOptions.Equal }, "datetime-local"),
            Type t when t == typeof(DateOnly) =>
                        (typeof(DataFilterOptions), new() { DataFilterOptions.Equal }, "date"),
            Type t when t == typeof(TimeOnly) || t == typeof(TimeSpan) =>
                        (typeof(DataFilterOptions), new() { DataFilterOptions.Equal }, "time"),
            Type t when t == typeof(bool) =>
                        (typeof(BoolFilterOptions), new() { DataFilterOptions.Equal }, "select"),
            Type t when IsNumber(t) =>
                        (typeof(DataFilterOptions), new() { DataFilterOptions.Equal }, "number"),
            Type t when t.IsEnum =>
                        (typeof(EnumFilterOptions), new() { EnumFilterOptions.Equal }, string.Empty),
            _ => throw new NotSupportedException($"type {TypeOfProperty} not supported")
        };
        selectedFilterOptionsDefault = selectedFilterOptions.ToList();
    }
    private static bool IsNumber(Type value)
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
    /// This method is called when the user clicks on the "OK" button of the search form.
    /// It creates lambda expressions from selected filter options in the form and adds them to the list of column filter expressions.
    /// Expressions are created using values contained in <see cref="filterValues"/> and selected filter options.
    /// If valid expressions are created, they are added to the <see cref="columnFilterExpressions"/> list and the filter is applied by calling the <see cref="ApplyColumnFilterFromGrid"/> method.
    /// </summary>
    /// <summary xml:lang="fr">
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

            if (selectedFilterOptions[i] is StringFilterOptions optionFiltreString)
            {
                action = optionFiltreString switch
                {
                    StringFilterOptions.Contains => new(() => CreateStringFilterExpression("Contains", filterValues[i])),
                    StringFilterOptions.StartsWith => new(() => CreateStringFilterExpression("StartsWith", filterValues[i])),
                    StringFilterOptions.EndsWith => new(() => CreateStringFilterExpression("EndsWith", filterValues[i])),
                    StringFilterOptions.Equal => new(() => CreateDataFilterExpression(ExpressionType.Equal, filterValues[i])),
                    StringFilterOptions.NotEqual => new(() => CreateDataFilterExpression(ExpressionType.NotEqual, filterValues[i])),
                    _ => throw new NotSupportedException()
                };
            }
            else if (selectedFilterOptions[i] is DataFilterOptions optionFiltreData)
            {
                action = optionFiltreData switch
                {
                    DataFilterOptions.Equal => new(() => CreateDataFilterExpression(ExpressionType.Equal, filterValues[i])),
                    DataFilterOptions.GreaterThan => new(() => CreateDataFilterExpression(ExpressionType.GreaterThan, filterValues[i])),
                    DataFilterOptions.GreaterThanOrEqual => new(() => CreateDataFilterExpression(ExpressionType.GreaterThanOrEqual, filterValues[i])),
                    DataFilterOptions.LessThan => new(() => CreateDataFilterExpression(ExpressionType.LessThan, filterValues[i])),
                    DataFilterOptions.LessThanOrEqual => new(() => CreateDataFilterExpression(ExpressionType.LessThanOrEqual, filterValues[i])),
                    DataFilterOptions.NotEqual => new(() => CreateDataFilterExpression(ExpressionType.NotEqual, filterValues[i])),
                    _ => throw new NotSupportedException()
                };
            }
            else if (selectedFilterOptions[i] is EnumFilterOptions optionFiltreEnum)
            {
                action = optionFiltreEnum switch
                {
                    EnumFilterOptions.Equal => new(() => CreateDataFilterExpression(ExpressionType.Equal, filterValues[i])),
                    EnumFilterOptions.NotEqual => new(() => CreateDataFilterExpression(ExpressionType.NotEqual, filterValues[i])),
                    _ => throw new NotSupportedException()
                };
            }
            else
            {
                throw new NotSupportedException();
            }

            Expression<Func<TGridItem, bool>> lambda = action.Invoke();

            if (columnFilterExpressions == null)
            {
                columnFilterExpressions = new() { lambda };
            }
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
        if (columnFilterExpressions != null && columnFilterExpressions.Count != 0)
        {
            ApplyColumnFilterFromGrid();
            FilterApplied = true;
            Column.IsOptionVisible = false;
        }
    }

    /// <summary>
    /// This method adds the column filter to the grid.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Cette méthode ajoute le filtre de la colonne dans la grille.
    /// </summary>
    protected virtual void ApplyColumnFilterFromGrid()
    {
        if (columnFilterExpressions != null && columnFilterExpressions.Count != 0)
        {
            Grid.ApplyColumnFilter(columnFilterExpressions.First(), Column);
        }
    }

    /// <summary>
    /// This method resets selected filter options in the search form and removes column filter expressions.
    /// If a filter has been applied previously, it is removed from the grid <see cref="QuickGridC{TGridItem}.columnFilters"/>.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Cette méthode réinitialise les options de filtre sélectionnées dans le formulaire de recherche et supprime les expressions de filtre de la colonne.
    /// Si un filtre a été appliqué précédemment, il est supprimé de la grille <see cref="QuickGridC{TGridItem}.columnFilters"/>.
    /// </summary>
    protected void ResetColumnFilters()
    {
        filterValues = new() { null };
        selectedFilterOptions = selectedFilterOptionsDefault.ToList();
        filterOptions = default!;
        columnFilterExpressions = null!;
        columnFilterAdditions = 0;
        if (FilterApplied)
        {
            Grid.ApplyColumnFilter(null, Column);
        }

        FilterApplied = false;
    }

    /// <summary>
    /// This method creates a lambda expression for string type fields using the specified method and provided value.
    /// </summary>
    /// <param name="methode">The method to use to create the lambda expression.</param>
    /// <param name="objValue">The value to use in the lambda expression.</param>
    /// <returns>A lambda expression representing the filter for a string type field.</returns>
    /// <summary xml:lang="fr">
    /// Cette méthode crée une expression lambda pour les champs de type chaîne de caractères en utilisant la méthode spécifiée et la valeur fournie.
    /// </summary>
    /// <param name="methode" xml:lang="fr">La méthode à utiliser pour créer l'expression lambda.</param>
    /// <param name="objValue" xml:lang="fr">La valeur à utiliser dans l'expression lambda.</param>
    /// <returns xml:lang="fr">Une expression lambda représentant le filtre pour un champ de type chaîne de caractères.</returns>
    private Expression<Func<TGridItem, bool>> CreateStringFilterExpression(string methode, object? objValue)
    {
        MemberExpression memberExp = null!;
        if (objValue == null)
        {
            return CreateDataFilterExpression(ExpressionType.Equal, objValue);
        }

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
            throw new MenuOptionException("Not MemberExpression");
        }
    }

    /// <summary>
    /// This method creates a lambda expression for numeric, date, enum or bool type fields using the comparison type and provided value.
    /// </summary>
    /// <param name="comparisonType">The comparison type to use to create the lambda expression.</param>
    /// <param name="objValue">The value to use in the lambda expression.</param>
    /// <returns>A lambda expression representing the filter for a numeric, date, enum or bool type field.</returns>
    /// <summary xml:lang="fr">
    /// Cette méthode crée une expression lambda pour les champs de type numérique, date, enum ou bool en utilisant le type de comparaison et la valeur fournie.
    /// </summary>
    /// <param name="comparisonType" xml:lang="fr">Le type de comparaison à utiliser pour créer l'expression lambda.</param>
    /// <param name="objValue" xml:lang="fr">La valeur à utiliser dans l'expression lambda.</param>
    /// <returns xml:lang="fr">Une expression lambda représentant le filtre pour un champ de type numérique, date, enum ou bool.</returns>
    private Expression<Func<TGridItem, bool>> CreateDataFilterExpression(ExpressionType comparisonType, object? objValue)
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
            var typeOfProperty = Nullable.GetUnderlyingType(TypeOfProperty) ?? TypeOfProperty;
            object? objectConverted;
            if (objValue == null || string.IsNullOrEmpty((string)objValue))
            {
                objectConverted = null;
            }
            else if (typeOfProperty.IsEnum)
            {
                objectConverted = Enum.Parse(typeOfProperty, (string)objValue);
            }
            else if (typeOfProperty == typeof(DateOnly) || Nullable.GetUnderlyingType(typeOfProperty) == typeof(DateOnly))
            {
                objectConverted = DateOnly.Parse((string)objValue);
            }
            else if (typeOfProperty == typeof(DateTimeOffset))
            {
                objectConverted = (DateTimeOffset?)DateTimeOffset.Parse((string)objValue).ToUniversalTime();
            }
            else if (typeOfProperty == typeof(TimeSpan))
            {
                objectConverted = TimeSpan.Parse((string)objValue);
            }
            else if (typeOfProperty == typeof(TimeOnly))
            {
                objectConverted = TimeOnly.Parse((string)objValue);
            }
            else if (typeOfProperty == typeof(decimal))
            {
                objectConverted = decimal.Parse((string)objValue, CultureInfo.InvariantCulture);
            }
            else if (typeOfProperty == typeof(Guid))
            {
                objectConverted = Guid.Parse((string)objValue);
            }
            else
            {
                objectConverted = Convert.ChangeType(objValue, typeOfProperty);
            }

            var parameter = Expression.Parameter(typeof(TGridItem), "x");
            var property = Expression.Property(parameter, memberExp.Member.Name);

            var unaryExpression = Expression.Convert(property, typeOfProperty);
            if (objectConverted != null)
            {
                var constant = Expression.Constant(objectConverted);
                var constantConverted = Expression.Convert(constant, typeOfProperty);
                var comparison = Expression.MakeBinary(comparisonType, unaryExpression, constantConverted);
                return Expression.Lambda<Func<TGridItem, bool>>(comparison, parameter);
            }
            else
            {
                var constant = Expression.Constant(null);
                //var constantConverted = Expression.Convert(constant, TypeOfProperty);
                //todo ajoute un controle pour comparisonType
                var salfeComparisonType = comparisonType switch
                {
                    ExpressionType.Equal => ExpressionType.Equal,
                    _ => ExpressionType.NotEqual
                };
                BinaryExpression comparison;
                if (Nullable.GetUnderlyingType(TypeOfProperty) == null && TypeOfProperty != typeof(string))
                {
                    comparison = Expression.MakeBinary(salfeComparisonType, property, Expression.Default(property.Type));
                }
                else
                {
                    comparison = Expression.MakeBinary(salfeComparisonType, property, constant);
                }
                return Expression.Lambda<Func<TGridItem, bool>>(comparison, parameter);
            }
        }
        else
        {
            throw new MenuOptionException("Not MemberExpression");
        }

    }

    /// <summary>
    /// Returns the list of filter options for different field types in the search form.
    /// This method is used to generate drop-down menus allowing the user to select filter options in the form.
    /// </summary>
    /// <param name="index">The index of the filter options list to return.</param>
    /// <returns>The list of filter options <see cref="filterOptions"/> at the specified index.</returns>
    /// <summary xml:lang="fr">
    /// Renvoie la liste des options de filtre pour les différents types de champs dans le formulaire de recherche.
    /// Cette méthode est utilisée pour générer les menus déroulants permettant à l'utilisateur de sélectionner les options de filtre dans le formulaire.
    /// </summary>
    /// <param name="index" xml:lang="fr">L'index de la liste des options de filtre à renvoyer.</param>
    /// <returns xml:lang="fr">La liste des options de filtre <see cref="filterOptions"/> à l'index spécifié.</returns>
    protected virtual List<Enum> GetListOptionFiltre(int index)
    {
        filterOptions ??= new() { Enum.GetValues(optionsType).Cast<Enum>().ToList() };
        return filterOptions[index];
    }
}
