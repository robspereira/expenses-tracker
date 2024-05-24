using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Expense_Tracker.Models;

public class User : IdentityUser
{
    public double balance { get; set; }
}