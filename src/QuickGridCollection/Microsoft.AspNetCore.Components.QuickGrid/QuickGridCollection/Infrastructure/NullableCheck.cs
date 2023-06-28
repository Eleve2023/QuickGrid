using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Components.QuickGrid.QuickGridCollection.Infrastructure
{
    internal class NullableCheck : ExpressionVisitor
    {
        /// <summary>
        /// considère le type string nullable si il est null comme string vide
        /// </summary>
        private readonly bool stringNullIsStrngEmplty;

        private NullableCheck(bool stringNullIsStrngEmplty)
        {
            this.stringNullIsStrngEmplty = stringNullIsStrngEmplty;
        }

        /// <summary>
        /// ajoute des condition sur les type nullable, si est n'est pas null
        /// </summary>
        /// <param name="expression">l'expression a révise</param>
        /// <param name="stringNullIsStrngEmplty">considère le type string nullable si il est null comme string vide</param>
        /// <returns></returns>
        public static Expression AddNullCheck(Expression expression, bool stringNullIsStrngEmplty = false)
        {
            if (expression is LambdaExpression lambda && lambda.Body is BinaryExpression binaryExpression && binaryExpression.Right is ConstantExpression constant && constant.Value == null)
            {
                var type_ = lambda.Body.GetType();
                var _ = lambda.Body as BinaryExpression;
                return expression;
            }
            return new NullableCheck(stringNullIsStrngEmplty).Visit(expression);
        }
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Type.IsGenericType && node.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var getValueOrDefaultMethod = node.Type.GetMethod("GetValueOrDefault", Type.EmptyTypes);
                return Expression.Call(node, getValueOrDefaultMethod!);
            }
            return base.VisitMember(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.Operand is ConstantExpression constant && constant.Value == null)
            {
                return base.VisitUnary(node);
            }
            if (node.NodeType == ExpressionType.Convert && node.Operand.Type.IsGenericType && node.Operand.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var getValueOrDefaultMethod = node.Operand.Type.GetMethod("GetValueOrDefault", Type.EmptyTypes);
                var getValueOrDefaultCall = Expression.Call(node.Operand, getValueOrDefaultMethod!);
                return Expression.Convert(getValueOrDefaultCall, node.Type);
            }
            return base.VisitUnary(node);
        }
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Object != null && node.Object.Type == typeof(string))
            {
                if (stringNullIsStrngEmplty)
                {
                    return Expression.Call(
                    Expression.Coalesce(node.Object, Expression.Constant(string.Empty)),
                    node.Method,
                    node.Arguments);
                }
                else
                {
                    return Expression.Condition(
                    Expression.Equal(node.Object, Expression.Constant(null)),
                    Expression.Constant(false),
                    Expression.Call(node.Object, node.Method, node.Arguments));
                }
            }
            return base.VisitMethodCall(node);
        }
    }
}
