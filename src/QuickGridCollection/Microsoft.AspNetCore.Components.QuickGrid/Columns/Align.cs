// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

namespace Components.QuickGrid;

/// <summary>
/// Describes alignment for a <see cref="QuickGridC{TGridItem}"/> column.
/// </summary>
public enum Align
{
    /// <summary>
    /// 
    /// </summary>
    Default,

    /// <summary>
    /// Justifies the content against the start of the container.
    /// </summary>
    Start,

    /// <summary>
    /// Justifies the content at the center of the container.
    /// </summary>
    Center,

    /// <summary>
    /// Justifies the content at the end of the container.
    /// </summary>
    End,

    /// <summary>
    /// Justifies the content against the left of the container.
    /// </summary>
    Left,

    /// <summary>
    /// Justifies the content at the right of the container.
    /// </summary>
    Right,
}
