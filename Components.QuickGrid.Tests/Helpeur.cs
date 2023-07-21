using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Components.QuickGrid.Tests
{
    public static class Helpeur<T>
    {
        public static PropertyInfo[] GetPropertyInfos()
        {
            return typeof(MyGridItem).GetProperties();
        }
        public static Expression<Func<MyGridItem, T>> GetExpression(PropertyInfo propertyInfo)
        {
            var parameter = Expression.Parameter(typeof(MyGridItem), "x");
            var memberExpression = Expression.Property(parameter, propertyInfo.Name);
            return Expression.Lambda<Func<MyGridItem, T>>(memberExpression, parameter);
        }
    }
}
