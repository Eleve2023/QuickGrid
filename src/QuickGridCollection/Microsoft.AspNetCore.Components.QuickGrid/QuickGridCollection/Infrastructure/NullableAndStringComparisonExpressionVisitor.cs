﻿using System.Linq.Expressions;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Infrastructure
{
    /// <summary>
    /// Un visiteur d'expression personnalisé qui gère les expressions avec des valeurs Nullable et des comparaisons de chaînes insensibles à la casse.
    /// </summary>
    internal class NullableAndStringComparisonExpressionVisitor : ExpressionVisitor
    {
        private bool useDefaultValueForNull;
        private bool ignoreCaseInStringComparison;

        /// <summary>
        /// Crée une nouvelle instance de la classe NullableAndStringComparisonExpressionVisitor.
        /// </summary>
        /// <param name="useDefaultValueForNull">Indique si les objets nullables doivent être traités comme ayant une valeur par défaut lors de la visite d'expressions.</param>
        /// <param name="ignoreCaseInStringComparison">Indique si les comparaisons de chaînes doivent être insensibles à la casse lors de la visite d'expressions.</param>
        internal NullableAndStringComparisonExpressionVisitor(bool useDefaultValueForNull = false, bool ignoreCaseInStringComparison = true)
        {
            this.useDefaultValueForNull = useDefaultValueForNull;
            this.ignoreCaseInStringComparison = ignoreCaseInStringComparison;
        }

        /// <summary>
        /// Visite les nœuds d'appel de méthode de l'arbre d'expression.
        /// </summary>
        /// <param name="node">Le nœud d'appel de méthode à visiter.</param>
        /// <returns>Le nœud d'appel de méthode modifié, si nécessaire, ou le nœud d'origine si aucune modification n'est nécessaire.</returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Object != null && node.Object.Type == typeof(string))
            {
                var expressionBody = node.Object;
                var arguments = node.Arguments;

                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                if (ignoreCaseInStringComparison)
                {
                    expressionBody = Expression.Call(node.Object, toLowerMethod!);
                    arguments = Array.AsReadOnly(new[] { (Expression)Expression.Call(node.Arguments[0], toLowerMethod!) });
                }

                if (useDefaultValueForNull)
                {
                    Expression instance = Expression.Coalesce(node.Object, Expression.Constant(string.Empty));
                    if (ignoreCaseInStringComparison)
                        instance = Expression.Call(instance, toLowerMethod!);

                    var methodCall = Expression.Call(instance, node.Method, arguments);
                    return methodCall;
                }
                else
                {
                    var conditionalExpression = Expression.Condition(Expression.Equal(node.Object, Expression.Constant(null)),
                                                                     Expression.Constant(false),
                                                                     Expression.Call(expressionBody, node.Method, arguments));
                    return conditionalExpression;
                }
            }

            return base.VisitMethodCall(node);
        }

        /// <summary>
        /// Visite les nœuds binaires de l'arbre d'expression.
        /// </summary>
        /// <param name="node">Le nœud binaire à visiter.</param>
        /// <returns>Le nœud binaire modifié, si nécessaire, ou le nœud d'origine si aucune modification n'est nécessaire.</returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Left is UnaryExpression unaryLeft
                && unaryLeft.Operand is MemberExpression memberExpression
                && node.Right is UnaryExpression unaryRight
                && unaryRight.Operand is ConstantExpression constant
                )
            {
                Expression property = memberExpression;
                object? objvalue = constant.Value;
                if (memberExpression.Type == typeof(string) && objvalue != null && ignoreCaseInStringComparison)
                    objvalue = ((string)objvalue).ToLower();
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                Expression constantExpression = Expression.Constant(objvalue, memberExpression.Type);
                
                if (useDefaultValueForNull)
                {
                    var getValueOrDefaultMethod = memberExpression.Type.GetMethod("GetValueOrDefault", Type.EmptyTypes);
                    Expression getValueOrDefaultExpression;
                    if (memberExpression.Type == typeof(string))
                    {
                        getValueOrDefaultExpression = Expression.Coalesce(property, Expression.Constant(string.Empty));
                        if (ignoreCaseInStringComparison)
                            getValueOrDefaultExpression = Expression.Call(getValueOrDefaultExpression, toLowerMethod!);
                    }
                    else if (Nullable.GetUnderlyingType(memberExpression.Type) != null)
                    {
                        getValueOrDefaultExpression = Expression.Call(property, getValueOrDefaultMethod!);
                    }
                    else
                        return Expression.MakeBinary(node.NodeType, memberExpression, constantExpression);

                    Expression getValueOrDefaultExpressionConvert = Expression.Convert(getValueOrDefaultExpression, memberExpression.Type);

                    var comparisonWhithgetValueOrDefault = Expression.MakeBinary(node.NodeType, getValueOrDefaultExpressionConvert, constantExpression);
                    return comparisonWhithgetValueOrDefault;
                }
                Expression comparison = Expression.MakeBinary(node.NodeType, property, constantExpression);
                if (ignoreCaseInStringComparison && memberExpression.Type == typeof(string))
                {
                    comparison = Expression.MakeBinary(node.NodeType, Expression.Call(property, toLowerMethod!), constantExpression);
                }
                if (Nullable.GetUnderlyingType(memberExpression.Type) == null)
                    return comparison;
                var condition = Expression.Condition(
                    Expression.Equal(property, Expression.Constant(null)),
                    Expression.Constant(false),
                    comparison
                    );
                return condition;
            }

            return base.VisitBinary(node);
        }
    }
}