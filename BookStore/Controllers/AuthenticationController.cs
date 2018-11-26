using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class AuthenticationController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        private RoleManager<IdentityRole> _roleManager;

        public AuthenticationController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(Registration viewModel)
        {
			if(await _userManager.FindByEmailAsync("bepi@donne.it") == null)
			{
				var role = new IdentityRole("Admin");
				await _roleManager.CreateAsync(role);

				var role2 = new IdentityRole("User");
				await _roleManager.CreateAsync(role2);

				var admin = new IdentityUser
				{
					UserName = "Bepi",
					Email = "bepi@donne.it",
				};

				var result2 = await _userManager.CreateAsync(admin, "admin");

				await _userManager.AddToRoleAsync(admin, "Admin");
			}
            
            if (viewModel == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(viewModel);

            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user == null)
            {
                var newUser = new IdentityUser
                {
                    UserName = viewModel.Name,
                    Email = viewModel.Email,
                };

                var result = await _userManager.CreateAsync(newUser, viewModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "User");
                    return RedirectToAction("login", "Authentication");
                }
                else
                {
                    return new JsonResult(result.Errors);
                }
            }

            ModelState.AddModelError("", "User already exists!");
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login viewModel)
        {
            if (viewModel == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(viewModel);

            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user != null)
            {
                var checkPwd = await _userManager.CheckPasswordAsync(user, viewModel.Password);

                if (checkPwd)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("index", "author");
                }
            }

            ModelState.AddModelError("", "Invalid email and/or password!");
            return View(viewModel);
        }
     
        [Authorize] 
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }
       
    }
}