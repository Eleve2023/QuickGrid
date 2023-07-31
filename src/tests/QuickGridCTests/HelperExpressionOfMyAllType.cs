using Shared;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace QuickGridCTests
{
    public class HelperExpressionOfMyAllType<T>
    {
        public static PropertyInfo[] GetPropertyInfos()
        {
            return typeof(MyAllType).GetProperties();
        }
        public static Expression<Func<MyAllType, T>> GetExpression(PropertyInfo propertyInfo) 
        {
            var parameter = Expression.Parameter(typeof(MyAllType), "x");
            var memberExpression = Expression.Property(parameter, propertyInfo.Name);
            return Expression.Lambda<Func<MyAllType, T>>(memberExpression, parameter);
        }
    }
}
