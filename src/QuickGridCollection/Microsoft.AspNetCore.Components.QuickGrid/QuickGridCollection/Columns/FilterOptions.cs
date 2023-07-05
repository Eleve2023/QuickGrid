namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns
{
    /// <summary>
    /// Énumération des options de filtre pour les champs de type chaîne.
    /// </summary>
    public enum StringFilterOptions
    {
        /// <summary>
        /// Option de filtre pour vérifier si la chaîne contient une sous-chaîne spécifiée.
        /// </summary>
        Contains,
        /// <summary>
        /// Option de filtre pour vérifier si la chaîne commence par une sous-chaîne spécifiée.
        /// </summary>
        StartsWith,
        /// <summary>
        /// Option de filtre pour vérifier si la chaîne se termine par une sous-chaîne spécifiée.
        /// </summary>
        EndsWith,
        /// <summary>
        /// Option de filtre pour vérifier si la chaîne est égale à une chaîne spécifiée.
        /// </summary>
        Equal,
        /// <summary>
        /// Option de filtre pour vérifier si la chaîne n'est pas égale à une chaîne spécifiée.
        /// </summary>
        NotEqual
    }
    /// <summary>
    /// Énumération des options de filtre pour les champs de type données.
    /// </summary>
    public enum DataFilterOptions
    {
        /// <summary>
        /// Option de filtre pour vérifier si la donnée n'est pas égale à une donnée spécifiée.
        /// </summary>
        NotEqual,
        /// <summary>
        /// Option de filtre pour vérifier si la donnée est supérieure à une donnée spécifiée.
        /// </summary>
        GreaterThan,
        /// <summary>
        /// Option de filtre pour vérifier si la donnée est supérieure ou égale à une donnée spécifiée.
        /// </summary>
        GreaterThanOrEqual,
        /// <summary>
        /// Option de filtre pour vérifier si la donnée est inférieure à une donnée spécifiée.
        /// </summary>
        LessThan,
        /// <summary>
        /// Option de filtre pour vérifier si la donnée est inférieure ou égale à une donnée spécifiée.
        /// </summary>
        LessThanOrEqual,
        /// <summary>
        /// Option de filtre pour vérifier si la donnée est égale à une donnée spécifiée.
        /// </summary>
        Equal
    }
    /// <summary>
    /// Énumération des options de filtre pour les champs de type énumération.
    /// </summary>
    public enum EnumFilterOptions
    {
        /// <summary>
        /// Option de filtre pour vérifier si l'énumération n'est pas égale à une énumération spécifiée.
        /// </summary>
        NotEqual,
        /// <summary>
        /// Option de filtre pour vérifier si l'énumération est égale à une énumération spécifiée.
        /// </summary>
        Equal
    }
    /// <summary>
    /// Énumération des options de filtre pour les champs de type booléen.
    /// </summary>
    public enum BoolFilterOptions
    {
        /// <summary>
        /// Option de filtre pour vérifier si le booléen est égal à un booléen spécifié.
        /// </summary>
        Equal
    }
    /// <summary>
    /// Énumération des opérateurs de filtre utilisés pour agréger les filtres.
    /// </summary>
    public enum FilterOperator
    {        
        AndAlso,
        And,
        AndAssign,        
        Or,
        OrElse,
        OrAssign
    }
}
