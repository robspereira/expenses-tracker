using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models;

public class User
{
    [Key] public int UserId { get; set; }

    [Column(TypeName = "varchar(60)")] public string Name { get; set; }

    [Column(TypeName = "varchar(50)")] public string Email { get; set; }
    
    [Column(TypeName = "varchar(50)")] public string Password { get; set; }

    public int TransactionId;
    public Transaction? Transaction;
    
}