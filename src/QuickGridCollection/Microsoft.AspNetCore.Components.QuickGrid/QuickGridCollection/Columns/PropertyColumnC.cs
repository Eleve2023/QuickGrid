using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns
{
    ///<summary>
    /// La classe <see cref="PropertyColumnC{TGridItem, TProp}"/> hérite de la classe <see cref="ColumnCBase{TGridItem}"/> et représente une colonne de propriété dans un tableau.
    ///</summary>
    ///<typeparam name="TGridItem">Le type des éléments de données affichés dans la grille.</typeparam>
    ///<typeparam name="TProp">Le type de la propriété.</typeparam>
    public partial class PropertyColumnC<TGridItem, TProp> : ColumnCBase<TGridItem>
    {
        ///<summary>
        /// Fonction pour obtenir le texte de la cellule.
        ///</summary>
        private Func<TGridItem, string?>? _cellTextFunc;
        /// <summary>
        /// Référence à la  dernière instance de <see cref="PropertyColumnC{TGridItem, TProp}.DisplayFormat"/> assignée à cette variable.
        /// </summary>
        private string? lastDisplayForma;

        
        ///<summary>
        /// Expression pour obtenir la propriété à afficher dans la colonne.
        ///</summary>
        [Parameter, EditorRequired] public Expression<Func<TGridItem, TProp>> Property { get; set; } = default!;
        ///<summary>
        /// Format à utiliser pour afficher la propriété.
        ///</summary>
        [Parameter] public string? DisplayFormat { get; set; }
        ///<summary>
        /// Indique si la colonne est triable. La valeur par défaut est <c>true</c>.
        ///</summary>
        [Parameter] public new bool IsSortable { get => isSortable; set => isSortable = value; }
        /// <summary>
        /// Obtient ou définit une valeur indiquant si cette colonne peut être triée avec d'autres colonnes triables.
        /// </summary>
        /// <remarks>
        /// Si cette propriété est définie sur <c>true</c> et que la propriété <see cref="IsSortable"/> est également définie sur <c>true</c>, cette colonne peut être triée avec d'autres colonnes triables.
        /// </remarks>
        [Parameter] public new bool MultipleSortingAllowed { get => multipleSortingAllowed; set => multipleSortingAllowed = value; }
        ///<summary>
        /// Indique si la colonne a des options de filtre. 
        /// <para>
        /// La valeur par défaut est <c>true</c>.
        /// </para> 
        ///</summary>
        ///<remarks>
        /// Remarque: si <see cref="PropertyColumnC{TGridItem, TProp}.HasAdvancedFilterOptions"/> est défini sur <c>true</c>, <see cref="PropertyColumnC{TGridItem, TProp}.HasFilterOptions"/> sera défini sur <c>false</c>.
        ///</remarks>
        [Parameter] public bool HasFilterOptions { get => hasFilterOptions; set => hasFilterOptions = value; }
        ///<summary>
        /// Indique si la colonne a des options de filtre avancées.
        ///</summary>
        ///<remarks>
        /// Remarque: si <see cref="PropertyColumnC{TGridItem, TProp}.HasAdvancedFilterOptions"/> est défini sur <c>true</c>, <see cref="PropertyColumnC{TGridItem, TProp}.HasFilterOptions"/> sera défini sur <c>false</c>.
        ///</remarks>
        [Parameter] public bool HasAdvancedFilterOptions { get => hasAdvancedFilterOptions; set => hasAdvancedFilterOptions = value; }
        /// <summary>
        /// Nombre maximum de filtres à applique pour cette colonne. La valeur par défaut est 5 et La valeur minimal est 2.
        /// </summary>
        /// <remarks>
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
                    SetPropertyExpressionAndTypet<TProp>(memberExpression);
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
