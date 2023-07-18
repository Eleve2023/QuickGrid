// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

namespace Components.QuickGrid.QuickGridCollection.Columns.MenuOptions;
internal sealed class MenuOptionException : Exception
{
    public MenuOptionException()
    {
    }

    public MenuOptionException(string? message) : base(message)
    {
    }

    public MenuOptionException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

}
