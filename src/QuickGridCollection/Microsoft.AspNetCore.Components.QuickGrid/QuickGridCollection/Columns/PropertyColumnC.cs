using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection
{
    /// <summary>
    /// The <see cref="PropertyColumnC{TGridItem, TProp}"/> class inherits from the <see cref="ColumnCBase{TGridItem}"/> class and represents a property column in a table.
    /// </summary>
    ///<typeparam name="TGridItem">The type of data items displayed in the grid.</typeparam>
    ///<typeparam name="TProp">The type of the property.</typeparam>
    /// <summary xml:lang="fr">
    /// La classe <see cref="PropertyColumnC{TGridItem, TProp}"/> hérite de la classe <see cref="ColumnCBase{TGridItem}"/> et représente une colonne de propriété dans un tableau.
    /// </summary>
    ///<typeparam name="TGridItem" xml:lang="fr">Le type des éléments de données affichés dans la grille.</typeparam>
    ///<typeparam name="TProp" xml:lang="fr">Le type de la propriété.</typeparam>
    public partial class PropertyColumnC<TGridItem, TProp> : ColumnCBase<TGridItem>
    {
        /// <summary>
        /// Function to get the cell text.
        /// </summary>
        /// <summary xml:lang="fr">
        /// Fonction pour obtenir le texte de la cellule.
        /// </summary>
        private Func<TGridItem, string?>? _cellTextFunc;

        /// <summary>
        /// Reference to the last instance of <see cref="PropertyColumnC{TGridItem, TProp}.DisplayFormat"/> assigned to this variable.
        /// </summary>
        /// <summary xml:lang="fr">
        /// Référence à la  dernière instance de <see cref="PropertyColumnC{TGridItem, TProp}.DisplayFormat"/> assignée à cette variable.
        /// </summary>
        private string? lastDisplayForma;


        /// <summary>
        /// Expression to get the property to display in the column.
        /// </summary>
        /// <summary xml:lang="fr">
        /// Expression pour obtenir la propriété à afficher dans la colonne.
        /// </summary>
        [Parameter, EditorRequired] public Expression<Func<TGridItem, TProp>> Property { get; set; } = default!;

        /// <summary>
        /// Format to use for displaying the property.
        /// </summary>
        /// <summary xml:lang="fr">
        /// Format à utiliser pour afficher la propriété.
        /// </summary>
        [Parameter] public string? DisplayFormat { get; set; }

        /// <summary>
        /// Indicates whether the column is sortable.
        /// </summary>
        /// <summary xml:lang="fr">
        /// Indique si la colonne est triable.
        /// </summary>
        [Parameter] public new bool IsSortable { get => isSortable; set => isSortable = value; }

        /// <summary>
        /// Gets or sets a value indicating whether this column can be sorted with other sortable columns.
        /// </summary>
        /// <remarks>
        /// If this property is set to <c>true</c> and the <see cref="IsSortable"/> property is also set to <c>true</c>, this column can be sorted with other sortable columns.
        /// </remarks>
        /// <summary xml:lang="fr">
        /// Obtient ou définit une valeur indiquant si cette colonne peut être triée avec d'autres colonnes triables.
        /// </summary>
        /// <remarks xml:lang="fr">
        /// Si cette propriété est définie sur <c>true</c> et que la propriété <see cref="IsSortable"/> est également définie sur <c>true</c>, cette colonne peut être triée avec d'autres colonnes triables.
        /// </remarks>
        [Parameter] public new bool MultipleSortingAllowed { get => multipleSortingAllowed; set => multipleSortingAllowed = value; }

        /// <summary>
        /// Indicates whether the column has filter options.
        /// </summary>
        ///<remarks>
        /// Note: if <see cref="PropertyColumnC{TGridItem, TProp}.HasAdvancedFilterOptions"/> is set to <c>true</c>, <see cref="PropertyColumnC{TGridItem, TProp}.HasFilterOptions"/> will be set to <c>false</c>.
        ///</remarks>
        /// <summary xml:lang="fr">
        /// Indique si la colonne a des options de filtre.
        /// </summary>
        ///<remarks>
        /// Remarque: si <see cref="PropertyColumnC{TGridItem, TProp}.HasAdvancedFilterOptions"/> est défini sur <c>true</c>, <see cref="PropertyColumnC{TGridItem, TProp}.HasFilterOptions"/> sera défini sur <c>false</c>.
        ///</remarks>
        [Parameter] public bool HasFilterOptions { get => hasFilterOptions; set => hasFilterOptions = value; }

        /// <summary>
        /// Indicates whether the column has advanced filter options.
        /// </summary>
        ///<remarks>
        /// Note: if <see cref="PropertyColumnC{TGridItem, TProp}.HasAdvancedFilterOptions"/> is set to <c>true</c>, <see cref="PropertyColumnC{TGridItem, TProp}.HasFilterOptions"/> will be set to <c>false</c>.
        ///</remarks>
        /// <summary xml:lang="fr">
        /// Indique si la colonne a des options de filtre avancées.
        /// </summary>
        ///<remarks>
        /// Remarque: si <see cref="PropertyColumnC{TGridItem, TProp}.HasAdvancedFilterOptions"/> est défini sur <c>true</c>, <see cref="PropertyColumnC{TGridItem, TProp}.HasFilterOptions"/> sera défini sur <c>false</c>.
        ///</remarks>
        [Parameter] public bool HasAdvancedFilterOptions { get => hasAdvancedFilterOptions; set => hasAdvancedFilterOptions = value; }

        /// <summary>
        /// Maximum number of filters to apply for this column. The default value is 5 and The minimum value is 2.
        /// </summary>
        /// <remarks> 
        /// This property is used if <see cref="PropertyColumnC{TGridItem, TProp}.HasAdvancedFilterOptions"/> is set to true
        ///</remarks> 
        /// <summary xml:lang="fr">
        /// Nombre maximum de filtres à applique pour cette colonne. La valeur par défaut est 5 et La valeur minimal est 2.
        /// </summary>
        /// <remarks xml:lang="fr">
        /// Cette propriété est utilise si <see cref="PropertyColumnC{TGridItem, TProp}.HasAdvancedFilterOptions"/> est définir sur <c>True</c>
        /// </remarks>
        [Parameter]
        public new int MaxFilters
        {
            get => maxFilters;
            set
            {
                if (value < 2)
                    maxFilters = 2;
                else
                    maxFilters = value;
            }
        }

        protected override void OnParametersSet()
        {
            MemberExpression? memberExpression = Property.Body as MemberExpression;

            if (HasAdvancedFilterOptions)
                hasFilterOptions = false;

            var IsNewProperty = InternalGridContext.Grid.IsFirstRender;
            if (IsNewProperty)
            {
                if (memberExpression != null)
                {
                    SetPropertyExpressionAndTypet(memberExpression);
                }                
            }
            if(IsNewProperty || lastDisplayForma != DisplayFormat)
            {
                lastDisplayForma = DisplayFormat;
                var compiledPropertyExpression = Property.Compile() ?? throw new ArgumentNullException();

                if (string.IsNullOrEmpty(DisplayFormat) && memberExpression is not null)
                    GetDisplayFormatFromDataAnnotations(memberExpression);

                if (!string.IsNullOrEmpty(DisplayFormat))
                {
                    var nullableUnderlyingTypeOrNull = Nullable.GetUnderlyingType(typeof(TProp));
                    if (!typeof(IFormattable).IsAssignableFrom(nullableUnderlyingTypeOrNull ?? typeof(TProp)))
                    {
                        throw new InvalidOperationException($"A '{nameof(DisplayFormat)}' parameter was supplied, but the type '{typeof(TProp)}' does not implement '{typeof(IFormattable)}'.");
                    }

                    _cellTextFunc = (TGridItem item) => ((IFormattable?)compiledPropertyExpression(item))?.ToString(DisplayFormat, null);
                }
                else
                {
                    _cellTextFunc = (TGridItem item) => compiledPropertyExpression(item)?.ToString();
                }
            }

            if (Title is null && memberExpression is not null)
            {                
                GetTitleFromDataAnnotations(memberExpression);
                Title ??= memberExpression.Member.Name;
            }
            if (IsNewProperty)
                AddColumn();
        }

        protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
        => builder.AddContent(0, _cellTextFunc!(item));

        partial void GetTitleFromDataAnnotations(MemberExpression memberExpression);

        partial void GetDisplayFormatFromDataAnnotations(MemberExpression memberExpression);

    }
}
