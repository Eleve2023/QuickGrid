﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Columns
{
    // doit être déplace verre un autre Project d'extension, si le développeur veux utilise DataAnnotation il devrai téléchargé extension
    public partial class PropertyColumnC<TGridItem, TProp>
    {
        /// <summary xml:lang="fr">
        /// Méthode partielle pour obtenir le titre de la colonne à partir des annotations de données.
        /// Si l'extension est installée, cette méthode utilise les attributs <see cref="DisplayNameAttribute"/>  et <see cref="DisplayAttribute"/>  pour définir <see cref="ColumnCBase{TGridItem}.Title"/> le titre de la colonne.
        /// </summary>
        partial void GetTitleFromDataAnnotations(MemberExpression memberExpression)
        {
            var memberInfo = memberExpression.Member;
            var displayName = memberInfo.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute;
            var display = memberInfo.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            Title = displayName?.DisplayName ?? display?.Name ?? memberInfo.Name ?? "";
        }
        /// <summary xml:lang="fr">
        /// Méthode partielle pour obtenir le format à utiliser pour afficher la propriété à partir des annotations de données.
        /// Si l'extension est installée, cette méthode utilise l'attribut <see cref="DisplayFormatAttribute"/> pour définir <see cref="PropertyColumnC{TGridItem, TProp}.Format"/> le format de la colonne.
        /// </summary>
        partial void GetDisplayFormatFromDataAnnotations(MemberExpression memberExpression)
        {
            var memberInfo = memberExpression.Member;
            var displayFormat = memberInfo.GetCustomAttribute(typeof(DisplayFormatAttribute)) as DisplayFormatAttribute;
            DisplayFormat = displayFormat?.DataFormatString;
        }
    }
}

