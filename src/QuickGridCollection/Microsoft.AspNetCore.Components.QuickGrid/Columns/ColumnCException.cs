// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

namespace Components.QuickGrid.QuickGridCollection.Columns;
internal sealed class ColumnCException : Exception
{
    public ColumnCException()
    {
    }

    public ColumnCException(string? message) : base(message)
    {
    }

    public ColumnCException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
