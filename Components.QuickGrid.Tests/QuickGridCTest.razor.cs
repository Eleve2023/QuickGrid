using Castle.Components.DictionaryAdapter.Xml;
using Components.QuickGrid.Columns;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace Components.QuickGrid.Tests
{    
    public class QuickGridCTest
    {
        private object gridInit = null!;
        private List<object> columnsInit = new List<object>();

        //[Fact]
        //public void AddColumn_AddsColumnToColumns()
        //{
        //    // Arrange            
        //    //var grid = new QuickGridC<MyGridItem>();
        //    var column = new PropertyColumnC<MyGridItem, string>();

        //    var type = typeof(QuickGridC<MyGridItem>);
        //    var mgrid = Activator.CreateInstance(type);

        //    MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name == "AddColumn").First();
        //    var columns = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name == "_columns").First();
        //    var _collectingColumns = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name == "_collectingColumns").First();
        //    _collectingColumns.SetValue(mgrid, true);
        //    //Act

        //    method.Invoke(mgrid, new object[] { column });
        //    List<ColumnCBase<MyGridItem>> _columns = (List<ColumnCBase<MyGridItem>>)columns.GetValue(mgrid);
        //    // Act
        //    //grid.AddColumn(column);

        //    // Assert
        //    Assert.Contains(column, _columns);
        //}
        [Fact]
        public void InitializedGridAndColumns()
        {
            var typeofMyGridItem = typeof(MyGridItem);            
            var typeofQuickGridC = typeof(QuickGridC<MyGridItem>);

            gridInit = Activator.CreateInstance(typeofQuickGridC)!;
            var fieldInfo_internalGridContext = typeofQuickGridC.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(e => e.Name == "_internalGridContext").First();
            
            var _internalGridContext = fieldInfo_internalGridContext.GetValue(gridInit);

            columnsInit = new List<object>();
            
            for (int i = 0; i < typeofMyGridItem.GetProperties().Length; i++)
            {
                var cloumn = Activator.CreateInstance("Components.QuickGrid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\r\n",
                    $"Components.QuickGrid.PropertyColumnC`2[[{typeofMyGridItem.AssemblyQualifiedName}]," +
                    $"[{typeofMyGridItem.GetProperties()[i].PropertyType.AssemblyQualifiedName}]]")!;
                
                

                var internalGridContext = cloumn!.Unwrap().GetComponentType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Where(e => e.Name == "InternalGridContext").First();

                var property = cloumn.Unwrap().GetComponentType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(e => e.Name == "Property").First();
                
                var parameter = Expression.Parameter(typeofMyGridItem, "x");
                var memberExpression = Expression.Property(parameter, typeofMyGridItem.GetProperties()[i].Name);
                var expression = Expression.Lambda(memberExpression, parameter);

                internalGridContext.SetValue(cloumn.Unwrap(), _internalGridContext);
                property.SetValue(cloumn.Unwrap(), expression);
                                
                columnsInit.Add(cloumn.Unwrap()!);
            }
        }
        [Fact]
        public void CollectedColumnsAvecValeurParDefaut()
        {
            InitializedGridAndColumns();
            var fieldInfo_collectingColumns = gridInit.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name == "_collectingColumns").First();
            var fieldInfo_columns = gridInit.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name == "_columns").First();
            fieldInfo_collectingColumns.SetValue(gridInit, true);
            foreach (var column in columnsInit)
            {
                var onParametersSet = column.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(e => e.Name == "OnParametersSet").First();
                onParametersSet.Invoke(column, Array.Empty<object>());
            }
            fieldInfo_collectingColumns.SetValue(gridInit, false);
            
        }
        [Fact]
        public void AfficheLigneGrid()
        {
            CollectedColumnsAvecValeurParDefaut();

        }
        
    }
}
