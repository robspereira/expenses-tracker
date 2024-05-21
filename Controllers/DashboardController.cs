using System.Globalization;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: DashboardController
        public async Task<ActionResult> Index()
        {
            DateTime StartDate = DateTime.Today.AddDays(-6);
            System.Console.WriteLine(StartDate);
            DateTime utcDate = StartDate.Kind == DateTimeKind.Utc ? StartDate : StartDate.ToUniversalTime();
            StartDate = utcDate;
            System.Console.WriteLine(StartDate);
            
            DateTime EndDate = DateTime.Today;
            System.Console.WriteLine(EndDate);
            utcDate = EndDate.Kind == DateTimeKind.Utc ? EndDate : EndDate.ToUniversalTime();
            EndDate = utcDate;
            System.Console.WriteLine(EndDate);
                
            List<Transaction> SelectedTransactions = await _context.Transactions
                .Include(x => x.Category)
                .Where(y => y.Date >= StartDate && y.Date <= EndDate)
                .ToListAsync();
            
            //Total income
            int TotalIncome = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");
            
            int TotalExpense = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .Sum(j => j.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C0");
            
            //Balance
            int Balance = TotalIncome - TotalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C0}", Balance);
            
            //Doughnut
            ViewBag.DoughnutChartData = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Category.CategoryId)
                .Select(k => new
                {
                    categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
                    amount = k.Sum(j => j.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
                }).OrderByDescending(l => l.amount)
                .ToList();
            
            //Spline Chart
            
            //Income
            //Income
            List<SplineChartData> IncomeSummary = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    income = k.Sum(l => l.Amount)
                })
                .ToList();

            //Expense
            List<SplineChartData> ExpenseSummary = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(l => l.Amount)
                })
                .ToList();

            //Combine Income & Expense
            string[] Last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).Date.ToString("dd/MM"))
                .ToArray();
            System.Console.WriteLine(Last7Days);

            ViewBag.SplineChartData = from day in Last7Days
                                    join income in IncomeSummary on day equals income.day into dayIncomeJoined
                                    from income in dayIncomeJoined.DefaultIfEmpty()
                                    join expense in ExpenseSummary on day equals expense.day into expenseJoined
                                    from expense in expenseJoined.DefaultIfEmpty()
                                    select new
                                    {
                                        day = day,
                                        income = income == null ? 0 : income.income,
                                        expense = expense == null ? 0 : expense.expense,
                                    };
            
            return View();
        }

    }

    public class SplineChartData
    {
        public string day;
        public int income;
        public int expense;
    }
    
    
}