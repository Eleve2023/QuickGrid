// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

using System.Linq.Expressions;

namespace Components.QuickGrid.Infrastructure;

internal sealed class ParameterReplacer : ExpressionVisitor
{
    private readonly ParameterExpression _oldParameter;
    private readonly ParameterExpression _newParameter;

    private ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        _oldParameter = oldParameter;
        _newParameter = newParameter;
    }

    public static Expression Replace(Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        return new ParameterReplacer(oldParameter, newParameter).Visit(expression);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (node == _oldParameter)
        {
            return _newParameter;
        }

        return base.VisitParameter(node);
    }
}
