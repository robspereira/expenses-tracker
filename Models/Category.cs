using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Expense_Tracker.Models;

public class Category
{
    [Key] 
    public int CategoryId { get; set; }
    
    [Column(TypeName = "varchar(50)")]
    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; }

    [Column(TypeName = "varchar(5)")] 
    public string Icon { get; set; } = "";

    [Column(TypeName = "varchar(10)")] public string Type { get; set; } = "Expense";
    
    [NotMapped]
    public string? TitleWithIcon {
        get
        {
            return this.Icon + " " + this.Title;
        }
        
    }

}