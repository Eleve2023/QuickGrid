// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpeQuickGrid.Data;

public class Person
{
    [Key] public int Id { get; set; }

    [StringLength(50)] public string FirstName { get; set; } = default!;

    [StringLength(50)] public string? LastName { get; set; } 
    public DateTimeOffset? BirthDate { get; set; }
    [Column(TypeName = "nvarchar")][StringLength(10)] public Gender? Gender { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Sold { get; set; }
    public bool? Active { get; set; }
}
