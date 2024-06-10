using Expense_Tracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Expense_Tracker.Controllers;

[Route("user")]
[ApiController]
public class UserController(UserManager<User> sm, SignInManager<User> um) : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View(new User());
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        string message = "";
        IdentityResult result = new();
        try
        {
            User _user = new User
            {
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                Email = user.Email
            };
             result = await _userManager.CreateAsync(_user);

             if (!result.Succeeded)
             {
                 return BadRequest(result);
             }

             message = "Registro feito com sucesso.";
             
        } catch (Exception ex)
        {
            return BadRequest("Algo saiu errado!" + ex.Message);
        }

        return View(user);
    }
    
    [HttpGet("login")]
    public IActionResult Login()
    {
        return View(new Login());
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(Login login)
    {
        string message = "";
        SignInResult result = new();
        
        try
        {
            User _user = await _userManager.FindByEmailAsync(login.Email);
            if (_user != null && !_user.EmailConfirmed)
            {
                _user.EmailConfirmed = true;
            }
            result = await _signInManager.PasswordSignInAsync(_user.Email, login.Password, login.RememberMe,
                lockoutOnFailure: false);
        }
        catch(Exception ex)
        {
            return BadRequest(result);
        }

        return View(login);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}