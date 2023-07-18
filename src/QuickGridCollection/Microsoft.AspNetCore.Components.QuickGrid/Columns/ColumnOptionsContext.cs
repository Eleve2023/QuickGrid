// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

using System.Linq.Expressions;

namespace Components.QuickGrid.Columns;

/// <summary>
/// Provides a context for custom column options
/// defined in the <see cref="ColumnCBase{TGridItem}.ColumnOptions"/> property.
/// </summary>
/// <typeparam name="TGridItem">The type of data items displayed in the grid.</typeparam>
/// <summary xml:lang="fr">
/// Fournit un contexte pour les options de colonne personnalisées
/// définies dans la propriété <see cref="ColumnCBase{TGridItem}.ColumnOptions"/>.    
/// </summary>
/// <typeparam name="TGridItem" xml:lang="fr">Le type des éléments de données affichés dans la grille.</typeparam>
public class ColumnOptionsContext<TGridItem>
{
    private readonly ColumnCBase<TGridItem> column;

    public ColumnOptionsContext(ColumnCBase<TGridItem> column)
    {
        this.column = column;
    }

    /// <summary>
    /// Indicates whether the column options are applied.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Indique si les options de la colonne sont applique 
    /// </summary>
    public bool OptionApplied { get => column.OptionApplied; set => column.OptionApplied = value; }

    /// <summary>
    /// Indicates whether column options are shown or hidden.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Indique si les options de colonne sont affichées ou masquées.
    /// </summary>
    public bool IsOptionVisible { get => column.IsOptionVisible; set => column.IsOptionVisible = value; }

    /// <summary>
    /// Adds or updates a filter for a specified column.
    /// If the filter expression is null, the existing filter for this column is removed.
    /// Otherwise, the existing filter for this column is updated or added to the list of filters.
    /// </summary>
    /// <summary xml:lang="fr">
    /// Ajoute ou met à jour un filtre pour une colonne spécifiée.
    /// Si l'expression de filtre est nulle, le filtre existant pour cette colonne est supprimé.
    /// Sinon, le filtre existant pour cette colonne est mis à jour ou ajouté à la liste des filtres.
    /// </summary>
    public void ApplyColumnFilter(Expression<Func<TGridItem, bool>>? expression)
    {
        column.Grid.ApplyColumnFilter(expression, column);
    }
}
