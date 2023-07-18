// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

namespace Components.QuickGrid.QuickGridCollection;
internal sealed class QuickGridCException : Exception
{
    public QuickGridCException()
    {
    }

    public QuickGridCException(string? message) : base(message)
    {
    }

    public QuickGridCException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
