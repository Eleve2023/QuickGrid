using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace SimpeQuickGrid.Data
{
    public class Person
    {
        [Key] public Guid Id { get; set; }

        [StringLength(50)] public string? FirstName { get; set; } 

        [StringLength(50)] public string? LastName { get; set; } 
        public DateOnly? BirthDate { get; set; }
        [Column(TypeName = "nvarchar")][StringLength(10)] public Gender? Gender { get; set; }
        public decimal? Sold { get; set; }
        public bool? Active { get; set; }
    }
}
