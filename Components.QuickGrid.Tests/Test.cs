using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Components.QuickGrid.Tests
{
    public class Test: TestContext
    {        
        private readonly ICollection<MyGridItem> _items;
        
        public Test()
        {
            _items = MyGridItemService.GetMyGridItems();
        }
        [Fact]
        public void ExempleTest()
        {
            var lines = new List<string> { "Hello", "World" };

        }
        
    }
}
