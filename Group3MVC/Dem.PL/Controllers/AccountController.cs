using System.Security.Claims;
using Dem.DAL.Models;
using Dem.PL.Helpers;
using Dem.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Dem.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //Register

        //BaseURL/Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        public async Task<IActionResult> Register(RegisterViewModel model) 
        {
            if (ModelState.IsValid) //Server Side Validation 
            {
                var User = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    FName = model.FName,
                    LName = model.LName,
                    IsAgree = model.IsAgree
                };
                //Save User in Database
             
                var Result = await _userManager.CreateAsync(User,model.Password);
                if (Result.Succeeded)
                    return RedirectToAction(nameof(Login));
                
                else 
                    foreach (var error in Result.Errors) 
                        ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        //Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Check if User Exist
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User != null)
                {
                    //Check if Password is Correct
                    var Result = await _userManager.CheckPasswordAsync(User, model.Password);
                    if (Result)
                    {
                        //Sign In
                        var LoginResult = await _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false);
                        if (LoginResult.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError(string.Empty, "Password is incorrect");
                }
                else
                    ModelState.AddModelError(string.Empty, "Email does not exist");

            }
            return View(model);
        }
        //SignOut
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public new  async Task<IActionResult> SignOut() 
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        //Forget Password
        public IActionResult ForgetPassword() 
        {
            return View();
        }
        //Reset Password
        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model) 
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(User); //Valid for only one time for this User
                  
                    //https://localhost:7191/Account/ResetPassword?email=abc%40gmail.com&token=sfjsldfjsldfjsldfjsldfjsldfjsldfjsldfjsldfj
                    var ResetPasswordLink = 
                        Url.Action
                        ("ResetPassword", "Account", new { email = model.Email,Token=token }, Request.Scheme);
                    var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Templates/EmailTemplate.html");
                    string htmlTemplate = System.IO.File.ReadAllText(templatePath);
                    htmlTemplate = htmlTemplate.Replace("{User.UserName}", User.UserName ?? "User");
                    htmlTemplate = htmlTemplate.Replace("{ResetPasswordLink}", ResetPasswordLink);
                    var email = new Email()
                    {
                        Subject = "Reset Password",
                        To = User.Email,
                        Body = htmlTemplate,
                    };
                    await EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }
                else 
                    ModelState.AddModelError(string.Empty, "Email is no Exists");
            }
                return View("ForgetPassword", model);
        }
        public IActionResult CheckYourInbox() 
        {
            return View();  
        }

        public IActionResult ResetPassword(string email ,string token) 
        {
            //Using Hidden Input in view 
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model) 
        {
            if (ModelState.IsValid) 
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var User = await _userManager.FindByEmailAsync(email);
                if (User is not null)
                {
                    var Result = await _userManager.ResetPasswordAsync(User, token, model.Password);
                    if (Result.Succeeded)
                        return RedirectToAction(nameof(Login));
                    else 
                        foreach (var error in Result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        //External Login (Google, Facebook, etc.)
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // الـ redirectUrl هو الـ callback path
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return  Challenge(properties, provider); // ده هيعمل redirect لـ Google
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/Home/Index");
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded)
                return LocalRedirect(returnUrl);

            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout)); // أضف action للـ Lockout لو عايز، أو غيره لـ Login
            }
            var email = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Email);
            if (email != null)
            {
                var user = new ApplicationUser
                {
                    UserName = email.Split('@')[0],
                    Email = email,
                    FName = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.GivenName),
                    LName = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Surname)
                };
                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
            }
            TempData["ExternalEmail"] = email;
            TempData["LoginProvider"] = info.LoginProvider;
            TempData["ProviderKey"] = info.ProviderKey;

            ViewData["ErrorMessage"] = "Something went wrong while signing in with external provider.";
            return View("Login");
        }
   
    }
}
