using AutoMapper;
using Dem.BLL.Interfaces;
using Dem.DAL.Models;
using Dem.PL.Helpers;
using Dem.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dem.PL.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue)) 
            {
                var users = _userManager.Users;
                var MappedUsers = users.Select(user => new ApplicationUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email
                }).ToList();
                return View(MappedUsers);
            }
            

            var user = await _userManager.FindByEmailAsync(searchValue);
            if (user == null)
                return View(new List<ApplicationUserViewModel>());

            var MappedUser = new ApplicationUserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
            return View(new List<ApplicationUserViewModel>(){ MappedUser });
        }
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();   //Status Code 400
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();
            var MappedUser = new ApplicationUserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
            return View(ViewName, MappedUser);
        }
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUserViewModel userVm, [FromRoute] string id)
        {
            if (id != userVm.Id)
                return BadRequest();

            if (ModelState.IsValid) //Server Side Validation
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user is null)
                        return NotFound();
                    userVm.Id = id;
                    user.UserName = userVm.UserName;
                    user.NormalizedUserName = userVm.UserName.ToUpper();
                    userVm.PhoneNumber = userVm.PhoneNumber;
                    var Result = await _userManager.UpdateAsync(user);
                    if (Result.Succeeded)
                        return RedirectToAction(nameof(Index));

                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                catch (Exception ex)
                {
                    //ModelState.AddModelError(string.Empty, ex.Message);
                    throw; //Save Stack Trase
                }
            }
            return View(userVm);
        }

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ApplicationUserViewModel userVm, [FromRoute] string id)
        {
            if (id != userVm.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user is null)
                        return NotFound();
                    var Result = await _userManager.DeleteAsync(user);
                    if (Result.Succeeded)
                        return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(userVm);
        }
    }
}
