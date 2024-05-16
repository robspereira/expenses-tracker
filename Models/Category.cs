using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Expense_Tracker.Models;

public class Category
{
    [Key] 
    public int CategoryId { get; set; }
    
    [Column(TypeName = "nvarchar(50)")]
    public string Title { get; set; }

    [Column(TypeName = "nvarchar(5)")] 
    public string Icon { get; set; } = "";

    [Column(TypeName = "nvarchar(10)")] public string Type { get; set; } = "Expense";

}