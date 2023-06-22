using Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RazorClassLibrary.DataGrid.Columns
{
    // doit être déplace verre un autre Project d'extension, si le développeur veux utilise DataAnnotation il devrai téléchargé extension
    public partial class PropertyColumn<TGridItem, TProp>
    {
        ///<summary>
        /// Méthode partielle pour obtenir le titre de la colonne à partir des annotations de données.
        /// Si l'extension est installée, cette méthode utilise les attributs <see cref="DisplayNameAttribute"/>  et <see cref="DisplayAttribute"/>  pour définir <see cref="Column{TGridItem}.Title"/> le titre de la colonne.
        ///</summary>
        partial void GetTitleFromDataAnnotations(MemberInfo? memberInfo)
        {
            var displayName = memberInfo?.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute;
            var display = memberInfo?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            Title = displayName?.DisplayName ?? display?.Name ?? memberInfo?.Name ?? "";
        }
        ///<summary>
        /// Méthode partielle pour obtenir le format à utiliser pour afficher la propriété à partir des annotations de données.
        /// Si l'extension est installée, cette méthode utilise l'attribut <see cref="DisplayFormatAttribute"/> pour définir <see cref="PropertyColumn{TGridItem, TProp}.Format"/> le format de la colonne.
        ///</summary>
        partial void GetDisplayFormatFromDataAnnotations(MemberInfo? memberInfo)
        {
            var displayFormat = memberInfo?.GetCustomAttribute(typeof(DisplayFormatAttribute)) as DisplayFormatAttribute;
            DisplayFormat = displayFormat?.DataFormatString;
        }
    }
}

