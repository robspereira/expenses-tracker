using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Expense_Tracker.Models;

public class User: IdentityUser
{
    [Column(TypeName = "VARCHAR(255)")]
    public string ProfilePicture { get; set; } =
        "https://upload.wikimedia.org/wikipedia/commons/b/b5/Windows_10_Default_Profile_Picture.svg";
}