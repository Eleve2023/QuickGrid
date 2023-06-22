using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns
{
    ///<summary>
    /// La classe <see cref="PropertyColumn{TGridItem, TProp}"/> hérite de la classe <see cref="Column{TGridItem}"/> et représente une colonne de propriété dans un tableau.
    ///</summary>
    ///<typeparam name="TGridItem">Le type des éléments de données affichés dans la grille.</typeparam>
    ///<typeparam name="TProp">Le type de la propriété.</typeparam>
    public partial class PropertyColumn<TGridItem, TProp> : Column<TGridItem>
    {
        ///<summary>
        /// Fonction pour obtenir le texte de la cellule.
        ///</summary>
        private Func<TGridItem, string?>? _cellTextFunc;

        public PropertyColumn()
        {
            isSortable = true;
            hasFilterOptions = true;
            hasAdvancedFilterOptions = false;
        }
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
        ///<summary>
        /// Indique si la colonne a des options de filtre. 
        /// <para>
        /// La valeur par défaut est <c>true</c>.
        /// </para> 
        ///</summary>
        ///<remarks>
        /// Remarque: si <see cref="PropertyColumn{TGridItem, TProp}.HasAdvancedFilterOptions"/> est défini sur <c>true</c>, <see cref="PropertyColumn{TGridItem, TProp}.HasFilterOptions"/> sera défini sur <c>false</c>.
        ///</remarks>
        [Parameter] public bool HasFilterOptions { get => hasFilterOptions; set => hasFilterOptions = value; }
        ///<summary>
        /// Indique si la colonne a des options de filtre avancées.
        ///</summary>
        ///<remarks>
        /// Remarque: si <see cref="PropertyColumn{TGridItem, TProp}.HasAdvancedFilterOptions"/> est défini sur <c>true</c>, <see cref="PropertyColumn{TGridItem, TProp}.HasFilterOptions"/> sera défini sur <c>false</c>.
        ///</remarks>
        [Parameter] public bool HasAdvancedFilterOptions { get => hasAdvancedFilterOptions; set => hasAdvancedFilterOptions = value; }
        /// <summary>
        /// Nombre maximum de filtres à applique pour cette colonne. La valeur par défaut est 5 et La valeur minimal est 2.
        /// </summary>
        /// <remarks>
        /// Cette propriété est utilise si <see cref="PropertyColumn{TGridItem, TProp}.HasAdvancedFilterOptions"/> est définir sur <c>True</c>
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
                var compiledPropertyExpression = Property.Compile() ?? throw new ArgumentNullException();

                if (string.IsNullOrEmpty(DisplayFormat) && memberExpression is not null)
                    GetDisplayFormatFromDataAnnotations(memberExpression.Member);

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
                MemberInfo? memberInfo = memberExpression.Member;
                GetTitleFromDataAnnotations(memberInfo);
                Title ??= memberInfo?.Name ?? "";
            }
            if (IsNewProperty)
                AddColumn();
        }

        protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
        => builder.AddContent(0, _cellTextFunc!(item));

        partial void GetTitleFromDataAnnotations(MemberInfo? memberInfo);

        partial void GetDisplayFormatFromDataAnnotations(MemberInfo? memberInfo);

    }
}
